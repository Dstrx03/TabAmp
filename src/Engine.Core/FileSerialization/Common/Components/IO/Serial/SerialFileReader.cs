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

internal sealed class SerialFileReader : ISerialFileReader
{
    private readonly FileSerializationContext _context;
    private readonly ArrayPool<byte> _arrayPool;
    private readonly FileDeserializationMetadata _metadata;

    private FileStream _fileStream;
    private readonly long _length;
    private long _position;

    private static readonly IOperationExceptionFluentBuilder<NegativeBytesCountOperationException>
        _negativeBytesCountOperationExceptionWithReadSkip = NegativeBytesCountOperationException.With.ReadSkip;

    private static readonly IOperationExceptionFluentBuilder<EndOfFileOperationException>
        _endOfFileOperationExceptionWithReadSkip = EndOfFileOperationException.With.ReadSkip;

    public SerialFileReader(FileSerializationContext context)
    {
        _fileStream = OpenFileStream(context);
        _length = _fileStream.Length;

        _context = context;
        _arrayPool = ArrayPool<byte>.Shared;
        _metadata = new FileDeserializationMetadata(_length);
    }

    private static FileStream OpenFileStream(FileSerializationContext context)
    {
        var options = new FileStreamOptions
        {
            Options = FileOptions.Asynchronous,
            Share = FileShare.None
        };

        return File.Open(context.FilePath, options);
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

    public ValueTask SkipBytesAsync(int count)
    {
        _negativeBytesCountOperationExceptionWithReadSkip.ThrowIfNegative(count);
        _endOfFileOperationExceptionWithReadSkip.ThrowIfTrailing(count, CalculateTrailingBytesCount(count));

        _fileStream.Position += count;
        _position += count;

        return ValueTask.CompletedTask;
    }

    private long CalculateTrailingBytesCount(int count) => _position - _length + count;

    public IFileDeserializationMetadata Metadata
    {
        get
        {
            _metadata.ProcessedBytes = _position;
            return _metadata;
        }
    }

    public void Dispose()
    {
        _fileStream?.Dispose();
        _fileStream = null;
    }

    private class FileDeserializationMetadata(long length) : IFileDeserializationMetadata
    {
        public long? Length { get; } = length;
        public long? ProcessedBytes { get; set; }
    }
}
