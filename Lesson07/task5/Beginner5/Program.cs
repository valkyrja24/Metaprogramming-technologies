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
        var products = new List<Product>();
        for (int i = 1; i <= 100000; i++)
        {
            products.Add(new Product(i, "Product" + i, i % 2 == 0 ? "Books" : "Electronics", i * 1.1m, i % 3 != 0));
        }

        var sw = Stopwatch.StartNew();

        var naive = products
            .Where(p => p.IsActive)
            .ToList()
            .Where(p => p.Category == "Books")
            .ToList()
            .OrderBy(p => p.Price)
            .ThenBy(p => p.Name)
            .Select(p => new { p.Id, p.Name, p.Price })
            .ToList();

        sw.Stop();
        Console.WriteLine("Naive pipeline: {0} ms, count = {1}", sw.ElapsedMilliseconds, naive.Count);

        sw.Restart();

        var optimized = products
            .Where(p => p.IsActive && p.Category == "Books")
            .OrderBy(p => p.Price)
            .ThenBy(p => p.Name)
            .Select(p => new { p.Id, p.Name, p.Price })
            .ToList();

        sw.Stop();
        Console.WriteLine("Optimized pipeline: {0} ms, count = {1}", sw.ElapsedMilliseconds, optimized.Count);

        bool hasExpensive = products.Any(p => p.Price > 100000m);
        Console.WriteLine("Any expensive product: " + hasExpensive);

        var lookupByCategory = products.ToLookup(p => p.Category);
        Console.WriteLine("Categories: " + string.Join(", ", lookupByCategory.Select(k => k.Key)));
    }
}
