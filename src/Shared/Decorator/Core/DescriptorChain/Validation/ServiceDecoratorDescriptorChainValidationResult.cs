using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain.Validation;

internal readonly ref struct ServiceDecoratorDescriptorChainValidationResult(List<Exception>? errors)
{
    internal ImmutableArray<Exception>? Errors { get; } = errors?.ToImmutableArray();

    internal bool IsValid => Errors?.IsEmpty ?? true;

    internal void ThrowFirstErrorIfAny()
    {
        var firstError = Errors?[0];
        if (firstError is not null)
            throw firstError;
    }
}
