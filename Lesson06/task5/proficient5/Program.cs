using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

class Program
{
    static string inboxPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inbox");
    static string processedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "processed");
    static string errorLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.log");
    static string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "daily_report.csv");
    static ConcurrentDictionary<string, Timer> debounceTimers = new ConcurrentDictionary<string, Timer>();
    static int debounceMs = 500;
    static int maxRetries = 3;
    static int retryDelayMs = 500;

    static void Main()
    {
        Directory.CreateDirectory(inboxPath);
        Directory.CreateDirectory(processedPath);

        FileSystemWatcher watcher = new FileSystemWatcher(inboxPath, "*.csv");
        watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
        watcher.Created += OnChanged;
        watcher.Changed += OnChanged;
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;

        Console.WriteLine("Watching folder: " + inboxPath);
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();

        watcher.Dispose();
    }

    static void OnChanged(object sender, FileSystemEventArgs e)
    {
        string safePath = GetSafePath(e.FullPath, inboxPath);
        if (safePath == null) return;

        var timer = debounceTimers.AddOrUpdate(safePath,
            path => new Timer(_ => ProcessFile(path), null, debounceMs, Timeout.Infinite),
            (path, oldTimer) =>
            {
                oldTimer.Change(debounceMs, Timeout.Infinite);
                return oldTimer;
            });
    }

    static string GetSafePath(string path, string root)
    {
        try
        {
            string fullPath = Path.GetFullPath(path);
            string rootFull = Path.GetFullPath(root);
            if (!fullPath.StartsWith(rootFull, StringComparison.OrdinalIgnoreCase))
                return null;
            return fullPath;
        }
        catch
        {
            return null;
        }
    }

    static void ProcessFile(string filePath)
    {
        debounceTimers.TryRemove(filePath, out _);

        if (!File.Exists(filePath))
            return;

        int attempt = 0;
        bool success = false;
        string newFilePath = null;

        while (attempt < maxRetries && !success)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                string dateSuffix = DateTime.Now.ToString("yyyyMMdd");
                string destFileName = $"{fileName}-{dateSuffix}{extension}";
                newFilePath = Path.Combine(processedPath, destFileName);

                string tempFile = newFilePath + ".tmp";
                File.Copy(filePath, tempFile, true);
                File.Replace(tempFile, newFilePath, null);
                File.Delete(filePath);

                Console.WriteLine($"Processed: {newFilePath}");
                success = true;
            }
            catch (IOException)
            {
                attempt++;
                Thread.Sleep(retryDelayMs);
            }
            catch (UnauthorizedAccessException)
            {
                attempt++;
                Thread.Sleep(retryDelayMs);
            }
        }

        string status = success ? "OK" : "ERROR";
        if (!success)
        {
            string msg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Failed to process: {filePath}";
            Console.WriteLine(msg);
            File.AppendAllText(errorLogPath, msg + Environment.NewLine);
        }

        File.AppendAllText(reportPath,
            $"{DateTime.Now:yyyy-MM-dd HH:mm:ss};{filePath};{(newFilePath ?? "")};{status}" + Environment.NewLine);
    }
}
