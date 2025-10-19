# TempFileWriter - Minimal Version

## Description
`TempFileWriter` creates a temporary file and allows writing lines to it. It implements `IDisposable` to ensure the file is properly closed and resources are released.

- Writing after disposal throws `ObjectDisposedException`.
- Usage via `using` ensures automatic disposal.

## How to Run
1. Open the project in Visual Studio or any C# IDE.
2. Run the program.
3. The program demonstrates writing to a temporary file and automatic disposal.

## Example Usage
```csharp
using (var writer = new TempFileWriter())
{
    writer.WriteLine("Hello, world!");
    writer.WriteLine("This is a temporary file.");
    Console.WriteLine($"File created at: {writer.FilePath}");
}
```

## Behavior After Disposal
```csharp
var writer = new TempFileWriter();
writer.Dispose();
writer.WriteLine("This will fail"); // throws ObjectDisposedException
```

## Unit Tests / Test Cases
1. Write lines before disposal → succeeds.
2. Write line after disposal → throws `ObjectDisposedException`.
3. Using `using` statement → automatically closes the file.
4. File path is accessible via `FilePath` property.

