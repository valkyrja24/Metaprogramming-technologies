# AsyncFileCopier - Advanced Version

## Description
`AsyncFileCopier` provides asynchronous file copying with progress reporting. It uses `FileStream` with `useAsync:true` and supports cancellation via `CancellationToken`. Implements `IDisposable` with proper cleanup and event unsubscription.

## Features
- Asynchronous file copy.
- Progress reporting via `ProgressChanged` event (percentage).
- Proper disposal with `Dispose()` including event unsubscription.
- Cancellation support via `CancellationToken`.
- Iдемпотентний `Dispose()`.

## How to Run
1. Open the project in Visual Studio or any C# IDE.
2. Ensure a source file exists (e.g., `source.txt`).
3. Run the program.

Example:
```csharp
var copier = new AsyncFileCopier();
copier.ProgressChanged += (s, percent) => Console.WriteLine($"Progress: {percent}%");
await copier.CopyAsync("source.txt", "dest.txt", CancellationToken.None);
copier.ProgressChanged -= handler;
copier.Dispose();
```

## Example Output
```Progress: 0%Progress: 1%Progress: 2% ...Progress: 100%
Copy completed successfully.
Copied content length: 50000
```

## Unit Tests / Test Cases
1. Copy a small file → content matches source.
2. Copy a large file → `ProgressChanged` invoked multiple times with increasing percentages.
3. Cancel copy via `CancellationToken` → throws `OperationCanceledException`.
4. Dispose after copy → event unsubscription successful.
5. Multiple Dispose calls → idempotent, no exception.
6. Attempt copy after disposal → throws `ObjectDisposedException`.

