using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain.Validation;

internal readonly ref struct ServiceDecoratorDescriptorChainValidationResult(List<Exception>? errors)
{
    internal ImmutableArray<Exception> Errors { get; } = errors?.ToImmutableArray() ?? [];

    internal bool IsValid => Errors.IsEmpty;

    internal void ThrowFirstErrorIfAny()
    {
        if (!IsValid)
        {
            throw Errors[0];
        }
    }
}
