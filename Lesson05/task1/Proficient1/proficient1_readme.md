# ConfigParser - Advanced Version

## Description
`ConfigParser` is an advanced parser for configuration files in the format `key=value`. It supports asynchronous file reading, cancellation, duplicate key detection, and robust exception handling.

## How to Run
1. Open the project in Visual Studio or any C# IDE.
2. Create a file `settings.txt` with configuration, for example:
```
username=admin
password=12345
# This is a comment
port=8080
```
3. Run the program. It will read the file asynchronously and print the keys and values:
```
username = admin
password = 12345
port = 8080
```

## Features
- Asynchronous parsing with `CancellationToken` support.
- Skips empty lines and lines starting with `#`.
- Throws `ConfigFormatException` for invalid lines with line number and preserves `InnerException`.
- Throws `InvalidOperationException` if duplicate keys are found.
- Compatible with C# 7.3 and higher (no `using var`, no `CallerArgumentExpression`).

## Unit Tests / Test Cases
1. Correct line: `"username=admin"` → returns `("username", "admin")`.
2. Empty line: `""` → throws `ArgumentNullException`.
3. Line without `=`: `"username"` → throws `FormatException`.
4. Line with spaces: `"  user  =  123  "` → returns `("user", "123")`.
5. File with comments and empty lines parses correctly.
6. Duplicate keys → throws `InvalidOperationException`.
7. Async parsing cancellation → throws `OperationCanceledException`.