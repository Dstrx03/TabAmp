using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions;

internal sealed class FileSerializationIntegrityException : FileSerializationException
{
    public FileSerializationIntegrityException(string message)
        : base(message)
    {
    }

    public FileSerializationIntegrityException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
