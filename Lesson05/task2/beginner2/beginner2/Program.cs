using System;

public static class SafeMath
{
    public static int AddChecked(int a, int b)
    {
        checked
        {
            return a + b;
        }
    }

    public static int AddWrapped(int a, int b)
    {
        unchecked
        {
            return a + b;
        }
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Checked addition:");
        try
        {
            Console.WriteLine(SafeMath.AddChecked(int.MaxValue, 1));
        }
        catch (OverflowException)
        {
            Console.WriteLine("Overflow detected!");
        }

        Console.WriteLine("Wrapped addition:");
        Console.WriteLine(SafeMath.AddWrapped(int.MaxValue, 1));

        Console.WriteLine("Additional tests:");
        Console.WriteLine(SafeMath.AddChecked(100, 200));
        Console.WriteLine(SafeMath.AddWrapped(int.MinValue, -1));
    }
}
