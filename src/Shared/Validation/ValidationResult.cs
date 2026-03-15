using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace TabAmp.Shared.Validation;

public readonly ref struct ValidationResult
{
    private readonly ImmutableArray<Exception> _errors;

    internal ValidationResult(Exception? error) =>
        _errors = error is not null ? [error] : [];

    internal ValidationResult(IEnumerable<Exception>? errors) =>
        _errors = errors?.ToImmutableArray() ?? [];

    public IEnumerable<Exception> Errors => _errors;
    public bool IsValid => _errors.IsEmpty;

    public void ThrowIfAnyErrors(string? message = null)
    {
        if (IsValid)
            return;

        throw new ValidationException(message, Errors);
    }
}
