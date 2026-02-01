using System;
using System.Collections.Generic;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain.Validation;

internal static class ServiceDecoratorDescriptorChainValidator
{
    internal static void ValidateAndThrow<TService>(
        ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : class => Validate(descriptorChain).ThrowFirstErrorIfAny();

    internal static ServiceDecoratorDescriptorChainValidationResult Validate<TService>(
        ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(descriptorChain);
        List<Exception>? errors = null;
        var hasDisposableContainer = false;
        var descriptor = descriptorChain;
        while (descriptor is not null)
        {
            var isInner = descriptor.Next is not null;
            var isDisposable = descriptor.IsDecoratorDisposable || descriptor.IsDecoratorAsyncDisposable;
            if (isDisposable && (isInner || hasDisposableContainer)) hasDisposableContainer = true;
            descriptor = descriptor.Next;
        }
        if (hasDisposableContainer && !descriptorChain.IsDisposableContainerAllowed)
        {
            errors ??= [];
            errors.Add(new InvalidOperationException("TODO: ..."));
        }
        return new(errors);
    }
}
