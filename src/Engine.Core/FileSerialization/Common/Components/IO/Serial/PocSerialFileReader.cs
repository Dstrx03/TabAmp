using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Context;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Metadata;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Fluent;
using static TabAmp.Engine.Core.FileSerialization.Common.Components.IO.Serial.ISerialFileReader;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.IO.Serial;

[Obsolete("Proof of concept implementation")]
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
        _length = _fileStream.Length;
        _arrayPool = ArrayPool<byte>.Shared;
        _context = context;

    }

    private long _length;
    private long _position;

    private FileDeserializationMetadata _metadata;
    public IFileDeserializationMetadata Metadata
    {
        get
        {
            _metadata ??= new FileDeserializationMetadata { Length = _length };
            _metadata.ProcessedBytes = _position;
            return _metadata;
        }
    }

    public async ValueTask<T> ReadBytesAsync<T>(int count, Convert<T> convert)
    {
        byte[]? buffer = null;

        try
        {
            buffer = _arrayPool.Rent(count);

            await _fileStream.ReadExactlyAsync(buffer, offset: 0, count, _context.CancellationToken);
            _position += count;

            return convert(buffer.AsSpan(start: 0, count));
        }
        catch (ArgumentOutOfRangeException exception) when (count < 0)
        {
            throw NegativeBytesCountOperationException.With.Read.Build(count, exception);
        }
        catch (EndOfStreamException exception)
        {
            _fileStream.Position = _position;
            throw EndOfFileOperationException.With.Read.Build(count, CalculateTrailingBytesCount(count), exception);
        }
        finally
        {
            if (buffer != null)
                _arrayPool.Return(buffer);
        }
    }

    private static readonly IOperationExceptionFluentBuilder<NegativeBytesCountOperationException>
        _negativeBytesCountOperationExceptionWithReadSkip = NegativeBytesCountOperationException.With.ReadSkip;

    private static readonly IOperationExceptionFluentBuilder<EndOfFileOperationException>
        _endOfFileOperationExceptionWithReadSkip = EndOfFileOperationException.With.ReadSkip;

    public async ValueTask SkipBytesAsync(int count)
    {
        _negativeBytesCountOperationExceptionWithReadSkip.ThrowIfNegative(count);
        _endOfFileOperationExceptionWithReadSkip.ThrowIfTrailing(count, CalculateTrailingBytesCount(count));

        var skippedBytes = await ReadBytesAsync(count, buffer => buffer.ToArray());
        Console.WriteLine($"Skipped {count} bytes from {_position - count} to {_position - 1} inclusive: {string.Join(",", skippedBytes)}");

        // TODO: implement production grade tracking of skipped information
        //_fileStream.Position += count;
        //Position += count;
    }

    public void Dispose()
    {
        PrintStatistics();
        _fileStream?.Dispose();
    }

    private long CalculateTrailingBytesCount(int count) => _position - _length + count;

    [Obsolete("TODO: implement production grade tracking")]
    private void PrintStatistics()
    {
        var message = $"read {_position} ({_fileStream.Position}) of {_length} bytes";
        var ratio = _position / (double)_length;
        var diff = _position - _length;
        var diffStr = diff > 0 ? $"+{diff}" : diff.ToString();

        var summary = string.Empty;
        var summaryColor = ConsoleColor.DarkRed;
        if (_position == _length)
        {
            summary = "FULL";
            summaryColor = ConsoleColor.DarkGreen;
        }
        if (_position < _length) summary = "PRTL";
        if (_position > _length) summary = "EXCD";

        Console.Write($"[{nameof(PocSerialFileReader)}] {message} | {ratio:P} {diffStr}b ");
        var color = Console.ForegroundColor;
        Console.ForegroundColor = summaryColor;
        Console.Write($"*{summary}*");
        Console.ForegroundColor = color;
        Console.WriteLine($" | {_context.FilePath}");
    }

    private class FileDeserializationMetadata : IFileDeserializationMetadata
    {
        public long? Length { get; set; }
        public long? ProcessedBytes { get; set; }
    }
}
