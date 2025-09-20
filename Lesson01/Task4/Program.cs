using System;
using System.Collections.Generic;
using System.Linq;

class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }

    public string GetDescription()
    {
        return $"Title: {Title}, Author: {Author}, Year: {Year}";
    }
}

class Program
{
    static void Main()
    {
        List<Book> library = new List<Book>
        {
            new Book { Title = "Doktor Glas", Author = "Hjalmar Söderberg", Year = 1905 },
            new Book { Title = "Gentlemen", Author = "Klas Östergren", Year = 1980 },
            new Book { Title = "Ett öga rött", Author = "Jonas Hassen Khemiri", Year = 2003 }
        };

        Console.WriteLine("Book Library");
        Console.Write("Search by (title/author/year): ");
        string criteria = Console.ReadLine().ToLower();

        Console.Write("Enter value: ");
        string value = Console.ReadLine();

        var results = SearchBooks(library, criteria, value);

        Console.WriteLine("\n Search Results");
        foreach (var book in results)
            Console.WriteLine(book.GetDescription());

        if (!results.Any())
            Console.WriteLine("No books found.");
    }

    static IEnumerable<Book> SearchBooks(List<Book> books, string criteria, string value)
    {
        switch (criteria)
        {
            case "title":
                return books.Where(b => b.Title.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0);
            case "author":
                return books.Where(b => b.Author.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0);
            case "year":
                if (int.TryParse(value, out int year))
                    return books.Where(b => b.Year == year);
                break;
        }
        return new List<Book>();
    }
}
