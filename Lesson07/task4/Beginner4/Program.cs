using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
    static void Main(string[] args)
    {
        var products = new List<Product>
        {
            new Product(1, "C# in Depth", "Books", 45.99m, true),
            new Product(2, "Laptop", "Electronics", 999.99m, true),
            new Product(3, "ASP.NET Core", "Books", 39.99m, false),
            new Product(4, "Notebook", "Books", 12.50m, true),
            new Product(5, "Headphones", "Electronics", 199.99m, true),
            new Product(6, "Clean Code", "Books", 35.00m, true)
        };

        decimal min = 30m;
        decimal max = 50m;

        Expression<Func<Product, bool>> filterExpr = p =>
            p.IsActive && p.Category == "Books" && p.Price >= min && p.Price <= max;

        Func<Product, bool> filterFunc = filterExpr.Compile();

        var result = products
            .Where(filterFunc)
            .OrderBy(p => p.Price)
            .ThenBy(p => p.Name)
            .Select(p => new { p.Id, p.Name, p.Price });

        foreach (var p in result)
        {
            Console.WriteLine("{0}: {1} - {2:C}", p.Id, p.Name, p.Price);
        }
    }
}
