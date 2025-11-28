using System;
using System.Reflection;

namespace ReflectionMathOperations
{
    // Class with basic math operations
    public class MathOperations
    {
        public int Add(int a, int b) => a + b;
        public int Sub(int a, int b) => a - b;
        public int Mul(int a, int b) => a * b;
        public int Div(int a, int b)
        {
            if (b == 0) throw new DivideByZeroException();
            return a / b;
        }
    }

    class Program
    {
        static void Main()
        {
            MathOperations math = new MathOperations();
            Type type = typeof(MathOperations);

            Console.WriteLine("Enter the name of the method to invoke (Add, Sub, Mul, Div):");
            string methodName = Console.ReadLine();

            // Get the MethodInfo dynamically
            MethodInfo method = type.GetMethod(methodName);

            if (method != null)
            {
                // Example input parameters
                object[] parameters = new object[] { 5, 10 };

                // Invoke the method dynamically
                object result = method.Invoke(math, parameters);
                Console.WriteLine($"Result of {methodName}(5, 10): {result}");
            }
            else
            {
                Console.WriteLine($"Method '{methodName}' not found!");
                Console.WriteLine("Available methods:");
                foreach (MethodInfo m in type.GetMethods())
                {
                    if (m.DeclaringType == typeof(MathOperations)) // only show custom methods
                        Console.WriteLine($"- {m.Name}");
                }
            }
        }
    }
}

/*
Demonstrates:
- How to dynamically invoke a method by name entered by the user.
- Uses MethodInfo.Invoke() to execute the method at runtime.
- Includes runtime safety: if the method does not exist, prints all available methods.
- Example of metaprogramming: program inspects and executes methods dynamically based on user input.
*/
