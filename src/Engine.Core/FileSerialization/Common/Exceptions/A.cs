using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions;

internal  class A: FileSerializationException
{
    private const  string _msg = "Unable to read the next {0} byte(s), the specified byte count must be a non-negative value.";

    public     A     (int count, Exception inner)
        : base(string.Format(_msg, count), inner)
    {
    }
}
