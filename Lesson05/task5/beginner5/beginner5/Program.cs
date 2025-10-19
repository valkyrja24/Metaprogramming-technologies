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

    public event EventHandler<CounterChangedEventArgs> Changed;

    public void Increment()
    {
        _count++;
        OnChanged(new CounterChangedEventArgs(_count));
    }

    protected virtual void OnChanged(CounterChangedEventArgs e)
    {
        var handlers = Changed;
        if (handlers == null) return;

        foreach (EventHandler<CounterChangedEventArgs> handler in handlers.GetInvocationList())
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
        var counter = new Counter();

        EventHandler<CounterChangedEventArgs> showValue = (s, e) => Console.WriteLine("Value: " + e.Value);
        EventHandler<CounterChangedEventArgs> logValue = (s, e) => Console.WriteLine("Log: counter incremented to " + e.Value);

        counter.Changed += showValue;
        counter.Changed += logValue;

        counter.Increment();
        counter.Increment();

        counter.Changed -= logValue;

        counter.Increment();
    }
}
