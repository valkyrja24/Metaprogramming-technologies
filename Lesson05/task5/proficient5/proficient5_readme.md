# Counter with Listener - Advanced Version

## Description
`CounterListener` demonstrates safe subscription and unsubscription to events using `IDisposable`. Multiple listeners can subscribe to `Counter` events and be disposed properly to avoid memory leaks.

## Features
- `Counter` class with `Increment()` method.
- `Changed` and `ThresholdReached` events.
- `CounterListener` subscribes to both events.
- Implements `IDisposable` to unsubscribe safely.
- Multiple listeners can be created and disposed.

## How to Run
1. Open the project in Visual Studio or any C# IDE.
2. Run the program.
3. Observe console output as listeners respond to counter events.

Example:
```csharp
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
```

## Example Output
```
Listener: Counter changed to 1
Listener: Counter changed to 2
Listener: Counter changed to 3
Listener: Threshold reached at 3
```

## Notes
- If listeners are not disposed, event subscriptions keep objects alive, causing potential memory leaks.
- Using `Dispose()` ensures that listeners unsubscribe and can be garbage collected.

## Unit Tests / Test Cases
1. Increment triggers `Changed` event for all listeners.
2. ThresholdReached triggers at or above threshold.
3. Dispose unsubscribes listener; subsequent increments do not trigger events for disposed listeners.
4. Multiple listeners function independently.
5. Creating listeners without Dispose demonstrates potential memory retention (observed via profiler).