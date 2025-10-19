# TextFormatter - Medium Version

## Description
`TextFormatter` allows transforming a collection of strings using a current strategy defined by a `Func<string,string>`. The strategy can be changed on the fly.

## Features
- `Strategy` property holds the current transformation function.
- `FormatAll(IEnumerable<string>)` applies the current strategy to all strings.
- Built-in strategies: `TrimToUpper`, `MaskDigits`.
- Custom strategies can be provided via lambda expressions or method groups.

## How to Run
1. Open the project in Visual Studio or any C# IDE.
2. Run the program.
3. Observe the output as the strategy is changed.

Example:
```csharp
var formatter = new TextFormatter();
var lines = new List<string> { "  hello123 ", " world456 " };

formatter.Strategy = TextFormatter.TrimToUpper;
foreach (var line in formatter.FormatAll(lines))
    Console.WriteLine(line);

formatter.Strategy = TextFormatter.MaskDigits;
foreach (var line in formatter.FormatAll(lines))
    Console.WriteLine(line);

formatter.Strategy = s => {
    char[] chars = s.ToCharArray();
    Array.Reverse(chars);
    return new string(chars);
};
foreach (var line in formatter.FormatAll(lines))
    Console.WriteLine(line);
```

## Unit Tests / Test Cases
1. Default strategy returns strings unchanged.
2. `TrimToUpper` strategy → trims spaces and converts to uppercase.
3. `MaskDigits` strategy → replaces digits with `*`.
4. Lambda strategy → reverses strings.
5. Changing strategy multiple times applies the new strategy correctly.
6. Passing null to `FormatAll` → throws `ArgumentNullException`.

