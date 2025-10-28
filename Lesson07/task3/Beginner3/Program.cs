using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

static class StringExtensions
{
    public static string OrEmpty(this string s)
    {
        return s ?? string.Empty;
    }

    public static string NormalizeSpaces(this string s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        return Regex.Replace(s, @"\s+", " ").Trim();
    }

    public static IEnumerable<string> Words(this string s)
    {
        if (string.IsNullOrEmpty(s)) yield break;
        foreach (var word in s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
        {
            yield return word.ToLowerInvariant();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        string text = "  Hello   world! This   is   a   TEST.  ";

        var result = text
            .OrEmpty()
            .NormalizeSpaces()
            .Words()
            .Distinct()
            .OrderBy(w => w);

        foreach (var word in result)
        {
            Console.WriteLine(word);
        }
    }
}
