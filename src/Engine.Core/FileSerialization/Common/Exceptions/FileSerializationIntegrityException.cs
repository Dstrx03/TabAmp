namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions;

internal class FileSerializationIntegrityException : FileSerializationException
{
    public FileSerializationIntegrityException(string message) : base(message)
    {
    }
}
