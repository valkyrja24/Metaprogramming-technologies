using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

class Customer
{
    public int Id { get; }
    public string Name { get; }

    public Customer(int id, string name)
    {
        Id = id;
        Name = name;
    }
}

class Order
{
    public int Id { get; }
    public int CustomerId { get; }
    public decimal Total { get; }

    public Order(int id, int customerId, decimal total)
    {
        Id = id;
        CustomerId = customerId;
        Total = total;
    }
}

class Program
{
    static void Main()
    {
        var customers = new List<Customer>();
        var orders = new List<Order>();

        for (int i = 1; i <= 10000; i++)
        {
            customers.Add(new Customer(i, "Customer" + i));
            orders.Add(new Order(i, i % 1000 + 1, i * 1.5m));
        }

        var sw = Stopwatch.StartNew();

        var naive = orders
            .Where(o => customers.Any(c => c.Id == o.CustomerId))
            .ToList();

        sw.Stop();
        Console.WriteLine("{0} ms, count = {1}", sw.ElapsedMilliseconds, naive.Count);

        sw.Restart();

        var customerDict = customers.ToDictionary(c => c.Id);
        var optimized = orders
            .Where(o => customerDict.ContainsKey(o.CustomerId))
            .ToList();

        sw.Stop();
        Console.WriteLine("{0} ms, count = {1}", sw.ElapsedMilliseconds, optimized.Count);

        sw.Restart();

        var orderLookup = orders.ToLookup(o => o.CustomerId);

        var customerOrders = customers
            .Select(c => new
            {
                Customer = c,
                Orders = orderLookup[c.Id]
            })
            .ToList();

        sw.Stop();
        Console.WriteLine("{0} ms, count = {1}", sw.ElapsedMilliseconds, customerOrders.Count);
    }
}
