using System;
using System.Collections.Generic;
using TabAmp.Shared.Decorator.Fluent;

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

        var hasDisposableContainer = HasDisposableContainer(descriptorChain);

        if (hasDisposableContainer && !descriptorChain.IsServiceInterface)
        {
            var error = DisposableContainerCannotBeUsedWithNonInterfaceException();
            if (!TryAdd(ref errors, error, stopOnFirstError))
                return new(error);
        }

        if (hasDisposableContainer && !descriptorChain.IsDisposableContainerAllowed)
        {
            var error = DisposableContainerIsNotAllowedException();
            if (!TryAdd(ref errors, error, stopOnFirstError))
                return new(error);
        }

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

    private static NotSupportedException DisposableContainerCannotBeUsedWithNonInterfaceException() =>
        new("Decorator disposable container cannot be used with a decorated type that is not an interface.");

    private static InvalidOperationException DisposableContainerIsNotAllowedException() =>
        new("At least one inner decorator type requires disposal, " +
            "but the use of a decorator disposable container is not allowed. " +
            $"Use {nameof(ServiceDecoratorDescriptorChainFluentBuilder<object, object>)}" +
            $".{nameof(ServiceDecoratorDescriptorChainFluentBuilder<object, object>.AllowDisposableContainer)} " +
            $"({nameof(ServiceDecoratorDescriptorChainOptions)}" +
            $".{nameof(ServiceDecoratorDescriptorChainOptions.IsDisposableContainerAllowed)}) " +
            "to allow the use of a decorator disposable container.");
}
