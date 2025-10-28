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
            new Student(5, "Eve", "A2", 80, true, "eve@mail.com"),
            new Student(6, "Frank", "A3", 95, true, "frank@mail.com"),
            new Student(7, "Grace", "A1", 70, true, "grace@mail.com"),
            new Student(8, "Hank", "A2", 82, true, "hank@mail.com")
        };

        var topStudents = students
            .Where(s => s.IsActive && s.Avg >= 80)
            .OrderByDescending(s => s.Avg)
            .ThenBy(s => s.Name)
            .Select(s => new { s.Name, s.Avg });

        foreach (var student in topStudents)
        {
            Console.WriteLine("Name: {0}, Avg: {1}", student.Name, student.Avg);
        }

        var groupStats =
            from s in students
            group s by s.Group into g
            select new
            {
                Group = g.Key,
                Count = g.Count(),
                AvgGroup = g.Average(x => x.Avg)
            };

        var sortedGroupStats = groupStats.OrderByDescending(g => g.AvgGroup);

        foreach (var g in sortedGroupStats)
        {
            Console.WriteLine("Group: {0}, Count: {1}, AvgGroup: {2:F2}", g.Group, g.Count, g.AvgGroup);
        }
    }
}
