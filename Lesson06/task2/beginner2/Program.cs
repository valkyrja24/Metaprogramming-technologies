using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string filePath = "appsettings.txt";

        var configLines = new List<string>
        {
            "AppName=MyApplication",
            "Version=1.0.0",
            "EnableLogging=True"
        };

        using (var fs = new FileStream(
            filePath,
            FileMode.Create,
            FileAccess.Write,   
            FileShare.None))  
        {
            using (var writer = new StreamWriter(fs, new UTF8Encoding(false)))
            {
                foreach (var line in configLines)
                {
                    writer.WriteLine(line);
                }

                writer.Flush();
                fs.Flush(true);
            }
        }

        Console.WriteLine($"Configuration saved to {filePath}");
    }
}

