using System;
using System.Collections.Generic;
using System.Linq;

namespace TabAmp.Shared.Validation.Exceptions;

public class ValidationException : Exception
{
    public IEnumerable<Exception> Errors { get; }

    internal ValidationException(string? message, IEnumerable<Exception> errors)
        : base(message, errors.First())
    {
        Errors = errors;
    }
}
