using System;
using System.Buffers;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Context;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Metadata;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.FileOpenFailed;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.Operation;
using static TabAmp.Engine.Core.FileSerialization.Common.Components.IO.Serial.ISerialFileReader;

namespace TabAmp.Engine.Core.FileSerialization.Common.Components.IO.Serial;

internal sealed class SerialFileReader : ISerialFileReader, IDisposable
{
    private readonly FileSerializationContext _context;
    private readonly ArrayPool<byte> _arrayPool;
    private readonly FileDeserializationMetadata _metadata;

    private FileStream _fileStream;
    private readonly long _length;
    private long _position;

    public SerialFileReader(FileSerializationContext context)
    {
        _fileStream = OpenFileStream(context);
        _length = _fileStream.Length;

        _context = context;
        _arrayPool = ArrayPool<byte>.Shared;
        _metadata = new FileDeserializationMetadata(_length);
    }

    private FileStream OpenFileStream(FileSerializationContext context)
    {
        var options = new FileStreamOptions
        {
            Options = FileOptions.Asynchronous,
            Share = FileShare.None
        };

        try
        {
            return File.Open(context.FilePath, options);
        }
        catch (IOException exception)
        {
            var reason = Reason.IOError;

            if (exception is FileNotFoundException)
                reason = Reason.FileNotFound;
            else if (exception is DriveNotFoundException)
                reason = Reason.DriveNotFound;
            else if (exception is DirectoryNotFoundException)
                reason = Reason.InvalidPath;
            else if (exception is PathTooLongException)
                reason = Reason.PathTooLong;
            else if (IsWin32ErrorInvalidName(exception))
                reason = Reason.InvalidPath;

            throw new FileOpenFailedException(reason, exception);
        }
        catch (NotSupportedException exception)
        {
            throw new FileOpenFailedException(Reason.InvalidPath, exception);
        }
        catch (UnauthorizedAccessException exception)
        {
            throw new FileOpenFailedException(Reason.AccessDenied, exception);
        }
        catch (SecurityException exception)
        {
            throw new FileOpenFailedException(Reason.AccessDenied, exception);
        }

        bool IsWin32ErrorInvalidName(IOException exception)
        {
            const int win32ErrorInvalidName = unchecked((int)0x8007007B);
            return OperatingSystem.IsWindows() && exception.HResult == win32ErrorInvalidName;
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

    public ValueTask SkipBytesAsync(int count)
    {
        NegativeBytesCountOperationException.With.ReadSkip.ThrowIfNegative(count);
        EndOfFileOperationException.With.ReadSkip.ThrowIfTrailing(count, CalculateTrailingBytesCount(count));

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
