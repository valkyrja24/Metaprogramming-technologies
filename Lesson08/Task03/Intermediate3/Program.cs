using System;
using System.Reflection;

// Define custom attribute
[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : Attribute
{
    // Marker attribute for required properties
}

// User class
public class User
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Email { get; set; }

    public string Password { get; set; } // Optional
}

class Program
{
    // Method to validate required properties
    static void Validate(object obj)
    {
        if (obj == null)
        {
            Console.WriteLine("Object is null!");
            return;
        }

        Type type = obj.GetType();
        foreach (PropertyInfo property in type.GetProperties())
        {
            // Check if property has [Required] attribute
            if (Attribute.IsDefined(property, typeof(RequiredAttribute)))
            {
                object value = property.GetValue(obj);
                if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                {
                    Console.WriteLine($"Error: Property '{property.Name}' is required but not filled!");
                }
            }
        }
    }

    static void Main()
    {
        User user = new User
        {
            Name = "Alice",
            Email = "", // empty to test validation
            Password = "secret123"
        };

        Console.WriteLine("Validating user...");
        Validate(user);
    }
}

/*
Demonstrates:
- How to use reflection to inspect custom attributes at runtime.
- Checks all properties marked with [Required] and validates if they are filled.
- Prints an error message if a required property is empty or null.
- Example of metaprogramming: program dynamically reads metadata (attributes) and acts accordingly.
*/
