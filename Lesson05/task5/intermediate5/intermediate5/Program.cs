using System;

public class CounterChangedEventArgs : EventArgs
{
    public int Value { get; }

    public CounterChangedEventArgs(int value)
    {
        Value = value;
    }
}

public class Counter
{
    private int _count;
    private readonly int _threshold;

    public event EventHandler<CounterChangedEventArgs> Changed;
    public event EventHandler<CounterChangedEventArgs> ThresholdReached;

    public Counter(int threshold)
    {
        _threshold = threshold;
    }

    public void Increment()
    {
        _count++;
        RaiseEventSafely(Changed, new CounterChangedEventArgs(_count));

        if (_count >= _threshold)
            RaiseEventSafely(ThresholdReached, new CounterChangedEventArgs(_count));
    }

    private void RaiseEventSafely(EventHandler<CounterChangedEventArgs> handlers, CounterChangedEventArgs e)
    {
        var snapshot = handlers;
        if (snapshot == null) return;

        foreach (EventHandler<CounterChangedEventArgs> handler in snapshot.GetInvocationList())
        {
            try
            {
                handler(this, e);
            }
            catch
            {
            }
        }
    }
}

class Program
{
    static void Main()
    {
        var counter = new Counter(3);

        EventHandler<CounterChangedEventArgs> showValue = (s, e) => Console.WriteLine("Value: " + e.Value);
        EventHandler<CounterChangedEventArgs> logValue = (s, e) => Console.WriteLine("Log: counter incremented to " + e.Value);
        EventHandler<CounterChangedEventArgs> thrower = (s, e) => throw new Exception("Intentional");

        counter.Changed += showValue;
        counter.Changed += logValue;
        counter.Changed += thrower;

        counter.ThresholdReached += (s, e) => Console.WriteLine("Threshold reached at " + e.Value);

        counter.Increment();
        counter.Increment();
        counter.Increment();
    }
}
