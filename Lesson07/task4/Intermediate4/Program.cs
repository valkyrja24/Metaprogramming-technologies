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

static class ProductFilters
{
    public static Expression<Func<Product, bool>> ByCategory(string category)
    {
        return p => p.Category == category;
    }

    public static Expression<Func<Product, bool>> ByPriceRange(decimal min, decimal max)
    {
        return p => p.Price >= min && p.Price <= max;
    }

    public static Expression<Func<Product, bool>> OnlyActive()
    {
        return p => p.IsActive;
    }

    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var param = Expression.Parameter(typeof(T));
        var body = Expression.AndAlso(
            Expression.Invoke(expr1, param),
            Expression.Invoke(expr2, param)
        );
        return Expression.Lambda<Func<T, bool>>(body, param);
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

        var combinedFilter = ProductFilters.OnlyActive()
            .AndAlso(ProductFilters.ByCategory("Books"))
            .AndAlso(ProductFilters.ByPriceRange(min, max));

        var filterFunc = combinedFilter.Compile();

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
