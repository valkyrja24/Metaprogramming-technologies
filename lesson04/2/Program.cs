using System;
using System.Collections.Generic;

class Book : IComparable<Book>
{
    public string Title { get; }
    public string Author { get; }
    public int Year { get; }

    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }

    // Реалізація "правильного порядку" через IComparable<Book>.
    public int CompareTo(Book other)
    {
        if (other == null) return 1;

        int authorCmp = string.Compare(this.Author, other.Author, StringComparison.Ordinal);
        if (authorCmp != 0) return authorCmp;

        int titleCmp = string.Compare(this.Title, other.Title, StringComparison.Ordinal);
        if (titleCmp != 0) return titleCmp;

        return this.Year.CompareTo(other.Year);
    }

    // Логічна рівність визначається лише за Title+Author (без Year).
    public override bool Equals(object obj)
    {
        Book other = obj as Book;
        if (other == null) return false;

        return string.Equals(this.Title, other.Title, StringComparison.Ordinal) &&
               string.Equals(this.Author, other.Author, StringComparison.Ordinal);
    }

    // Хеш повинен бути узгоджений з Equals: Title+Author.
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            if (Title != null) hash = hash * 23 + Title.GetHashCode();
            if (Author != null) hash = hash * 23 + Author.GetHashCode();
            return hash;
        }
    }

    public override string ToString()
    {
        return $"{Author}, \"{Title}\" ({Year})";
    }
}

class Program
{
    static void Main()
    {
        // Створення списку книжок зі шведськими авторами
        List<Book> books = new List<Book>
        {
            new Book("The Hobbit", "Selma Lagerlöf", 1937),
            new Book("The Silmarillion", "Klas Östergren", 1977),
            new Book("1984", "Hjalmar Söderberg", 1949),
            new Book("Animal Farm", "Astrid Lindgren", 1945),
            new Book("Brave New World", "Torgny Lindgren", 1932)
        };

        Console.WriteLine("Original list:");
        foreach (var b in books) Console.WriteLine(b);

        // Сортування через Sort() – використовує IComparable<Book>
        books.Sort();

        Console.WriteLine("\nSorted list (natural order):");
        foreach (var b in books) Console.WriteLine(b);

        // BinarySearch для наявної книги
        Book search1 = new Book("1984", "Hjalmar Söderberg", 1949);
        int idx1 = books.BinarySearch(search1);
        Console.WriteLine("\nBinarySearch for existing book:");
        Console.WriteLine(idx1 >= 0 ? "Found at index " + idx1 : "Not found");

        // BinarySearch для відсутньої книги
        Book search2 = new Book("Some Book", "Unknown", 2000);
        int idx2 = books.BinarySearch(search2);
        Console.WriteLine("\nBinarySearch for missing book:");
        Console.WriteLine(idx2 >= 0 ? "Found at index " + idx2 : "Not found");

        // --- Короткий звіт ---
        Console.WriteLine("\nReport:");
        Console.WriteLine(
            "Book implements IComparable<Book> to define natural ordering by Author, then Title, then Year. " +
            "Equals/GetHashCode are based only on Title+Author, ignoring Year, ensuring logical equality consistency. " +
            "Sort() arranges elements in stable natural order, and BinarySearch correctly returns index for found books " +
            "or a negative value when the item is missing."
        );
    }
}
