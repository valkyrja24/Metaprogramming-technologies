using System;

static class ActionExtensions
{
    public static void SafeInvoke(this Action<string> handler, string msg)
    {
        if (handler == null) return;

        foreach (Action<string> singleHandler in handler.GetInvocationList())
        {
            try
            {
                singleHandler(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Handler error: " + ex.Message);
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Action<string> notifier = null;

        notifier += msg => Console.WriteLine("Handler 1 received: " + msg);
        notifier += msg => throw new InvalidOperationException("Handler 2 failed!");
        notifier += msg => Console.WriteLine("Handler 3 received: " + msg);

        notifier.SafeInvoke("Test message");
    }
}
