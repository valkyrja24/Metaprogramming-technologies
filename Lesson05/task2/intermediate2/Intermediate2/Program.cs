using System;

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

        int[] numbers = { int.MaxValue, 1, 2 };

        try
        {
            int result = SafeMath.SumAll(numbers, safe);
            Console.WriteLine("Sum result: " + result);
        }
        catch (OverflowException)
        {
            Console.WriteLine("Overflow occurred during sum.");
        }
    }
}
