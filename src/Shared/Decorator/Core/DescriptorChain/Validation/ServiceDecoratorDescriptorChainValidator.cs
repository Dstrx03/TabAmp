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
        if (HasDisposableContainer(descriptorChain) && !descriptorChain.IsDisposableContainerAllowed) AddError(ref errors, new InvalidOperationException("TODO: ..."));
        return new(errors);
    }

    private static bool HasDisposableContainer<TService>(ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : class
    {
        var hasDisposableContainer = false;
        var descriptor = descriptorChain;
        while (descriptor is not null)
        {
            var isInner = descriptor.Next is not null;
            var isDisposable = descriptor.IsDecoratorDisposable || descriptor.IsDecoratorAsyncDisposable;
            if (isDisposable && (isInner || hasDisposableContainer)) hasDisposableContainer = true;
            descriptor = descriptor.Next;
        }
        return hasDisposableContainer;
    }

    private static void AddError(ref List<Exception>? errors, Exception error)
    {
        errors ??= [];
        errors.Add(error);
    }
}
