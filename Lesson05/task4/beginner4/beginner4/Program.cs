using System;
using System.Text.RegularExpressions;

public static class StringTransformer
{
    public static string Transform(string s, Func<string, string> strategy)
    {
        if (s == null)
            throw new ArgumentNullException(nameof(s));

        if (strategy == null)
            throw new ArgumentNullException(nameof(strategy));

        return strategy(s);
    }

    public static string TrimToUpper(string s) => s.Trim().ToUpper();

    public static string MaskDigits(string s) => Regex.Replace(s, @"\d", "*");
}

class Program
{
    static void Main()
    {
        string input = "  Hello123 World  ";

        string upper = StringTransformer.Transform(input, StringTransformer.TrimToUpper);
        Console.WriteLine("TrimToUpper: " + upper);

        string masked = StringTransformer.Transform(input, StringTransformer.MaskDigits);
        Console.WriteLine("MaskDigits: " + masked);

        string reversed = StringTransformer.Transform(input, s =>
        {
            char[] chars = s.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        });
        Console.WriteLine("Reversed (lambda): " + reversed);
    }
}
