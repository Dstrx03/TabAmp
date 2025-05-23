using System;
using System.Diagnostics;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IntegrityValidation.Fluent;

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

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public static A The => new FooBar();
}
