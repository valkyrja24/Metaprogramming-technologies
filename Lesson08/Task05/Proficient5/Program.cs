using System;
using System.Linq;
using System.Reflection;

// Custom attribute to store plugin description
[AttributeUsage(AttributeTargets.Class)]
public class PluginInfoAttribute : Attribute
{
    public string Description { get; }

    public PluginInfoAttribute(string description)
    {
        Description = description;
    }
}

// Plugin interface
public interface IPlugin
{
    void Execute();
}

// Plugins with [PluginInfo] attribute

[PluginInfo("Prints a hello message")]
public class HelloPlugin : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("Hello from HelloPlugin!");
    }
}

[PluginInfo("Displays the current time")]
public class TimePlugin : IPlugin
{
    public void Execute()
    {
        Console.WriteLine($"Current time: {DateTime.Now}");
    }
}

[PluginInfo("Says goodbye")]
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
        Console.WriteLine("Discovering all IPlugin implementations with PluginInfo...\n");

        var pluginTypes = Assembly.GetExecutingAssembly()
                                  .GetTypes()
                                  .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface);

        foreach (var type in pluginTypes)
        {
            // Get the PluginInfo attribute
            var attr = type.GetCustomAttribute<PluginInfoAttribute>();

            // Create instance dynamically
            IPlugin plugin = Activator.CreateInstance(type) as IPlugin;

            // Print plugin name and description
            Console.WriteLine($"Plugin: {type.Name}");
            if (attr != null)
                Console.WriteLine($"Description: {attr.Description}");

            // Execute plugin method dynamically
            plugin?.Execute();

            Console.WriteLine(); // extra line for readability
        }
    }
}

/*
Demonstrates:
- How to create and use a custom attribute [PluginInfo] with a parameter for plugin description.
- Reflection is used to:
    * Discover all classes implementing IPlugin at runtime.
    * Read attribute values (Description) dynamically.
    * Create instances dynamically and execute Execute() method.
- Example of metaprogramming:
    * Program inspects its own types and attributes at runtime.
    * Dynamically executes logic based on metadata, without hardcoding class names or descriptions.
- Output shows both the plugin name and its description dynamically.
*/
