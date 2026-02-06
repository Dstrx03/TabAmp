using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain.Validation;

internal readonly ref struct ServiceDecoratorDescriptorChainValidationResult
{
    private readonly ImmutableArray<Exception> _errors;

    internal ServiceDecoratorDescriptorChainValidationResult(Exception error) =>
        _errors = [error];

    internal ServiceDecoratorDescriptorChainValidationResult(IEnumerable<Exception>? errors) =>
        _errors = errors?.ToImmutableArray() ?? [];

    internal IEnumerable<Exception> Errors => _errors;
    internal bool IsValid => _errors.IsEmpty;

    internal void ThrowIfAnyErrors(string? message = null)
    {
        if (IsValid)
            return;

        throw new ServiceDecoratorDescriptorChainValidationException(message, Errors);
    }
}
