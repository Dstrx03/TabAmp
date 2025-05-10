using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.FileOpenFailed;

internal sealed class FileOpenFailedException : FileSerializationException
{
    public FileOpenFailedException(Reason reason, Exception inner)
        : base("", inner)
    {
        Reason = reason;
    }

    public Reason Reason { get; }
}
