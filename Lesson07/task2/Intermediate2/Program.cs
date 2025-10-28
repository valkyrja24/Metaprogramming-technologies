using System;

public delegate void Notifier(string message);

class Program
{
    static void Main(string[] args)
    {
        Notifier notifier = null;

        notifier += NamedHandler;

        int count = 0;
        Notifier lambdaHandler = null;
        lambdaHandler = msg =>
        {
            Console.WriteLine("Lambda handler received: " + msg);
            count++;
            if (count >= 5)
            {
                notifier -= lambdaHandler;
                Console.WriteLine("Lambda handler unsubscribed after 5 calls.");
            }
        };
        notifier += lambdaHandler;

        notifier += msg => Console.WriteLine("Another lambda handler received: " + msg);

        for (int i = 1; i <= 7; i++)
        {
            SafeInvoke(notifier, "Message " + i);
        }
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
