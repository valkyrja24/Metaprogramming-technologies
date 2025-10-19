using System;

public static class ConfigParser
{
    public static (string Key, string Value) ParseSetting(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            throw new ArgumentNullException(nameof(line), "The line cannot be null or empty");

        int eqIndex = line.IndexOf('=');
        if (eqIndex < 0)
            throw new FormatException($"Invalid configuration line: '{line}' (parameter: {nameof(line)})");

        string key = line.Substring(0, eqIndex).Trim();
        string value = line.Substring(eqIndex + 1).Trim();

        return (key, value);
    }
}

class Program
{
    static void Main()
    {
        try
        {
            var setting = ConfigParser.ParseSetting("username=admin");
            Console.WriteLine($"Key: {setting.Key}, Value: {setting.Value}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
