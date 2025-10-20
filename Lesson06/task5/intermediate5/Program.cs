using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

class Program
{
    static string inboxPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inbox");
    static string processedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "processed");
    static string errorLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.log");
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
        var timer = debounceTimers.AddOrUpdate(e.FullPath,
            path => new Timer(_ => ProcessFile(path), null, debounceMs, Timeout.Infinite),
            (path, oldTimer) =>
            {
                oldTimer.Change(debounceMs, Timeout.Infinite);
                return oldTimer;
            });
    }

    static void ProcessFile(string filePath)
    {
        debounceTimers.TryRemove(filePath, out _);

        if (!File.Exists(filePath))
            return;

        int attempt = 0;
        bool success = false;

        while (attempt < maxRetries && !success)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                string dateSuffix = DateTime.Now.ToString("yyyyMMdd");
                string newFileName = $"{fileName}-{dateSuffix}{extension}";
                string destPath = Path.Combine(processedPath, newFileName);

                File.Move(filePath, destPath);
                Console.WriteLine($"Processed: {destPath}");
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

        if (!success)
        {
            string msg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Failed to process: {filePath}";
            Console.WriteLine(msg);
            File.AppendAllText(errorLogPath, msg + Environment.NewLine);
        }
    }
}
