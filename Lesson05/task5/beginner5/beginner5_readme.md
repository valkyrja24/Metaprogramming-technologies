# Counter with Events - Minimal Version

## Description
`Counter` demonstrates user-defined events using `EventHandler<T>`. The `Changed` event is raised every time the counter is incremented.

## Features
- `Counter` class with `Increment()` method.
- Event `Changed` using `EventHandler<CounterChangedEventArgs>`.
- Safe invocation of multiple subscribers with error isolation.
- Subscribers can unsubscribe to prevent memory leaks.

## How to Run
1. Open the project in Visual Studio or any C# IDE.
2. Run the program.
3. Observe console output from multiple subscribers.

Example:
```csharp
var counter = new Counter();

EventHandler<CounterChangedEventArgs> showValue = (s, e) => Console.WriteLine("Value: " + e.Value);
EventHandler<CounterChangedEventArgs> logValue = (s, e) => Console.WriteLine("Log: counter incremented to " + e.Value);

counter.Changed += showValue;
counter.Changed += logValue;

counter.Increment();
counter.Increment();

counter.Changed -= logValue;
counter.Increment();
```

## Example Output
```
Value: 1
Log: counter incremented to 1
Value: 2
Log: counter incremented to 2
Value: 3
```

## Unit Tests / Test Cases
1. Increment counter once → `Changed` event called.
2. Multiple subscribers receive event.
3. Subscriber throwing exception does not affect others.
4. Unsubscribe works → event no longer triggers for removed subscriber.
5. Multiple increments → `Changed` raised each time.