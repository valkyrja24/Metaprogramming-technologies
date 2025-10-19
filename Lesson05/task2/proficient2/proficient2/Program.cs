using System;
using System.Diagnostics;

public static class SafeMath
{
    public static int SumAll(int[] values, bool safe)
    {
        int sum = 0;

        if (safe)
        {
            checked
            {
                foreach (var v in values)
                {
                    sum += v;
                }
            }
        }
        else
        {
            unchecked
            {
                foreach (var v in values)
                {
                    sum += v;
                }
            }
        }

        return sum;
    }
}

class Program
{
    static void Main(string[] args)
    {
        bool safe = true;

        if (args.Length > 0)
        {
            if (args[0] == "--safe")
                safe = true;
            else if (args[0] == "--fast")
                safe = false;
            else
            {
                Console.WriteLine("Unknown argument. Use --safe or --fast.");
                return;
            }
        }

        int[] numbers = new int[10_000_000];
        for (int i = 0; i < numbers.Length; i++)
            numbers[i] = 1;

        Stopwatch sw = new Stopwatch();
        sw.Start();
        try
        {
            int result = SafeMath.SumAll(numbers, safe);
            sw.Stop();
            Console.WriteLine($"Sum result: {result}");
        }
        catch (OverflowException)
        {
            sw.Stop();
            Console.WriteLine("Overflow occurred during sum.");
        }

        Console.WriteLine($"Elapsed time ({(safe ? "safe" : "fast")} mode): {sw.ElapsedMilliseconds} ms");

#if DEBUG
        Console.WriteLine("Debug mode: overflow checks may be enabled by <CheckForOverflowUnderflow>.");
#else
        Console.WriteLine("Release mode: overflow checks disabled unless explicitly using checked.");
#endif
    }
}
