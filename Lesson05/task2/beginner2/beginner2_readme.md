# SafeMath - Minimal Version

## Description
`SafeMath` provides two methods for integer addition:
- `AddChecked(int a, int b)` performs addition with overflow checking (throws `OverflowException`).
- `AddWrapped(int a, int b)` performs addition without overflow checking (wrap-around behavior).

## How to Run
1. Open the project in Visual Studio or any C# IDE.
2. Run the program. It demonstrates both checked and wrapped addition.

## Examples
```
SafeMath.AddChecked(int.MaxValue, 1); // throws OverflowException
SafeMath.AddWrapped(int.MaxValue, 1); // wraps around to int.MinValue
SafeMath.AddChecked(100, 200); // returns 300
SafeMath.AddWrapped(int.MinValue, -1); // wraps around to int.MaxValue
```

## Unit Tests / Test Cases
1. Checked addition with overflow: `AddChecked(int.MaxValue, 1)` → throws `OverflowException`.
2. Wrapped addition with overflow: `AddWrapped(int.MaxValue, 1)` → wraps to `int.MinValue`.
3. Normal addition: `AddChecked(100, 200)` → returns 300.
4. Wrapped addition with negative overflow: `AddWrapped(int.MinValue, -1)` → wraps to `int.MaxValue`.