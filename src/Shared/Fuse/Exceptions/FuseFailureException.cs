using System;
using System.Collections.Generic;
using System.Linq;

namespace TabAmp.Shared.Fuse.Exceptions;

public class FuseFailureException : Exception
{
    public IEnumerable<Exception> Errors { get; }

    internal FuseFailureException(string? message, IEnumerable<Exception> errors)
        : base(message, errors.First())
    {
        Errors = errors;
    }
}
