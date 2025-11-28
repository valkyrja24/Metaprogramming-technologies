using System;
using System.Reflection;

namespace ReflectionExampleC
{
    // Example class
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Group { get; set; }

        public void PrintInfo()
        {
            Console.WriteLine($"Student: {Name}, Age: {Age}, Group: {Group}");
        }
    }

    class Program
    {
        // Function that prints structure of any object
        public static void PrintObjectStructure(object obj)
        {
            if (obj == null)
            {
                Console.WriteLine("Object is null!");
                return;
            }

            Type type = obj.GetType();

            Console.WriteLine($"Class Name: {type.Name}");
            Console.WriteLine($"Base Type: {type.BaseType?.Name ?? "None"}");
            Console.WriteLine($"Number of Fields: {type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Length}");
            Console.WriteLine($"Number of Properties: {type.GetProperties().Length}");
            Console.WriteLine($"Number of Methods: {type.GetMethods().Length}");
        }

        static void Main()
        {
            Student student = new Student { Name = "Alice", Age = 20, Group = "B1" };

            // Dynamically inspect the structure of any object
            PrintObjectStructure(student);

            // Example with a built-in type
            int number = 42;
            PrintObjectStructure(number);
        }
    }
}

/*
Demonstrates:
- How to dynamically inspect any object's structure at runtime using reflection.
- Prints class name, base type, number of fields, properties, and methods.
- Works for custom classes (Student) and built-in types (int).
- This is metaprogramming because the program analyzes an object's type and metadata dynamically at runtime without knowing its type beforehand.
- Includes runtime safety: checks for null objects.
*/
