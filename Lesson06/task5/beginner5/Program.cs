using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

class Program
{
    static string inboxPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inbox");
    static string processedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "processed");
    static ConcurrentDictionary<string, Timer> debounceTimers = new ConcurrentDictionary<string, Timer>();

    static void Main()
    {
        Directory.CreateDirectory(inboxPath);
        Directory.CreateDirectory(processedPath);

        FileSystemWatcher watcher = new FileSystemWatcher(inboxPath, "*.csv");
        watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
        watcher.Created += OnChanged;
        watcher.Changed += OnChanged;
        watcher.IncludeSubdirectories = false;
        watcher.EnableRaisingEvents = true;

        Console.WriteLine("Watching folder: " + inboxPath);
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();

        watcher.Dispose();
    }

    static void OnChanged(object sender, FileSystemEventArgs e)
    {
        var timer = debounceTimers.AddOrUpdate(e.FullPath,
            path => new Timer(_ => ProcessFile(path), null, 500, Timeout.Infinite),
            (path, oldTimer) =>
            {
                oldTimer.Change(500, Timeout.Infinite);
                return oldTimer;
            });
    }

    static void ProcessFile(string filePath)
    {
        debounceTimers.TryRemove(filePath, out _);

        if (!File.Exists(filePath))
            return;

        try
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);
            string dateSuffix = DateTime.Now.ToString("yyyyMMdd");
            string newFileName = $"{fileName}-{dateSuffix}{extension}";
            string destPath = Path.Combine(processedPath, newFileName);

            File.Move(filePath, destPath);

            Console.WriteLine($"Processed: {destPath}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
        }
    }
}
