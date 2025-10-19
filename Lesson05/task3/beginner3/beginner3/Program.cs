using System;
using System.IO;

public class TempFileWriter : IDisposable
{
    private StreamWriter _writer;
    private string _filePath;
    private bool _disposed;

    public string FilePath => _filePath;

    public TempFileWriter()
    {
        _filePath = Path.GetTempFileName();
        _writer = new StreamWriter(_filePath);
        _disposed = false;
    }

    public void WriteLine(string line)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(TempFileWriter));

        _writer.WriteLine(line);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _writer?.Dispose();
            _disposed = true;
        }
    }
}

class Program
{
    static void Main()
    {
        using (var writer = new TempFileWriter())
        {
            writer.WriteLine("Hello, world!");
            writer.WriteLine("This is a temporary file.");
            Console.WriteLine($"File created at: {writer.FilePath}");
        }

        try
        {
            var writer = new TempFileWriter();
            writer.Dispose();
            writer.WriteLine("Should fail");
        }
        catch (ObjectDisposedException ex)
        {
            Console.WriteLine($"Caught expected exception: {ex.Message}");
        }
    }
}
