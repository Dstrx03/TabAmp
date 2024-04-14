using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions;

internal abstract class FileSerializationException : Exception
{
    protected FileSerializationException()
    {
    }

    protected FileSerializationException(string message)
        : base(message)
    {
    }

    protected FileSerializationException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
