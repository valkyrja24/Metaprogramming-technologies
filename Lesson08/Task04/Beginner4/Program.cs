using System;

namespace ReflectionProductExample
{
    // Product class with basic properties
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
            // Create instance dynamically using Activator
            object productObj = Activator.CreateInstance(typeof(Product));

            // Cast to Product to set properties
            Product product = productObj as Product;

            if (product != null)
            {
                product.Id = 1;
                product.Name = "Laptop";
                product.Price = 1200.50;

                Console.WriteLine($"Product: Id={product.Id}, Name={product.Name}, Price={product.Price}");
            }
        }
    }
}

/*
Demonstrates:
- How to dynamically create an instance of a class using Activator.CreateInstance.
- Shows how to cast the created object to access properties.
- Useful in metaprogramming scenarios where types are not known at compile time.
- Program inspects and manipulates objects at runtime.
*/
