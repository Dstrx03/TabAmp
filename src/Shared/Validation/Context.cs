using System;
using System.Collections.Generic;

namespace TabAmp.Shared.Validation;

public readonly ref struct Context
{
    private readonly List<Exception>? _errors;
    public bool StopOnFirstError { get; }
    internal Context WithError(Exception error)
    {
        throw new NotImplementedException();
    }
}
