# Counter with Threshold - Medium Version

## Description
`Counter` raises events on increment and when a threshold is reached. Handlers are invoked safely; exceptions in one handler do not prevent others from executing.

## Features
- `Counter` class with `Increment()` method.
- `Changed` event raised on every increment.
- `ThresholdReached` event raised when counter reaches or exceeds threshold.
- Safe invocation of event handlers; exceptions in handlers are isolated.
- Supports multiple subscribers, including ones that throw exceptions.

## How to Run
1. Open the project in Visual Studio or any C# IDE.
2. Run the program.
3. Observe console output from multiple subscribers.

Example:
```csharp
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
```

## Example Output
```
Value: 1
Log: counter incremented to 1
Value: 2
Log: counter incremented to 2
Value: 3
Log: counter incremented to 3
Threshold reached at 3
```

## Unit Tests / Test Cases
1. Increment triggers `Changed` event.
2. `ThresholdReached` triggers at or above threshold.
3. Exception in one handler does not affect others.
4. Multiple subscribers receive events.
5. Increment below threshold → `ThresholdReached` not called.