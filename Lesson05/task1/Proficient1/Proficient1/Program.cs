using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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

    public static async Task<Dictionary<string, string>> ParseFileAsync(string path, CancellationToken ct)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("Configuration file not found.", path);

        string[] lines;
        try
        {
            lines = await Task.Run(() => File.ReadAllLines(path), ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }

        var result = new Dictionary<string, string>();

        for (int i = 0; i < lines.Length; i++)
        {
            ct.ThrowIfCancellationRequested();

            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                continue;

            try
            {
                var kv = ParseSetting(line);

                if (result.ContainsKey(kv.Key))
                    throw new InvalidOperationException($"Duplicate key '{kv.Key}' found in configuration.");

                result[kv.Key] = kv.Value;
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
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
    static async Task Main()
    {
        var cts = new CancellationTokenSource();
        try
        {
            var config = await ConfigParser.ParseFileAsync("settings.txt", cts.Token);

            foreach (var kv in config)
            {
                Console.WriteLine("{0} = {1}", kv.Key, kv.Value);
            }
        }
        catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
        {
            Console.WriteLine("Parsing was canceled by the user.");
        }
        catch (ConfigFormatException ex)
        {
            Console.WriteLine("Config error: {0}", ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error: {0}", ex.Message);
        }
        finally
        {
            cts.Dispose();
        }
    }
}
