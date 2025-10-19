# SumAll - Advanced Version

## Description
`SumAll` calculates the sum of a large integer array with optional overflow checking and simple performance measurements.

- `safe=true` → uses `checked` for overflow detection.
- `safe=false` → uses `unchecked` for fast wrap-around addition.

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

## Debug vs Release
- **Debug mode**: overflow checks can be enabled via `<CheckForOverflowUnderflow>` in the project file, adding extra safety but slightly reducing performance.
- **Release mode**: overflow checks are disabled unless explicitly using `checked`, which improves performance.

## Example Output
```
Sum result: 10000000
Elapsed time (safe mode): 450 ms
Debug mode: overflow checks may be enabled by <CheckForOverflowUnderflow>.
```

## Unit Tests / Test Cases
1. `SumAll(new int[]{100, 200, 300}, true)` → returns 600.
2. `SumAll(new int[]{int.MaxValue, 1}, true)` → throws `OverflowException`.
3. `SumAll(new int[]{int.MaxValue, 1}, false)` → wraps around to `int.MinValue`.
4. `SumAll(new int[]{-1, -2, -3}, true)` → returns -6.
5. Performance test: `SumAll` on 10,000,000 elements to measure elapsed time in safe/fast modes.