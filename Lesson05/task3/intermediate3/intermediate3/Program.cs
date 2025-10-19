using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class AsyncFileCopier : IDisposable
{
    private bool _disposed;

    public AsyncFileCopier()
    {
        _disposed = false;
    }

    public async Task CopyAsync(string src, string dst, CancellationToken ct)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(AsyncFileCopier));

        try
        {
            using (var sourceStream = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
            using (var destStream = new FileStream(dst, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
            {
                await sourceStream.CopyToAsync(destStream, 81920, ct);
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
    }

    public void Dispose()
    {
        _disposed = true;
    }
}

class Program
{
    static async Task Main()
    {
        string sourceFile = "source.txt";
        string destFile = "dest.txt";

        File.WriteAllText(sourceFile, "Hello, async file copying!");

        var cts = new CancellationTokenSource();

        var copier = new AsyncFileCopier();
        try
        {
            await copier.CopyAsync(sourceFile, destFile, cts.Token);
            Console.WriteLine("Copy completed successfully.");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Copy canceled.");
        }
        finally
        {
            copier.Dispose();
        }

        Console.WriteLine($"Copied content: {File.ReadAllText(destFile)}");
    }
}
