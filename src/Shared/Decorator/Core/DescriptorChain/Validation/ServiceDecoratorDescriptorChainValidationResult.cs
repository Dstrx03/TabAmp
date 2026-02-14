using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain.Validation;

public readonly ref struct ServiceDecoratorDescriptorChainValidationResult
{
    private readonly ImmutableArray<Exception> _errors;

    internal ServiceDecoratorDescriptorChainValidationResult(Exception error) =>
        _errors = [error];

    internal ServiceDecoratorDescriptorChainValidationResult(IEnumerable<Exception>? errors) =>
        _errors = errors?.ToImmutableArray() ?? [];

    public IEnumerable<Exception> Errors => _errors;
    public bool IsValid => _errors.IsEmpty;

    public void ThrowIfAnyErrors(string? message = null)
    {
        if (IsValid)
            return;

        throw new ServiceDecoratorDescriptorChainValidationException(message, Errors);
    }
}
