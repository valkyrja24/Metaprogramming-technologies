using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class AsyncFileCopier : IDisposable
{
    private bool _disposed;
    public event EventHandler<int> ProgressChanged;

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
            using (var sourceStream = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, useAsync: true))
            using (var destStream = new FileStream(dst, FileMode.Create, FileAccess.Write, FileShare.None, 8192, useAsync: true))
            {
                var buffer = new byte[8192];
                long totalRead = 0;
                long length = sourceStream.Length;

                int read;
                while ((read = await sourceStream.ReadAsync(buffer, 0, buffer.Length, ct)) > 0)
                {
                    await destStream.WriteAsync(buffer, 0, read, ct);
                    totalRead += read;

                    int percent = (int)((totalRead * 100L) / length);
                    ProgressChanged?.Invoke(this, percent);
                }
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            ProgressChanged = null; // відписка від подій
            _disposed = true;
        }
    }
}

class Program
{
    static async Task Main()
    {
        string sourceFile = "source.txt";
        string destFile = "dest.txt";
        File.WriteAllText(sourceFile, "Hello, async file copying with progress!".PadRight(50000, '*'));

        var cts = new CancellationTokenSource();

        var copier = new AsyncFileCopier();
        copier.ProgressChanged += Copier_ProgressChanged;

        try
        {
            await copier.CopyAsync(sourceFile, destFile, cts.Token);
            Console.WriteLine("\nCopy completed successfully.");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("\nCopy canceled.");
        }
        finally
        {
            copier.ProgressChanged -= Copier_ProgressChanged;
            copier.Dispose();
        }

        Console.WriteLine($"Copied content length: {new FileInfo(destFile).Length}");
    }

    private static void Copier_ProgressChanged(object sender, int percent)
    {
        Console.Write($"\rProgress: {percent}%");
    }
}
