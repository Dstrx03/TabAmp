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

        if (HasDisposableContainer(descriptorChain) && !descriptorChain.IsDisposableContainerAllowed)
            AddDisposableContainerIsNotAllowedError(ref errors);

        return new(errors);
    }

    private static bool HasDisposableContainer<TService>(ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : class
    {
        var descriptor = descriptorChain;
        while (descriptor.Next is not null)
        {
            if (descriptor.IsDecoratorDisposable || descriptor.IsDecoratorAsyncDisposable)
                return true;

            descriptor = descriptor.Next;
        }

        return false;
    }

    private static void AddDisposableContainerIsNotAllowedError(ref List<Exception>? errors) =>
        AddError(ref errors, DisposableContainerIsNotAllowedException());

    private static void AddError(ref List<Exception>? errors, Exception error)
    {
        errors ??= [];
        errors.Add(error);
    }

    private static InvalidOperationException DisposableContainerIsNotAllowedException() =>
        new("At least one inner decorator type requires disposal, " +
            "but the use of a decorator disposable container is not allowed.");
}
