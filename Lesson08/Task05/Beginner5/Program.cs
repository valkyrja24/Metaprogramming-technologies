using System;
using System.Linq;
using System.Reflection;

// Define the plugin interface
public interface IPlugin
{
    void Execute();
}

// Plugin that prints a hello message
public class HelloPlugin : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Hello from HelloPlugin!");
    }
}

// Plugin that prints the current time
public class TimePlugin : IPlugin
{
    public void Execute()
    {
        Console.WriteLine($"Current time: {DateTime.Now}");
    }
}

// Another plugin example
public class GoodbyePlugin : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Goodbye from GoodbyePlugin!");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Dynamically discovering all IPlugin implementations...");

        // Metaprogramming: discover all classes implementing IPlugin at runtime
        var pluginTypes = Assembly.GetExecutingAssembly()
                                  .GetTypes()
                                  .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface);

        foreach (var type in pluginTypes)
        {
            // Create instance dynamically
            IPlugin plugin = Activator.CreateInstance(type) as IPlugin;
            plugin?.Execute(); // dynamically execute method
        }
    }
}

/*
Demonstrates:
- How to define an interface IPlugin and implement it in multiple classes.
- Runtime discovery of classes implementing IPlugin using reflection (Assembly.GetExecutingAssembly()).
- Dynamically creating instances and calling Execute() without hardcoding class names.
- Example of metaprogramming: program inspects its own metadata and executes code dynamically at runtime.
*/
