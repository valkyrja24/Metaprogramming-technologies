using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Text Counter");
        Console.WriteLine("Enter your text (finish with an empty line):");

        string input = "";
        string line;
        int lineCount = 0;

        //to finalize the input, we can use an empty line
        while (!string.IsNullOrEmpty(line = Console.ReadLine()))
        {
            input += line + "\n";
            lineCount++;
        }

        int charCount = input.Length;
        int visibleCharCount = input.Replace("\n", "").Length;
        int spaceCount = input.Split(' ').Length - 1;
        int wordCount = input.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;

        Console.WriteLine("\nCount Result");
        Console.WriteLine($"Words: {wordCount}");
        Console.WriteLine($"Spaces: {spaceCount}");
        Console.WriteLine($"Characters (all): {charCount}");
        Console.WriteLine($"Characters (visible): {visibleCharCount}");
        Console.WriteLine($"Lines: {lineCount}");
    }
}
