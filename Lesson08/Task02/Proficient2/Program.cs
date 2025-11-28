using System;
using System.Reflection;
using System.Collections.Generic;

// Interface for plugins
public interface IPlugin
{
    void Execute();
}

// Example plugin class 1
public class PluginA : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("PluginA executed!");
    }

    public void ExtraMethod()
    {
        Console.WriteLine("PluginA ExtraMethod called!");
    }
}

// Example plugin class 2
public class PluginB : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("PluginB executed!");
    }
}

class Program
{
    static void Main()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] types = assembly.GetTypes();

        foreach (Type type in types)
        {
            if (typeof(IPlugin).IsAssignableFrom(type) && type.IsClass)
            {
                Console.WriteLine($"\nFound plugin class: {type.Name}");

                object pluginInstance = Activator.CreateInstance(type);

                // Cache all methods in a dictionary
                Dictionary<string, MethodInfo> methodCache = new Dictionary<string, MethodInfo>();
                foreach (MethodInfo method in type.GetMethods())
                {
                    if (!methodCache.ContainsKey(method.Name))
                        methodCache[method.Name] = method;
                }

                // Try to invoke a method dynamically
                string methodNameToInvoke = "Execute"; // example input
                Console.WriteLine($"Attempting to invoke method: {methodNameToInvoke}");

                if (methodCache.TryGetValue(methodNameToInvoke, out MethodInfo methodInfo))
                {
                    methodInfo.Invoke(pluginInstance, null);
                }
                else
                {
                    Console.WriteLine($"Method '{methodNameToInvoke}' not found!");
                    Console.WriteLine("Available methods:");
                    foreach (var m in methodCache.Keys)
                        Console.WriteLine($"- {m}");
                }
            }
        }
    }
}

/*
Demonstrates:
- How to dynamically inspect and invoke methods on plugin classes.
- Adds runtime safety: if method name is incorrect, lists all available methods.
- Uses Dictionary<string, MethodInfo> to cache methods for faster lookup.
- Metaprogramming: discovers classes, inspects methods, and invokes them at runtime without prior knowledge.
*/
