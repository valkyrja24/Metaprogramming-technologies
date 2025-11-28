using System;
using System.Reflection;

namespace ReflectionExampleA
{
    // Class Student with three properties
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Group { get; set; }
    }

    class Program
    {
        static void Main()
        {
            // Get type metadata of Student class at runtime
            Type studentType = typeof(Student);

            Console.WriteLine("Properties of Student:\n");

            // Loop through all properties
            foreach (PropertyInfo property in studentType.GetProperties())
            {
                // Print property name and data type
                Console.WriteLine($"Property Name: {property.Name}, Type: {property.PropertyType.Name}");
            }
        }
    }
}

/*
Demonstrates:
- How to inspect class properties at runtime using reflection.
- The program dynamically retrieves metadata about the class without needing to know property names in advance.
- This is an example of metaprogramming because it works with the program's structure (class properties) during execution.
*/
