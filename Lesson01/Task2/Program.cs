using System;

class Program
{
    const double USD_TO_UAH = 41.32;
    const double EUR_TO_UAH = 48.59;

    static void Main()
    {
        Console.WriteLine("Currency Converter");
        Console.Write("Enter amount: ");
        double amount = Convert.ToDouble(Console.ReadLine());

        Console.Write("From currency (USD, EUR, UAH): ");
        string from = Console.ReadLine().ToUpper();

        Console.Write("To currency (USD, EUR, UAH): ");
        string to = Console.ReadLine().ToUpper();

        double result = ConvertCurrency(amount, from, to);

        Console.WriteLine($"{amount} {from} = {result} {to}");
    }

    static double ConvertCurrency(double amount, string from, string to)
    {
        double amountInUAH;

        if (from == "USD") amountInUAH = amount * USD_TO_UAH;
        else if (from == "EUR") amountInUAH = amount * EUR_TO_UAH;
        else amountInUAH = amount;

        if (to == "USD") return amountInUAH / USD_TO_UAH;
        if (to == "EUR") return amountInUAH / EUR_TO_UAH;
        return amountInUAH;
    }
}
