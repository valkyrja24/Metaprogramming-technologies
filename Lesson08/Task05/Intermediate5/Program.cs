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
        Console.WriteLine("Discovering all IPlugin implementations dynamically...\n");

        // Discover all types implementing IPlugin at runtime
        var pluginTypes = Assembly.GetExecutingAssembly()
                                  .GetTypes()
                                  .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface);

        foreach (var type in pluginTypes)
        {
            // Create instance dynamically
            IPlugin plugin = Activator.CreateInstance(type) as IPlugin;

            // Execute method dynamically
            plugin?.Execute();
        }
    }
}

/*
Demonstrates:
- How to discover all classes implementing IPlugin at runtime using reflection.
- Uses Assembly.GetExecutingAssembly().GetTypes() to inspect types dynamically.
- Dynamically creates instances with Activator.CreateInstance and executes methods.
- Example of metaprogramming:
    * Program inspects its own metadata (types, interfaces) at runtime.
    * Creates objects and calls methods without knowing class names at compile time.
    * Any new class implementing IPlugin is automatically discovered and executed.
- No hardcoded class names; works dynamically with any future IPlugin implementations.
*/
