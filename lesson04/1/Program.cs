using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // List<int> реалізує IList<int>, ICollection<int> та IEnumerable<int>.
        // Інтерфейс IEnumerable<int> дозволяє використовувати foreach і ручну ітерацію через GetEnumerator().
        List<int> numbers = new List<int> { 10, 20, 30, 40, 50 };

        Console.WriteLine("1) Manual iteration using GetEnumerator():");

        // IEnumerator<int> використовується для покрокового проходу по колекції.
        var enumerator = numbers.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                int item = enumerator.Current;
                Console.WriteLine(item);
            }
        }
        finally
        {
            // Dispose() потрібно викликати вручну, якщо не використовується foreach.
            enumerator.Dispose();
        }

        Console.WriteLine("\n2) Iteration using foreach:");

        // foreach працює, тому що List<int> реалізує IEnumerable<int>.
        foreach (int item in numbers)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("\n3) Attempt to modify collection during foreach:");
        try
        {
            // Це спричинить InvalidOperationException, оскільки змінювати
            // колекцію під час ітерації заборонено.
            foreach (int item in numbers)
            {
                Console.WriteLine(item);
                numbers.Add(999);
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Exception: " + ex.GetType().Name + " - " + ex.Message);
        }

        Console.WriteLine("\n4) Safe iteration using a snapshot (ToArray()):");

        // ToArray() створює копію – зміни оригіналу не впливають на перебір.
        // Повернений масив реалізує IEnumerable, тому foreach доступний.
        int[] snapshot = numbers.ToArray();
        foreach (int item in snapshot)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("\n4b) Alternative safe iteration using for-loop:");

        // Цикл for не використовує енумератор і не кидає виняток при змінах.
        for (int i = 0; i < numbers.Count; i++)
        {
            Console.WriteLine(numbers[i]);
        }

        // --- Короткий звіт (2–5 речень) ---
        Console.WriteLine("\nReport:");
        Console.WriteLine(
            "List<int> was chosen because it provides indexed access and supports IEnumerable for iteration. " +
            "Manual traversal demonstrates the use of IEnumerator with MoveNext, Current, and Dispose. " +
            "InvalidOperationException occurs as expected when modifying the collection inside foreach. " +
            "Safe traversal is demonstrated using a snapshot (ToArray) and a for-loop to avoid modification conflicts."
        );
    }
}
