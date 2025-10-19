using System;
using System.Collections.Generic;
using System.IO;

public class ConfigFormatException : FormatException
{
    public int LineNumber { get; }

    public ConfigFormatException(string message, int lineNumber, Exception innerException = null)
        : base($"Line {lineNumber}: {message}", innerException)
    {
        LineNumber = lineNumber;
    }
}

public static class ConfigParser
{
    public static (string Key, string Value) ParseSetting(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            throw new ArgumentNullException(nameof(line), "The line cannot be null or empty");

        int eqIndex = line.IndexOf('=');
        if (eqIndex < 0)
            throw new FormatException($"Invalid configuration line: '{line}'");

        string key = line.Substring(0, eqIndex).Trim();
        string value = line.Substring(eqIndex + 1).Trim();

        return (key, value);
    }

    public static Dictionary<string, string> ParseFile(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Configuration file not found.", path);

        var result = new Dictionary<string, string>();
        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                continue;

            try
            {
                var (key, value) = ParseSetting(line);
                result[key] = value;
            }
            catch (Exception ex)
            {
                throw new ConfigFormatException(ex.Message, i + 1, ex);
            }
        }

        return result;
    }
}

class Program
{
    static void Main()
    {
        try
        {
            var config = ConfigParser.ParseFile("settings.txt");

            foreach (var kv in config)
            {
                Console.WriteLine($"{kv.Key} = {kv.Value}");
            }
        }
        catch (ConfigFormatException ex)
        {
            Console.WriteLine($"Config error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
