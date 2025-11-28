using System;
using System.Reflection;

namespace ReflectionMathOperations
{
    // Class with basic math operations
    public class MathOperations
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Sub(int a, int b)
        {
            return a - b;
        }

        public int Mul(int a, int b)
        {
            return a * b;
        }

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

            // Get the type of MathOperations
            Type type = typeof(MathOperations);

            // Get the MethodInfo for the Add method
            MethodInfo addMethod = type.GetMethod("Add");

            if (addMethod != null)
            {
                // Invoke the Add method dynamically
                object result = addMethod.Invoke(math, new object[] { 10, 5 });

                Console.WriteLine($"Result of Add(10, 5) using reflection: {result}");
            }
            else
            {
                Console.WriteLine("Method 'Add' not found!");
            }
        }
    }
}

/*
Demonstrates:
- How to dynamically invoke a method using MethodInfo.Invoke().
- The program calls Add(int a, int b) at runtime without directly using math.Add().
- This is metaprogramming because the code inspects and invokes methods dynamically during execution.
- Shows runtime safety: checks if MethodInfo is null before invoking.
*/
