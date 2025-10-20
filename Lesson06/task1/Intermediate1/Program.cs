using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

class Program
{
    const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

    static void Main()
    {
        Console.Write("Enter source directory: ");
        var sourceDir = Console.ReadLine()?.Trim();
        Console.Write("Enter destination directory: ");
        var destDir = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(sourceDir) || string.IsNullOrWhiteSpace(destDir))
        {
            Console.WriteLine("Invalid input.");
            return;
        }

        if (!Directory.Exists(sourceDir))
        {
            Console.WriteLine("Source directory does not exist.");
            return;
        }

        if (!Directory.Exists(destDir))
            Directory.CreateDirectory(destDir);

        var csvLines = new List<string>();
        csvLines.Add("relative_path;lines;spaces_removed");

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
            var destFile = Path.Combine(destDir, relativePath);
            var destFolder = Path.GetDirectoryName(destFile);
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);

            destFile = GetNonConflictingPath(destFile);

            var report = ProcessFile(file, destFile);
            totalLines += report.LinesProcessed;
            totalSpaces += report.SpacesRemoved;

            csvLines.Add($"{relativePath};{report.LinesProcessed};{report.SpacesRemoved}");
        }

        var csvPath = Path.Combine(destDir, "report.csv");
        File.WriteAllLines(csvPath, csvLines, new UTF8Encoding(false));

        Console.WriteLine();
        Console.WriteLine($"Total lines processed: {totalLines}");
        Console.WriteLine($"Total trailing spaces removed: {totalSpaces}");
        Console.WriteLine($"CSV report saved: {csvPath}");
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

    static (long LinesProcessed, long SpacesRemoved) ProcessFile(string sourceFile, string destinationFile)
    {
        long linesCount = 0;
        long spacesRemoved = 0;

        using (var reader = new StreamReader(sourceFile, DetectEncoding(sourceFile)))
        using (var writer = new StreamWriter(destinationFile, false, new UTF8Encoding(false)))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var trimmed = TrimEndSpaces(line, out var removed);
                spacesRemoved += removed;
                writer.Write(trimmed);
                writer.Write('\n');
                linesCount++;
            }
        }

        return (linesCount, spacesRemoved);
    }

    static Encoding DetectEncoding(string filename)
    {
        using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
        {
            if (fs.Length >= 3)
            {
                byte[] bom = new byte[3];
                fs.Read(bom, 0, 3);
                if (bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF)
                    return new UTF8Encoding(true);
            }
        }
        return new UTF8Encoding(false);
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
}
