using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

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
            "Version=1.1.0",
            "EnableLogging=True",
            "MaxUsers=100"
        };

        try
        {
            using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(fs, new UTF8Encoding(false)))
            {
                foreach (var line in configLines)
                {
                    writer.WriteLine(line);
                }
                writer.Flush();
                fs.Flush(true);
            }

            File.Replace(tempPath, filePath, backupPath, ignoreMetadataErrors: true);

            Console.WriteLine($"Configuration updated successfully.");
            Console.WriteLine($"Backup saved at: {backupPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during update: " + ex.Message);
            if (File.Exists(tempPath)) File.Delete(tempPath);
        }
    }
}
