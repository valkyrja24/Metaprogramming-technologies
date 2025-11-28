using System;
using System.Reflection;

namespace ReflectionProductExample
{
    // Product class
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }

    class Program
    {
        static void Main()
        {
            // Create instance dynamically
            object productObj = Activator.CreateInstance(typeof(Product));

            Type type = productObj.GetType();

            // Set properties dynamically using PropertyInfo
            PropertyInfo nameProp = type.GetProperty("Name");
            PropertyInfo priceProp = type.GetProperty("Price");

            if (nameProp != null) nameProp.SetValue(productObj, "Laptop");
            if (priceProp != null) priceProp.SetValue(productObj, 15000.0);

            // Retrieve values dynamically
            string nameValue = nameProp.GetValue(productObj) as string;
            double priceValue = (double)priceProp.GetValue(productObj);

            Console.WriteLine($"Product: Name={nameValue}, Price={priceValue}");
        }
    }
}

/*
Demonstrates:
- How to set and get property values dynamically using PropertyInfo.
- Uses reflection to access object properties without knowing them at compile time.
- Shows runtime manipulation of object data, an example of metaprogramming in C#.
- Useful when working with types or objects discovered at runtime.
*/
