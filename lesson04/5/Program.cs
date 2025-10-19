using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

class BoundedBag<T> : ICollection<T>
{
    private readonly List<T> _items;
    private readonly int _capacity;

    public BoundedBag(int capacity)
    {
        if (capacity <= 0) throw new ArgumentException("Capacity must be positive", nameof(capacity));
        _capacity = capacity;
        _items = new List<T>();
    }

    // --- Етап 1: IEnumerable<T> через yield return ---
    public IEnumerator<T> GetEnumerator()
    {
        foreach (var item in _items)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // --- Етап 2: ICollection<T> ---
    public int Count => _items.Count;

    public bool IsReadOnly => false;

    public void Add(T item)
    {
        if (_items.Count >= _capacity)
            throw new InvalidOperationException("BoundedBag is full");
        _items.Add(item);
    }

    public bool Remove(T item)
    {
        return _items.Remove(item);
    }

    public void Clear()
    {
        _items.Clear();
    }

    public bool Contains(T item)
    {
        return _items.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _items.CopyTo(array, arrayIndex);
    }

    // --- Публічний read-only API ---
    public static IReadOnlyCollection<T> ExposeView(BoundedBag<T> bag)
    {
        // ReadOnlyCollection<T> забезпечує лише читання
        return new ReadOnlyCollection<T>(bag._items);
    }
}

class Program
{
    static void Main()
    {
        // Створюємо BoundedBag з місткістю 3
        var bag = new BoundedBag<int>(3);
        bag.Add(10);
        bag.Add(20);
        bag.Add(30);

        // 1. foreach по BoundedBag<T>
        Console.WriteLine("1) foreach over BoundedBag:");
        foreach (var item in bag)
        {
            Console.WriteLine(item);
        }

        // 2. Спроба додати елемент понад місткість
        Console.WriteLine("\n2) Add element beyond capacity:");
        try
        {
            bag.Add(40);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }

        // 3. Повернення read-only view та неможливість змін
        Console.WriteLine("\n3) Read-only view:");
        IReadOnlyCollection<int> readOnly = BoundedBag<int>.ExposeView(bag);
        foreach (var item in readOnly)
        {
            Console.WriteLine(item);
        }

        // Спроба змін через read-only view неможлива:
        // readOnly.Add(50); бо компілятор не дозволить

        // 4. Безпечна ітерація при модифікаціях
        Console.WriteLine("\n4) Safe iteration using snapshot:");
        bag.Remove(20); // модифікуємо оригінал під час ітерації
        foreach (var item in new List<int>(bag)) // робимо знімок
        {
            Console.WriteLine(item);
        }

        // Альтернативно: for по індексах (копія List)
        Console.WriteLine("\nAlternative safe iteration using for loop:");
        var snapshot = new List<int>(bag);
        for (int i = 0; i < snapshot.Count; i++)
        {
            Console.WriteLine(snapshot[i]);
        }

        // --- Короткий звіт ---
        Console.WriteLine("\nReport:");
        Console.WriteLine(
            "BoundedBag<T> implements IEnumerable<T> with yield return, " +
            "and ICollection<T> with Add/Remove/Count and capacity enforcement. " +
            "ExposeView returns a read-only collection to prevent external modifications. " +
            "Safe iteration can be done via snapshot or index-based for loop when original bag is modified."
        );
    }
}
