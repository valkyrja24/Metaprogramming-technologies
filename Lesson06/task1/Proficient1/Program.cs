using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

class Program
{
    const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: <sourceDir> <destDir> [--lf|--crlf] [--keep-invalid]");
            return;
        }

        string sourceDir = args[0];
        string destDir = args[1];

        string newline = "\n";
        bool keepInvalid = false;

        for (int i = 2; i < args.Length; i++)
        {
            if (args[i].Equals("--lf", StringComparison.OrdinalIgnoreCase)) newline = "\n";
            if (args[i].Equals("--crlf", StringComparison.OrdinalIgnoreCase)) newline = "\r\n";
            if (args[i].Equals("--keep-invalid", StringComparison.OrdinalIgnoreCase)) keepInvalid = true;
        }

        if (!Directory.Exists(sourceDir))
        {
            Console.WriteLine("Source directory does not exist.");
            return;
        }

        Directory.CreateDirectory(destDir);

        string invalidDir = Path.Combine(destDir, "invalid");
        if (keepInvalid) Directory.CreateDirectory(invalidDir);

        var csvLines = new List<string>();
        csvLines.Add("relative_path;lines;spaces_removed;invalid_utf8");

        long totalLines = 0;
        long totalSpaces = 0;

        foreach (var file in Directory.GetFiles(sourceDir, "*.txt", SearchOption.AllDirectories))
        {
            var fileInfo = new FileInfo(file);
            if (fileInfo.Length > MaxFileSize)
            {
                Console.WriteLine($"Skipping (too large): {file}");
                continue;
            }

            string relativePath = GetRelativePath(file, sourceDir);
            string destFile = Path.Combine(destDir, relativePath);
            string destFolder = Path.GetDirectoryName(destFile);
            if (!Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);

            destFile = GetNonConflictingPath(destFile);

            bool invalidUtf8 = false;
            try
            {
                var report = ProcessFile(file, destFile, newline, out invalidUtf8);
                totalLines += report.LinesProcessed;
                totalSpaces += report.SpacesRemoved;

                csvLines.Add($"{relativePath};{report.LinesProcessed};{report.SpacesRemoved};{invalidUtf8}");

                if (invalidUtf8 && keepInvalid)
                {
                    string invalidPath = Path.Combine(invalidDir, Path.GetFileName(file));
                    File.Copy(file, invalidPath, true);
                }
            }
            catch (DecoderFallbackException)
            {
                invalidUtf8 = true;
                Console.WriteLine($"Invalid UTF-8 detected: {file}");
                csvLines.Add($"{relativePath};0;0;{invalidUtf8}");

                if (keepInvalid)
                {
                    string invalidPath = Path.Combine(invalidDir, Path.GetFileName(file));
                    File.Copy(file, invalidPath, true);
                }
            }
        }

        string csvPath = Path.Combine(destDir, "report.csv");
        File.WriteAllLines(csvPath, csvLines, new UTF8Encoding(false));

        Console.WriteLine();
        Console.WriteLine($"Total lines processed: {totalLines}");
        Console.WriteLine($"Total trailing spaces removed: {totalSpaces}");
        Console.WriteLine($"CSV report saved: {csvPath}");
    }

    static (long LinesProcessed, long SpacesRemoved) ProcessFile(string sourceFile, string destinationFile, string newline, out bool invalidUtf8)
    {
        invalidUtf8 = false;
        long linesCount = 0;
        long spacesRemoved = 0;

        string tempFile = destinationFile + ".tmp";

        Encoding enc = new UTF8Encoding(false, true); // strict UTF-8

        using (var reader = new StreamReader(sourceFile, enc))
        using (var writer = new StreamWriter(tempFile, false, new UTF8Encoding(false)))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var trimmed = TrimEndSpaces(line, out int removed);
                spacesRemoved += removed;
                writer.Write(trimmed);
                writer.Write(newline);
                linesCount++;
            }
        }

        if (File.Exists(destinationFile))
            File.Delete(destinationFile);
        File.Move(tempFile, destinationFile);

        return (linesCount, spacesRemoved);
    }

    static string TrimEndSpaces(string input, out int removed)
    {
        int originalLength = input.Length;
        string trimmed = input.TrimEnd(' ', '\t');
        removed = originalLength - trimmed.Length;
        return trimmed;
    }

    static string GetNonConflictingPath(string path)
    {
        if (!File.Exists(path)) return path;

        var dir = Path.GetDirectoryName(path);
        var name = Path.GetFileNameWithoutExtension(path);
        var ext = Path.GetExtension(path);
        int counter = 1;

        while (true)
        {
            var newPath = Path.Combine(dir, $"{name}_{counter}{ext}");
            if (!File.Exists(newPath))
                return newPath;
            counter++;
        }
    }

    static string GetRelativePath(string fullPath, string basePath)
    {
        if (!basePath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            basePath += Path.DirectorySeparatorChar;

        Uri fileUri = new Uri(fullPath);
        Uri folderUri = new Uri(basePath);
        Uri relativeUri = folderUri.MakeRelativeUri(fileUri);
        return Uri.UnescapeDataString(relativeUri.ToString().Replace('/', Path.DirectorySeparatorChar));
    }
}
