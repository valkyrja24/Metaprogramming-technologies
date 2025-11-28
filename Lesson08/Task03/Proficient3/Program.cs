using System;
using System.Reflection;

// Custom attribute with MinLength parameter
[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : Attribute
{
    public int MinLength { get; }

    public RequiredAttribute(int minLength = 0)
    {
        MinLength = minLength;
    }
}

// User class
public class User
{
    [Required(3)] // Name is required, min length 3
    public string Name { get; set; }

    [Required(5)] // Email is required, min length 5
    public string Email { get; set; }

    public string Password { get; set; } // Optional
}

class Program
{
    static void Validate(object obj)
    {
        if (obj == null) return;

        Type type = obj.GetType(); // runtime type inspection
        Console.WriteLine($"\nValidating object of type: {type.Name}");

        foreach (PropertyInfo prop in type.GetProperties())
        {
            var attr = prop.GetCustomAttribute<RequiredAttribute>(); // inspect attribute at runtime
            if (attr != null)
            {
                string value = prop.GetValue(obj) as string; // get value dynamically

                if (string.IsNullOrWhiteSpace(value))
                    Console.WriteLine($"Error: '{prop.Name}' is required!");
                else if (value.Length < attr.MinLength)
                    Console.WriteLine($"Error: '{prop.Name}' must be at least {attr.MinLength} characters!");
            }
        }
    }

    static void Main()
    {
        User user1 = new User
        {
            Name = "Al",      // too short
            Email = "",       // empty
            Password = "123"
        };

        User user2 = new User
        {
            Name = "Alice",
            Email = "alice@example.com",
            Password = "secret123"
        };

        Validate(user1);
        Validate(user2);
    }
}

/*
Demonstrates:
- How to extend a custom attribute with a parameter (MinLength).
- Runtime validation using reflection checks both required and minimum length rules.
- Prints specific error messages if fields are empty or too short.
- Example of metaprogramming: program inspects attributes and values dynamically at runtime and applies rules.
*/
