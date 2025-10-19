using System;
using System.Collections.Generic;
using System.Globalization;

// Тип UserId
class UserId
{
    public string Value { get; }

    public UserId(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}

// Компаратор для UserId: ігноруємо регістр (OrdinalIgnoreCase)
class UserIdIgnoreCaseComparer : IEqualityComparer<UserId>
{
    public bool Equals(UserId x, UserId y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x == null || y == null) return false;
        return string.Equals(x.Value, y.Value, StringComparison.OrdinalIgnoreCase);
    }

    public int GetHashCode(UserId obj)
    {
        return obj.Value?.ToUpperInvariant().GetHashCode() ?? 0;
    }
}

class Program
{
    static void Main()
    {
        // --- 1. Dictionary<UserId, string> з UserIdIgnoreCaseComparer ---
        var userDict = new Dictionary<UserId, string>(new UserIdIgnoreCaseComparer());
        userDict.Add(new UserId("ADMIN"), "Administrator");

        Console.WriteLine("Dictionary<UserId,string> with OrdinalIgnoreCase:");
        Console.WriteLine($"Contains 'admin'? {userDict.ContainsKey(new UserId("admin"))}");

        // --- 2. Dictionary<string,int> з різними StringComparer ---
        var dictOrdinalIgnoreCase = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        dictOrdinalIgnoreCase["I"] = 1;

        var trCulture = new CultureInfo("tr-TR");
        var dictTrCulture = new Dictionary<string, int>(StringComparer.Create(trCulture, true));
        dictTrCulture["I"] = 1;

        Console.WriteLine("\nDictionary<string,int> OrdinalIgnoreCase vs Turkish culture:");
        Console.WriteLine($"OrdinalIgnoreCase contains 'i'? {dictOrdinalIgnoreCase.ContainsKey("i")}");
        Console.WriteLine($"Turkish tr-TR contains 'i'? {dictTrCulture.ContainsKey("i")}");
        Console.WriteLine($"Turkish tr-TR contains 'ı'? {dictTrCulture.ContainsKey("ı")}");

        // --- 3. SortedDictionary зі шведською культурою sv-SE і Ordinal ---
        var svCulture = new CultureInfo("sv-SE");

        var sortedSv = new SortedDictionary<string, int>(StringComparer.Create(svCulture, false));
        var sortedOrdinal = new SortedDictionary<string, int>(StringComparer.Ordinal);

        string[] letters = { "a", "z", "å", "ä", "ö" };

        foreach (var l in letters)
        {
            sortedSv[l] = 1;
            sortedOrdinal[l] = 1;
        }

        Console.WriteLine("\nSortedDictionary with Swedish culture (sv-SE):");
        foreach (var kv in sortedSv) Console.Write(kv.Key + " ");
        Console.WriteLine("\nSortedDictionary with Ordinal:");
        foreach (var kv in sortedOrdinal) Console.Write(kv.Key + " ");

        // --- Короткий звіт ---
        Console.WriteLine("\n\nReport:");
        Console.WriteLine(
            "UserIdIgnoreCaseComparer ensures dictionary keys are compared ignoring case (OrdinalIgnoreCase), " +
            "suitable for technical identifiers like usernames. " +
            "StringComparer.OrdinalIgnoreCase works similarly for string keys, but culture-aware comparers " +
            "like tr-TR show differences for 'I' vs 'ı'. " +
            "SortedDictionary with sv-SE culture orders letters according to Swedish rules, while Ordinal uses Unicode code points. " +
            "This demonstrates conscious choice between culture-aware and technical key comparison policies."
        );
    }
}
