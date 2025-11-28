using System;
using System.Reflection;

// Example class 1
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
}

// Example class 2
public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

class Program
{
    // Method to create object dynamically and fill string properties
    static object CreateAndFill(Type t)
    {
        // Create instance dynamically
        object obj = Activator.CreateInstance(t);

        // Iterate all properties
        foreach (PropertyInfo prop in t.GetProperties())
        {
            // Only fill string properties
            if (prop.PropertyType == typeof(string) && prop.CanWrite)
            {
                prop.SetValue(obj, "DemoValue"); // set test value
            }
        }

        return obj;
    }

    static void Main()
    {
        // Create and fill Product
        object productObj = CreateAndFill(typeof(Product));
        Product product = productObj as Product;
        Console.WriteLine("Product object:");
        Console.WriteLine($"Id={product.Id}, Name={product.Name}, Price={product.Price}");

        // Create and fill User
        object userObj = CreateAndFill(typeof(User));
        User user = userObj as User;
        Console.WriteLine("\nUser object:");
        Console.WriteLine($"Name={user.Name}, Email={user.Email}, Password={user.Password}");
    }
}

/*
Demonstrates:
- How to dynamically create objects of any type using Activator.CreateInstance.
- Uses reflection to iterate all properties and fill string properties at runtime.
- Works for multiple classes without hardcoding property names.
- Example of metaprogramming: program inspects type metadata and manipulates object data dynamically.
- Useful for test data generation or dynamic initialization scenarios.
*/
