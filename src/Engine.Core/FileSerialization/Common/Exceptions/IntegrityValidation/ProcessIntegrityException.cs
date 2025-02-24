using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IntegrityValidation;

internal sealed class ProcessIntegrityException : FileSerializationException
{
    public ProcessIntegrityException(string message)
        : base(message)
    {
    }

    public ProcessIntegrityException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
