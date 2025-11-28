using System;
using System.Reflection;

namespace ReflectionExampleB
{
    // Class Student with properties and a method
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Group { get; set; }

        // Custom method to print student information
        public void PrintInfo()
        {
            Console.WriteLine($"Student: {Name}, Age: {Age}, Group: {Group}");
        }
    }

    class Program
    {
        static void Main()
        {
            // Get type metadata of Student class at runtime
            Type studentType = typeof(Student);

            Console.WriteLine("Methods of Student (including inherited):\n");

            // Loop through all methods, including inherited from Object
            foreach (MethodInfo method in studentType.GetMethods())
            {
                // Print method name and return type
                Console.WriteLine($"Method Name: {method.Name}, Return Type: {method.ReturnType.Name}");
            }
        }
    }
}

/*
Demonstrates:
- How to inspect all public methods of a class at runtime using reflection.
- GetMethods() returns both custom methods and inherited methods (e.g., ToString(), Equals()).
- PrintInfo() is a custom method added to the Student class.
- This is metaprogramming because the code inspects the structure and behavior (methods) of another class dynamically, during runtime.
*/
