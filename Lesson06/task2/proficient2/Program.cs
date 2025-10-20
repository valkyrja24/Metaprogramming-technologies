using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main()
    {
        string filePath = "appsettings.txt";
        string backupPath = filePath + ".bak";
        string tempPath = filePath + ".tmp";

        var configLines = new List<string>
        {
            "AppName=MyApplication",
            "Version=2.0.0",
            "EnableLogging=True",
            "MaxUsers=200"
        };

        int maxRetries = 5;
        int delayMs = 200;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                using (var fs = new FileStream(
                    tempPath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.Read))
                using (var writer = new StreamWriter(fs, new UTF8Encoding(false)))
                {
                    foreach (var line in configLines)
                        writer.WriteLine(line);

                    writer.Flush();
                    fs.Flush(true);
                }

                if (File.Exists(filePath))
                    File.Replace(tempPath, filePath, backupPath, ignoreMetadataErrors: true);
                else
                    File.Move(tempPath, filePath);

                Console.WriteLine("Configuration updated successfully.");
                Console.WriteLine("Backup saved at: " + backupPath);
                break;
            }
            catch (IOException) { if (attempt == maxRetries) throw; Thread.Sleep(delayMs * attempt); }
            catch (UnauthorizedAccessException) { if (attempt == maxRetries) throw; Thread.Sleep(delayMs * attempt); }
        }
    }
}
