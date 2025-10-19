using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TextFormatter
{
    private List<Func<string, string>> _pipeline = new List<Func<string, string>>();

    public Func<string, string> Strategy { get; set; }

    public TextFormatter()
    {
        Strategy = s => s;
    }

    public void SetPipeline(params Func<string, string>[] steps)
    {
        if (steps == null) throw new ArgumentNullException(nameof(steps));
        _pipeline = new List<Func<string, string>>(steps);
    }

    public IEnumerable<string> FormatAll(IEnumerable<string> texts)
    {
        if (texts == null) throw new ArgumentNullException(nameof(texts));

        foreach (var text in texts)
        {
            string result = text;
            try
            {
                if (_pipeline.Count > 0)
                {
                    foreach (var step in _pipeline)
                        result = step(result);
                }
                else
                {
                    result = Strategy(result);
                }
            }
            catch (Exception ex)
            {
                result = $"Error: {ex.Message}";
            }

            yield return result;
        }
    }

    public static string TrimToUpper(string s) => s.Trim().ToUpper();

    public static string MaskDigits(string s) => Regex.Replace(s, @"\\d", "*");
}

class Program
{
    static void Main()
    {
        var formatter = new TextFormatter();

        var lines = new List<string> { "  hello123 ", " world456 ", null };

        formatter.SetPipeline(
            TextFormatter.TrimToUpper,
            TextFormatter.MaskDigits,
            s => s + "!"
        );

        foreach (var line in formatter.FormatAll(lines))
            Console.WriteLine(line);
    }
}
