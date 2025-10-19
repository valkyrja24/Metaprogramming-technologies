using System;
using System.Collections.Generic;

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

public class CounterListener : IDisposable
{
    private readonly Counter _counter;
    private bool _disposed;

    public CounterListener(Counter counter)
    {
        _counter = counter ?? throw new ArgumentNullException(nameof(counter));
        _counter.Changed += OnChanged;
        _counter.ThresholdReached += OnThresholdReached;
        _disposed = false;
    }

    private void OnChanged(object sender, CounterChangedEventArgs e)
    {
        Console.WriteLine($"Listener: Counter changed to {e.Value}");
    }

    private void OnThresholdReached(object sender, CounterChangedEventArgs e)
    {
        Console.WriteLine($"Listener: Threshold reached at {e.Value}");
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _counter.Changed -= OnChanged;
            _counter.ThresholdReached -= OnThresholdReached;
            _disposed = true;
        }
    }
}

class Program
{
    static void Main()
    {
        var counter = new Counter(3);

        var listeners = new List<CounterListener>();
        for (int i = 0; i < 3; i++)
        {
            var listener = new CounterListener(counter);
            listeners.Add(listener);
        }

        counter.Increment();
        counter.Increment();
        counter.Increment();

        foreach (var listener in listeners)
            listener.Dispose();

        counter.Increment();
    }
}
