using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TextFormatter
{
    public Func<string, string> Strategy { get; set; }

    public TextFormatter()
    {
        Strategy = s => s;
    }

    public IEnumerable<string> FormatAll(IEnumerable<string> texts)
    {
        if (texts == null)
            throw new ArgumentNullException(nameof(texts));

        foreach (var text in texts)
            yield return Strategy(text);
    }

    public static string TrimToUpper(string s) => s.Trim().ToUpper();

    public static string MaskDigits(string s) => Regex.Replace(s, @"\d", "*");
}

class Program
{
    static void Main()
    {
        var formatter = new TextFormatter();

        var lines = new List<string> { "  hello123 ", " world456 " };

        foreach (var line in formatter.FormatAll(lines))
            Console.WriteLine(line);

        formatter.Strategy = TextFormatter.TrimToUpper;
        foreach (var line in formatter.FormatAll(lines))
            Console.WriteLine(line);

        formatter.Strategy = TextFormatter.MaskDigits;
        foreach (var line in formatter.FormatAll(lines))
            Console.WriteLine(line);

        formatter.Strategy = s =>
        {
            char[] chars = s.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        };
        foreach (var line in formatter.FormatAll(lines))
            Console.WriteLine(line);
    }
}
