using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Calculator");

        while (true)
        {
            Console.Write("\nEnter first number or 'q' to quit: ");
            string input = Console.ReadLine();
            if (input.ToLower() == "q") break;
            if (!double.TryParse(input, out double num1))
            {
                Console.WriteLine("Invalid input!");
                continue;
            }

            Console.Write("Enter second number: ");
            if (!double.TryParse(Console.ReadLine(), out double num2))
            {
                Console.WriteLine("Invalid input!");
                continue;
            }

            Console.Write("Choose operation (+, -, *, /): ");
            string op = Console.ReadLine();

            double result = 0;
            bool valid = true;

            switch (op)
            {
                case "+": result = num1 + num2; break;
                case "-": result = num1 - num2; break;
                case "*": result = num1 * num2; break;
                case "/":
                    if (num2 != 0) result = num1 / num2;
                    else { Console.WriteLine("Error: Division by zero!"); valid = false; }
                    break;
                default:
                    Console.WriteLine("Invalid operation!");
                    valid = false;
                    break;
            }

            if (valid)
                Console.WriteLine($"Result: {result}");
        }

        Console.WriteLine("Calculator closed. Goodbye!");
    }
}
