# StringTransformer - Minimal Version

## Description
`StringTransformer` demonstrates inversion of control using `Func<string,string>` strategies to transform strings.

- `TrimToUpper` → trims whitespace and converts to uppercase.
- `MaskDigits` → replaces all digits with `*`.
- Lambdas or method groups can be passed as strategies without using `if/else`.

## How to Run
1. Open the project in Visual Studio or any C# IDE.
2. Run the program.

Example:
```csharp
string input = "  Hello123 World  ";

string upper = StringTransformer.Transform(input, StringTransformer.TrimToUpper);
string masked = StringTransformer.Transform(input, StringTransformer.MaskDigits);
string reversed = StringTransformer.Transform(input, s => {
    char[] chars = s.ToCharArray();
    Array.Reverse(chars);
    return new string(chars);
});
```

## Example Output
```
TrimToUpper: HELLO123 WORLD
MaskDigits:   Hello*** World  
Reversed (lambda):   dlroW 321olleH  
```

## Unit Tests / Test Cases
1. `TrimToUpper("  test123 ")` → returns `TEST123`.
2. `MaskDigits("a1b2c3")` → returns `a*b*c*`.
3. Lambda reversing string → reverses correctly.
4. Passing null string → throws `ArgumentNullException`.
5. Passing null strategy → throws `ArgumentNullException`.