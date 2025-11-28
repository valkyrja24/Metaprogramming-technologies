using System;

// Define custom attribute
[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : Attribute
{
    // This attribute marks a property as required
}

// User class with properties
public class User
{
    [Required] // Name is required
    public string Name { get; set; }

    [Required] // Email is required
    public string Email { get; set; }

    public string Password { get; set; } // Optional
}

class Program
{
    static void Main()
    {
        // Example usage
        User user = new User
        {
            Name = "Alice",
            Email = "alice@example.com",
            Password = "secret123"
        };

        Console.WriteLine("User created:");
        Console.WriteLine($"Name: {user.Name}, Email: {user.Email}, Password: {user.Password}");
    }
}

/*
Demonstrates:
- How to create a custom attribute [Required].
- How to annotate properties with the attribute to mark them as required.
- Attributes store metadata about properties which can later be checked using reflection.
- This is a simple example of metaprogramming because the program can inspect attributes at runtime.
*/
