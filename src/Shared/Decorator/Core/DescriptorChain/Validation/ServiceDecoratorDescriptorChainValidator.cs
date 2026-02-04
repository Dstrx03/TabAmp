using System;
using System.Collections.Generic;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain.Validation;

internal static class ServiceDecoratorDescriptorChainValidator
{
    internal static void ValidateAndThrow<TService>(
        ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : class => Validate(descriptorChain).ThrowIfAnyErrors();

    internal static ServiceDecoratorDescriptorChainValidationResult Validate<TService>(
        ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(descriptorChain);

        List<Exception>? errors = null;

        if (HasDisposableContainer(descriptorChain) && !descriptorChain.IsDisposableContainerAllowed)
            AddDisposableContainerIsNotAllowedError(ref errors);

        AddError(ref errors, new NotSupportedException("Some error."));
        AddError(ref errors, new NullReferenceException("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."));
        AddError(ref errors, new NotImplementedException("Some other error."));

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
