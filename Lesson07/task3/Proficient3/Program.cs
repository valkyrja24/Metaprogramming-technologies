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
            yield return word;
        }
    }

    public static string Truncate(this string s, int max)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        return s.Length <= max ? s : s.Substring(0, max);
    }

    public static string Capitalize(this string s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;
        return char.ToUpper(s[0]) + s.Substring(1);
    }
}

class Program
{
    static void Main(string[] args)
    {
        string text = "  Hello   world! This is a TEST. Hello world! 123 123 ";

        var words = text
            .OrEmpty()
            .NormalizeSpaces()
            .Words()
            .Select(w => TruncateSafe(w, 10))
            .Select(w => CapitalizeSafe(w))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(w => w, StringComparer.OrdinalIgnoreCase)
            .ThenBy(w => w.Length)
            .ToList();

        foreach (var w in words)
        {
            Console.WriteLine(w);
        }

        var lookup = words.ToLookup(w => char.ToUpper(w[0]), EqualityComparer<char>.Default);

        foreach (var group in lookup)
        {
            Console.WriteLine("Key: {0} -> {1}", group.Key, string.Join(", ", group));
        }
    }

    static string TruncateSafe(string s, int max)
    {
        try
        {
            return s.Truncate(max);
        }
        catch
        {
            return string.Empty;
        }
    }

    static string CapitalizeSafe(string s)
    {
        try
        {
            return s.Capitalize();
        }
        catch
        {
            return string.Empty;
        }
    }
}
