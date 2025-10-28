using System;

public delegate void Notifier(string message);

class Program
{
    static void Main(string[] args)
    {
        Notifier notifier = null;

        notifier += NamedHandler;
        notifier += delegate (string msg) { Console.WriteLine("Anonymous handler received: " + msg); };
        notifier += msg => Console.WriteLine("Lambda handler received: " + msg);

        SafeInvoke(notifier, "Hello, subscribers!");

        notifier -= NamedHandler;

        SafeInvoke(notifier, "After removing named handler");
    }

    static void NamedHandler(string message)
    {
        Console.WriteLine("Named handler received: " + message);
    }

    static void SafeInvoke(Notifier notifier, string message)
    {
        if (notifier != null)
        {
            notifier(message);
        }
    }
}
