using System;
using Utilities;

namespace Utilities
{
    public class Counter
    {
        private int _value;

        public int Value => _value;

        public Counter() : this(0) { }

        public Counter(int start)
        {
            if (start < 0)
                throw new ArgumentOutOfRangeException(nameof(start), "Counter start value must be non-negative.");
            this._value = start;
        }

        public void Increment() => this._value++;

        public void Decrement()
        {
            if (this._value == 0)
                throw new InvalidOperationException("Cannot decrement: counter is already zero.");
            this._value--;
        }

        public bool TryDecrement()
        {
            if (this._value == 0) return false;
            this._value--;
            return true;
        }

        public void Reset() => this._value = 0;
    }

}

class Program
{
    static void Main()
    {
        var c = new Counter();
        Console.WriteLine(c.Value);

        c.Increment();
        Console.WriteLine(c.Value);

        c.Decrement();
        Console.WriteLine(c.Value);

        Console.WriteLine(c.TryDecrement());
        Console.WriteLine(c.Value);

        try
        {
            c.Decrement();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Exception caught: " + ex.Message);
        }

        var c2 = new Counter(5);
        Console.WriteLine(c2.Value);
        c2.Reset();
        Console.WriteLine(c2.Value);
    }
}