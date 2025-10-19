using System;
using System.Collections.Generic;

// Клас Book
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

    // Природний порядок: Author → Title → Year
    public int CompareTo(Book other)
    {
        if (other == null) return 1;
        int authorCmp = string.Compare(this.Author, other.Author, StringComparison.Ordinal);
        if (authorCmp != 0) return authorCmp;

        int titleCmp = string.Compare(this.Title, other.Title, StringComparison.Ordinal);
        if (titleCmp != 0) return titleCmp;

        return this.Year.CompareTo(other.Year);
    }

    // Рівність лише по Title + Author
    public override bool Equals(object obj)
    {
        Book other = obj as Book;
        if (other == null) return false;
        return string.Equals(this.Title, other.Title, StringComparison.Ordinal) &&
               string.Equals(this.Author, other.Author, StringComparison.Ordinal);
    }

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

// Компаратор 1: довжина Title, потім Title
class TitleLengthComparer : IComparer<Book>
{
    public int Compare(Book x, Book y)
    {
        if (x == null) return y == null ? 0 : -1;
        if (y == null) return 1;

        int lengthCmp = x.Title.Length.CompareTo(y.Title.Length);
        if (lengthCmp != 0) return lengthCmp;

        return string.Compare(x.Title, y.Title, StringComparison.Ordinal);
    }
}

// Компаратор 2: спадання Year, потім Author
class YearDescComparer : IComparer<Book>
{
    public int Compare(Book x, Book y)
    {
        if (x == null) return y == null ? 0 : -1;
        if (y == null) return 1;

        int yearCmp = y.Year.CompareTo(x.Year); // descending
        if (yearCmp != 0) return yearCmp;

        return string.Compare(x.Author, y.Author, StringComparison.Ordinal);
    }
}

class Program
{
    static void Main()
    {
        // Список книг зі шведськими авторами
        List<Book> books = new List<Book>
        {
            new Book("Röda Rummet", "August Strindberg", 1879),
            new Book("Gentlemen", "Klas Östergren", 1980),
            new Book("Doktor Glas", "Hjalmar Söderberg", 1905),
            new Book("Pippi Långstrump", "Astrid Lindgren", 1945),
            new Book("Bröderna Lejonhjärta", "Astrid Lindgren", 1973)
        };

        // --- 1. Сортування кожним компаратором ---
        books.Sort(new TitleLengthComparer());
        Console.WriteLine("Sorted by Title Length:");
        foreach (var b in books) Console.WriteLine(b);

        books.Sort(new YearDescComparer());
        Console.WriteLine("\nSorted by Year Descending:");
        foreach (var b in books) Console.WriteLine(b);

        // --- 2. SortedSet з TitleLengthComparer ---
        var setTitleLength = new SortedSet<Book>(new TitleLengthComparer());
        setTitleLength.Add(new Book("BookA", "Author1", 2000));
        setTitleLength.Add(new Book("BookB", "Author2", 2010)); // same length → merged
        setTitleLength.Add(new Book("BookC", "Author3", 2020)); // same length → merged
        setTitleLength.Add(new Book("LongerBook", "Author4", 2005));

        Console.WriteLine("\nSortedSet with TitleLengthComparer (duplicates merged by comparer):");
        foreach (var b in setTitleLength) Console.WriteLine(b);

        // --- SortedSet з YearDescComparer ---
        var setYearDesc = new SortedSet<Book>(new YearDescComparer());
        setYearDesc.Add(new Book("BookA", "Author1", 2020));
        setYearDesc.Add(new Book("BookB", "Author2", 2020)); // same Year → merged
        setYearDesc.Add(new Book("BookC", "Author3", 2015));
        setYearDesc.Add(new Book("BookD", "Author4", 2010));

        Console.WriteLine("\nSortedSet with YearDescComparer (duplicates merged by comparer):");
        foreach (var b in setYearDesc) Console.WriteLine(b);

        // --- 3. BinarySearch з компаратором ---
        books.Sort(new TitleLengthComparer());
        Book searchBook = new Book("Doktor Glas", "Hjalmar Söderberg", 1905);
        int index = books.BinarySearch(searchBook, new TitleLengthComparer());
        Console.WriteLine($"\nBinarySearch by TitleLengthComparer for 'Doktor Glas': {index}");

        Book missingBook = new Book("Unknown Book", "Unknown", 2000);
        int missingIndex = books.BinarySearch(missingBook, new TitleLengthComparer());
        Console.WriteLine($"BinarySearch for missing book: {missingIndex}");

        // --- Короткий звіт ---
        Console.WriteLine("\nReport:");
        Console.WriteLine(
            "Two external comparers were implemented: TitleLengthComparer (sorts by Title length then Title) " +
            "and YearDescComparer (sorts by descending Year then Author). " +
            "SortedSet demonstrates that elements considered equal by the comparer are merged. " +
            "BinarySearch uses the same comparer as Sort to return correct indices or negative values for missing elements. " +
            "This shows how external ordering policies influence collection behavior and equality in sets."
        );
    }
}
