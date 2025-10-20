using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

class Product
{
    public int Id;
    public double Price;
    public string Name;
    public string Category;
}

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: write <file> [--big-endian] | read <file>");
            return;
        }

        string command = args[0];
        string filePath = args[1];
        bool bigEndianId = false;

        for (int i = 2; i < args.Length; i++)
        {
            if (args[i].Equals("--big-endian", StringComparison.OrdinalIgnoreCase))
                bigEndianId = true;
        }

        if (command.Equals("write", StringComparison.OrdinalIgnoreCase))
            WriteFile(filePath, bigEndianId);
        else if (command.Equals("read", StringComparison.OrdinalIgnoreCase))
            ReadFile(filePath);
        else
            Console.WriteLine("Unknown command.");
    }

    static void WriteFile(string filePath, bool bigEndianId)
    {
        var products = new List<Product>
        {
            new Product { Id = 1, Price = 10.5, Name = "Apple", Category="Fruit" },
            new Product { Id = 2, Price = 5.25, Name = "Banana", Category="Fruit" },
            new Product { Id = 3, Price = 7.8, Name = "Orange", Category="Fruit" }
        };

        int version = 2;

        using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        using (var writer = new BinaryWriter(fs, Encoding.UTF8))
        {
            writer.Write(Encoding.ASCII.GetBytes("MAGC"));
            writer.Write(version);

            foreach (var p in products)
            {
                if (bigEndianId)
                {
                    byte[] idBytes = BitConverter.GetBytes(p.Id);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(idBytes);
                    writer.Write(idBytes);
                }
                else
                {
                    writer.Write(p.Id);
                }

                writer.Write(p.Price);
                writer.Write(p.Name);
                if (version >= 2)
                    writer.Write(p.Category);
            }
        }

        Console.WriteLine("File written: " + filePath + (bigEndianId ? " (Id in big-endian)" : ""));
    }

    static void ReadFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found: " + filePath);
            return;
        }

        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (var reader = new BinaryReader(fs, Encoding.UTF8))
        {
            var magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
            if (magic != "MAGC")
            {
                Console.WriteLine("Invalid file format: incorrect magic.");
                return;
            }

            int version = reader.ReadInt32();
            if (version < 1 || version > 2)
            {
                Console.WriteLine("Unsupported version: " + version);
                return;
            }

            Console.WriteLine("Version: " + version);
            Console.WriteLine("Products:");

            while (fs.Position < fs.Length)
            {
                int id = reader.ReadInt32();
                double price = reader.ReadDouble();
                string name = reader.ReadString();
                string category = version >= 2 ? reader.ReadString() : "(none)";

                Console.WriteLine($"#{id} {name} {price} Category={category}");
            }
        }
    }
}
