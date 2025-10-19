# TextFormatter - Advanced Version

## Description
`TextFormatter` supports a pipeline of string transformation strategies applied sequentially. If any step throws an exception, processing stops for that string and an informational result is returned.

## Features
- `Strategy` property for a single transformation.
- `SetPipeline(params Func<string,string>[] steps)` to define multiple steps.
- `FormatAll(IEnumerable<string>)` applies pipeline or single strategy.
- Errors in any step return a message like `Error: <exception message>`.
- Flexible strategy modification without `if/else`.

## How to Run
1. Open the project in Visual Studio or any C# IDE.
2. Run the program.
3. Observe the output with multiple transformation steps.

Example:
```csharp
var formatter = new TextFormatter();
var lines = new List<string> { "  hello123 ", " world456 ", null };

formatter.SetPipeline(
    TextFormatter.TrimToUpper,
    TextFormatter.MaskDigits,
    s => s + "!"
);

foreach (var line in formatter.FormatAll(lines))
    Console.WriteLine(line);
```

## Example Output
```
HELLO***!
WORLD***!
Error: Value cannot be null.
```

## Unit Tests / Test Cases
1. Single strategy (`TrimToUpper`) applies correctly.
2. Pipeline applies multiple transformations sequentially.
3. Pipeline with a step throwing exception returns `Error: <message>`.
4. Pipeline with null input throws `ArgumentNullException`.
5. Changing pipeline dynamically applies new transformations.

