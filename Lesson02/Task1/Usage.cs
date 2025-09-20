using System;
using Utilities;

class Program
{
    static void Main()
    {
        var task = new TodoItem("Buy milk");
        Console.WriteLine($"{task.Title} - Done? {task.IsDone}");

        task.MarkDone();
        Console.WriteLine($"{task.Title} - Done? {task.IsDone}");

        task.MarkUndone();
        Console.WriteLine($"{task.Title} - Done? {task.IsDone}");

        bool renamed = task.TryRename("Buy bread");
        Console.WriteLine($"Renamed: {renamed}, New title: {task.Title}");

        renamed = task.TryRename(""); // if the task name is empty
        Console.WriteLine($"Renamed: {renamed}, Title still: {task.Title}");
    }
}
