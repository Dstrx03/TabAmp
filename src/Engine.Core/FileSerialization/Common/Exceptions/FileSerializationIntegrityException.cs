namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions;

internal sealed class FileSerializationIntegrityException : FileSerializationException
{
    public FileSerializationIntegrityException(string message)
        : base(message)
    {
    }
}
