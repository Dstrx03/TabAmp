using System;
using System.Collections.Generic;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain.Validation;

internal static class ServiceDecoratorDescriptorChainValidator
{
    internal static ServiceDecoratorDescriptorChainValidationResult Validate<TService>(
        ServiceDecoratorDescriptorChain<TService> descriptorChain,
        bool stopOnFirstError = false)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(descriptorChain);

        List<Exception>? errors = null;

        if (HasDisposableContainer(descriptorChain) && !descriptorChain.IsDisposableContainerAllowed)
        {
            var error = DisposableContainerIsNotAllowedException();
            if (!TryAdd(ref errors, error, stopOnFirstError))
                return new(error);
        }

        var err0 = new NotSupportedException("Some error.");
        if (!TryAdd(ref errors, err0, stopOnFirstError))
            return new(err0);

        var err1 = new NullReferenceException("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
        if (!TryAdd(ref errors, err1, stopOnFirstError))
            return new(err1);

        var err2 = new NotImplementedException("Some other error.");
        if (!TryAdd(ref errors, err2, stopOnFirstError))
            return new(err2);

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

    private static bool TryAdd(ref List<Exception>? errors, Exception error, bool stopOnFirstError)
    {
        if (stopOnFirstError)
            return false;

        errors ??= [];
        errors.Add(error);

        return true;
    }

    private static InvalidOperationException DisposableContainerIsNotAllowedException() =>
        new("At least one inner decorator type requires disposal, " +
            "but the use of a decorator disposable container is not allowed.");
}
