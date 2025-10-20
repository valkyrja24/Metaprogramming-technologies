using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

class UserProfile
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("full_name")]
    public string FullName { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("registered_utc")]
    public DateTimeOffset RegisteredUtc { get; set; }

    [JsonIgnore]
    public bool IsInternal { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: save <file> | load <file>");
            return;
        }

        string command = args[0];
        string filePath = args[1];

        if (command.Equals("save", StringComparison.OrdinalIgnoreCase))
            SaveProfiles(filePath);
        else if (command.Equals("load", StringComparison.OrdinalIgnoreCase))
            LoadProfiles(filePath);
        else
            Console.WriteLine("Unknown command.");
    }

    static void SaveProfiles(string filePath)
    {
        var profiles = new List<UserProfile>
        {
            new UserProfile { Id = 1, FullName = "Alice Smith", Email = "alice@example.com", RegisteredUtc = DateTimeOffset.UtcNow, IsInternal = true },
            new UserProfile { Id = 2, FullName = "Bob Johnson", Email = "bob@example.com", RegisteredUtc = DateTimeOffset.UtcNow, IsInternal = false }
        };

        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(profiles, options);
        File.WriteAllText(filePath, json);

        Console.WriteLine("Profiles saved to " + filePath);
    }

    static void LoadProfiles(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found: " + filePath);
            return;
        }

        string json = File.ReadAllText(filePath);
        var profiles = JsonSerializer.Deserialize<List<UserProfile>>(json);

        foreach (var p in profiles)
        {
            Console.WriteLine($"Id={p.Id}, FullName={p.FullName}, Email={p.Email}, RegisteredUtc={p.RegisteredUtc:O}");
        }
    }
}
