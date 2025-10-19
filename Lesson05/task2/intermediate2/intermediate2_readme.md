# SumAll - Medium Version

## Description
`SumAll` calculates the sum of an integer array with optional overflow checking.

- `safe=true` → uses `checked` to detect overflow (throws `OverflowException`).
- `safe=false` → uses `unchecked` for wrap-around behavior.

## How to Run
1. Open the project in Visual Studio or any C# IDE.
2. Run the program with an optional argument:
   - `--safe` → safe mode with overflow checking
   - `--fast` → fast mode without overflow checking

Example:
```
Program.exe --safe
Program.exe --fast
```

## Example Output
```
Sum result: 2147483647
Overflow occurred during sum.
```

## Unit Tests / Test Cases
1. `SumAll(new int[]{100, 200, 300}, true)` → returns 600.
2. `SumAll(new int[]{int.MaxValue, 1}, true)` → throws `OverflowException`.
3. `SumAll(new int[]{int.MaxValue, 1}, false)` → wraps around to `int.MinValue`.
4. `SumAll(new int[]{-1, -2, -3}, true)` → returns -6.