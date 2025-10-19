using System;
using TimeIntervalLib;

namespace TimeIntervalApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var t1 = new TimeInterval("09:30-11:15");
            var t2 = new TimeInterval("10:00-12:00");

            Console.WriteLine("t1: " + t1);
            Console.WriteLine("t2: " + t2);

            Console.WriteLine("Length of t1: " + t1.Length());
            Console.WriteLine("Do they overlap? " + t1.Overlaps(t2));

            var union = t1 + t2;
            Console.WriteLine("Union (+): " + union);

            var intersection = t1 * t2;
            Console.WriteLine("Intersection (*): " + (intersection != null ? intersection.ToString() : "No intersection"));

            int minutes = (int)t1;
            Console.WriteLine("Explicit to int: " + minutes);

            Console.WriteLine("Indexer [0]: " + t1[0]);
            Console.WriteLine("Indexer \"end\": " + t1["end"]);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
