using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Context;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;
using static TabAmp.Engine.Core.FileSerialization.Common.Components.IO.Serial.ISerialFileReader;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.IO.Serial;

internal class PocSerialFileReader : ISerialFileReader
{
    private readonly FileStream _fileStream;
    private readonly ArrayPool<byte> _arrayPool;
    private readonly FileSerializationContext _context;

    public PocSerialFileReader(FileSerializationContext context)
    {
        var options = new FileStreamOptions
        {
            Options = FileOptions.Asynchronous,
            Share = FileShare.None
        };
        _fileStream = File.Open(context.FilePath, options);
        Length = _fileStream.Length;
        _arrayPool = ArrayPool<byte>.Shared;
        _context = context;
    }

    public long Length { get; }
    public long Position { get; private set; }

    public async ValueTask<T> ReadBytesAsync<T>(int count, Convert<T> convert)
    {
        byte[]? buffer = null;

        try
        {
            buffer = _arrayPool.Rent(count);

            await _fileStream.ReadExactlyAsync(buffer, offset: 0, count, _context.CancellationToken);
            Position += count;

            return convert(buffer.AsSpan(start: 0, count));
        }
        catch (ArgumentOutOfRangeException exception) when (count < 0)
        {
            throw new NegativeBytesCountOperationException(OperationType.Read, count, exception);
        }
        catch (EndOfStreamException exception)
        {
            //_fileStream.Position = Position; // TODO: test

            var trailingCount = (int)CalculateTrailingBytesCount(count);
            throw new EndOfFileOperationException(OperationType.Read, count, trailingCount, exception);
        }
        finally
        {
            if (buffer != null)
                _arrayPool.Return(buffer);
        }
    }

    public async ValueTask SkipBytesAsync(int count)
    {
        NegativeBytesCountOperationException.ThrowIfNegative(OperationType.ReadSkip, count);
        EndOfFileOperationException.ThrowIfTrailing(OperationType.ReadSkip, count, CalculateTrailingBytesCount(count));

        var skippedBytes = await ReadBytesAsync(count, buffer => buffer.ToArray());
        Console.WriteLine($"Skipped {count} bytes from {Position - count} to {Position - 1} inclusive: {string.Join(",", skippedBytes)}");

        // TODO: implement production grade tracking of skipped information
        //_fileStream.Position += count;
        //Position += count;
    }

    public void Dispose()
    {
        PrintStatistics();
        _fileStream?.Dispose();
    }

    private long CalculateTrailingBytesCount(int count) => Position - Length + count;

    [Obsolete("TODO: implement production grade tracking")]
    private void PrintStatistics()
    {
        var message = $"read {Position} ({_fileStream.Position}) of {Length} bytes";
        var ratio = Position / (double)Length;
        var diff = Position - Length;
        var diffStr = diff > 0 ? $"+{diff}" : diff.ToString();

        var summary = string.Empty;
        var summaryColor = ConsoleColor.DarkRed;
        if (Position == Length)
        {
            summary = "FULL";
            summaryColor = ConsoleColor.DarkGreen;
        }
        if (Position < Length) summary = "PRTL";
        if (Position > Length) summary = "EXCD";

        Console.Write($"[{nameof(PocSerialFileReader)}] {message} | {ratio:P} {diffStr}b ");
        var color = Console.ForegroundColor;
        Console.ForegroundColor = summaryColor;
        Console.Write($"*{summary}*");
        Console.ForegroundColor = color;
        Console.WriteLine($" | {_context.FilePath}");
    }
}
