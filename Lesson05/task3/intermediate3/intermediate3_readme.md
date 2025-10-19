# AsyncFileCopier - Medium Version

## Description
`AsyncFileCopier` provides asynchronous file copying using `FileStream` with `useAsync:true`. It implements `IDisposable` for proper resource cleanup and supports cancellation via `CancellationToken`.

## How to Run
1. Open the project in Visual Studio or any C# IDE.
2. Ensure a source file exists (e.g., `source.txt`).
3. Run the program. The file will be copied to `dest.txt`.

Example:
```csharp
var copier = new AsyncFileCopier();
await copier.CopyAsync("source.txt", "dest.txt", CancellationToken.None);
copier.Dispose();
```

## Features
- Asynchronous copying with `await`.
- Cancellation support via `CancellationToken`.
- Proper disposal using `IDisposable`.
- Throws `ObjectDisposedException` if used after disposal.
- Correct propagation of `OperationCanceledException`.

## Example Output
```
Copy completed successfully.
Copied content: Hello, async file copying!
```

## Unit Tests / Test Cases
1. Copy a small file → contents match source.
2. Copy a non-existent file → throws `FileNotFoundException`.
3. Cancel the copy via `CancellationToken` → throws `OperationCanceledException`.
4. Use after disposal → throws `ObjectDisposedException`.