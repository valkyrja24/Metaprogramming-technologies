using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

class Program
{
    const int DEFAULT_N = 2_000_000;
    const int RUNS = 5;
    static Random rnd = new Random(123456);

    static int[] GenerateData(int n)
    {
        var arr = new int[n];
        for (int i = 0; i < n; i++)
            arr[i] = rnd.Next();
        return arr;
    }

    static (double ms, long memDelta, long sum) RunArrayList(int[] data)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        var memBefore = GC.GetTotalMemory(true);

        var sw = Stopwatch.StartNew();

        var al = new ArrayList(data.Length);
        foreach (var v in data)
            al.Add(v);

        long sum = 0;
        for (int i = 0; i < al.Count; i++)
            sum += (int)al[i];

        sw.Stop();
        var memAfter = GC.GetTotalMemory(false);
        return (sw.Elapsed.TotalMilliseconds, memAfter - memBefore, sum);
    }

    static (double ms, long memDelta, long sum) RunListInt(int[] data)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        var memBefore = GC.GetTotalMemory(true);

        var sw = Stopwatch.StartNew();

        var list = new List<int>(data.Length);
        foreach (var v in data)
            list.Add(v);

        long sum = 0;
        for (int i = 0; i < list.Count; i++)
            sum += list[i];

        sw.Stop();
        var memAfter = GC.GetTotalMemory(false);
        return (sw.Elapsed.TotalMilliseconds, memAfter - memBefore, sum);
    }

    static (double ms, long memDelta, long sum) RunListObject(int[] data)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        var memBefore = GC.GetTotalMemory(true);

        var sw = Stopwatch.StartNew();

        var list = new List<object>(data.Length);
        foreach (var v in data)
            list.Add((object)v);

        long sum = 0;
        for (int i = 0; i < list.Count; i++)
            sum += (int)list[i];

        sw.Stop();
        var memAfter = GC.GetTotalMemory(false);
        return (sw.Elapsed.TotalMilliseconds, memAfter - memBefore, sum);
    }

    static (double formatMs, double interpMs, double tostringMs) FormattingTest(int reps)
    {
        int[] sample = Enumerable.Range(0, reps).Select(i => i).ToArray();

        GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < sample.Length; i++)
        {
            string s = string.Format("X={0}", sample[i]);
        }
        sw.Stop();
        var formatMs = sw.Elapsed.TotalMilliseconds;

        GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
        sw.Restart();
        for (int i = 0; i < sample.Length; i++)
        {
            string s = $"X={sample[i]}";
        }
        sw.Stop();
        var interpMs = sw.Elapsed.TotalMilliseconds;

        GC.Collect(); GC.WaitForPendingFinalizers(); GC.Collect();
        sw.Restart();
        for (int i = 0; i < sample.Length; i++)
        {
            string s = "X=" + sample[i].ToString(CultureInfo.InvariantCulture);
        }
        sw.Stop();
        var tostringMs = sw.Elapsed.TotalMilliseconds;

        return (formatMs, interpMs, tostringMs);
    }

    static void Main(string[] args)
    {
        int N = DEFAULT_N;
        if (args.Length > 0 && int.TryParse(args[0], out var parsed)) N = parsed;

        Console.WriteLine($"Boxing vs Generics microbenchmark\nN = {N}, runs = {RUNS} (first run per scenario considered warmup)\n");
        Console.WriteLine("Generating data...");
        var data = GenerateData(N);

        Console.WriteLine("Warming up...");
        RunArrayList(new int[1000]);
        RunListInt(new int[1000]);
        RunListObject(new int[1000]);

        void MeasureScenario(string name, Func<int[], (double ms, long mem, long sum)> scenario)
        {
            var msList = new List<double>();
            var memList = new List<long>();
            long checksum = 0;

            for (int i = 0; i < RUNS; i++)
            {
                var (ms, mem, sum) = scenario(data);
                Console.WriteLine($"{name} run {i + 1}: {ms:F0} ms, mem delta {mem:N0} bytes, sum={sum}");
                msList.Add(ms);
                memList.Add(mem);
                checksum = sum;
            }

            var trimmedMs = (RUNS > 1) ? msList.Skip(1).ToArray() : msList.ToArray();
            var trimmedMem = (RUNS > 1) ? memList.Skip(1).ToArray() : memList.ToArray();

            double avgMs = trimmedMs.Average();
            double stdMs = Math.Sqrt(trimmedMs.Average(x => (x - avgMs) * (x - avgMs)));
            double avgMem = trimmedMem.Average();

            Console.WriteLine($"\n{name} summary (avg over {Math.Max(1, RUNS - 1)} runs):");
            Console.WriteLine($"  avg time = {avgMs:F0} ms, stddev = {stdMs:F0} ms, avg mem delta = {avgMem:N0} bytes, checksum = {checksum}\n");
        }

        MeasureScenario("ArrayList", RunArrayList);
        MeasureScenario("List<int>", RunListInt);
        MeasureScenario("List<object>", RunListObject);

        Console.WriteLine("Formatting test (100k reps) ...");
        var (formatMs, interpMs, tostringMs) = FormattingTest(100_000);
        Console.WriteLine($"string.Format (params object[]): {formatMs:F0} ms (boxes ints)");
        Console.WriteLine($"interpolation ($\"X={{x}}\")           : {interpMs:F0} ms (usually avoids boxing)");
        Console.WriteLine($"explicit ToString()                  : {tostringMs:F0} ms (no boxing)\n");

        Console.WriteLine("Done. Notes:");
        Console.WriteLine("- Run in Release and without debugger for accurate numbers.");
        Console.WriteLine("- Increase N to 5_000_000+ if you have enough RAM/CPU.");
        Console.WriteLine("- Results printed above; use output to create a report table.\n");
    }
}
