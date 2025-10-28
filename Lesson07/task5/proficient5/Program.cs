using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Product
{
    public int Id { get; }
    public string Name { get; }
    public string Category { get; }
    public decimal Price { get; }
    public bool IsActive { get; }

    public Product(int id, string name, string category, decimal price, bool isActive)
    {
        Id = id;
        Name = name;
        Category = category;
        Price = price;
        IsActive = isActive;
    }
}

class Program
{
    static void Main()
    {
        var rnd = new Random();
        var categories = new[] { "Books", "Electronics", "Toys" };
        var products = Enumerable.Range(1, 100_000)
            .Select(i => new Product(
                i,
                "Product" + rnd.Next(1, 100_000),
                categories[rnd.Next(categories.Length)],
                (decimal)(rnd.NextDouble() * 1000),
                rnd.Next(0, 2) == 1
            ))
            .ToList();

        var sw = Stopwatch.StartNew();

        var naive = products
            .Where(p => p.IsActive)
            .ToList()
            .Where(p => p.Category == "Books")
            .ToList()
            .OrderBy(p => p.Name.ToLower())
            .Select(p => new { p.Id, p.Name, p.Price })
            .ToList();

        sw.Stop();
        Console.WriteLine("Naive: {0} ms, count = {1}", sw.ElapsedMilliseconds, naive.Count);

        sw.Restart();

        var optimized = products
            .Where(p => p.IsActive && p.Category == "Books")
            .OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase)
            .Select(p => new { p.Id, p.Name, p.Price })
            .ToList();

        sw.Stop();
        Console.WriteLine("Optimized: {0} ms, count = {1}", sw.ElapsedMilliseconds, optimized.Count);
    }
}
