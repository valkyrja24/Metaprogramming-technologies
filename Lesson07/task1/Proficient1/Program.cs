using System;
using System.Collections.Generic;
using System.Linq;

class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Group { get; set; }
    public int Avg { get; set; }
    public bool IsActive { get; set; }
    public string Email { get; set; }

    public Student(int id, string name, string group, int avg, bool isActive, string email)
    {
        Id = id;
        Name = name;
        Group = group;
        Avg = avg;
        IsActive = isActive;
        Email = email;
    }
}

static class StudentExtensions
{
    public static bool IsTop(this Student s)
    {
        return s.IsActive && s.Avg >= 90;
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Student> students = new List<Student>
        {
            new Student(1, "Alice", "A1", 85, true, "alice@mail.com"),
            new Student(2, "Bob", "A2", 78, true, "bob@mail.com"),
            new Student(3, "Charlie", "A1", 92, true, "charlie@mail.com"),
            new Student(4, "Diana", "A3", 88, false, "diana@mail.com"),
            new Student(5, "Eve", "A2", 95, true, "eve@mail.com"),
            new Student(6, "Frank", "A3", 95, true, "frank@mail.com"),
            new Student(7, "Grace", "A1", 70, true, "grace@mail.com"),
            new Student(8, "Hank", "A2", 82, true, "hank@mail.com"),
            new Student(9, "Ivy", "A1", 90, true, "ivy@mail.com"),
            new Student(10, "Jack", "A3", 91, true, "jack@mail.com")
        };

        var topByGroup = students
            .GroupBy(s => s.Group)
            .Select(g => new
            {
                Group = g.Key,
                TopNames = g.Where(s => s.IsTop()).Select(s => s.Name).ToList()
            })
            .ToList();

        foreach (var g in topByGroup)
        {
            Console.WriteLine("Group: {0}, TopNames: {1}", g.Group, string.Join(", ", g.TopNames));
        }

        var duplicateEmails = students
            .GroupBy(s => s.Email.ToLower())
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateEmails.Any())
        {
            Console.WriteLine("Duplicate emails found: {0}", string.Join(", ", duplicateEmails));
        }
        else
        {
            Console.WriteLine("All emails are unique.");
        }
    }
}
