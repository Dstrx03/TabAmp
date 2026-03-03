using System;
using System.Collections.Generic;
using TabAmp.Shared.Decorator.Fluent;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain.Validation;

public static class ServiceDecoratorDescriptorChainValidator
{
    public static ServiceDecoratorDescriptorChainValidationResult Validate<TService, TImplementation>(
        ServiceDecoratorDescriptorChain<TService, TImplementation> descriptorChain,
        bool stopOnFirstError = false)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(descriptorChain);

        return _ValidateCore_TEMP(descriptorChain, stopOnFirstError);
    }

    [Obsolete]
    private static ServiceDecoratorDescriptorChainValidationResult ValidateCore<TService, TImplementation>(
        ServiceDecoratorDescriptorChain<TService, TImplementation> descriptorChain,
        bool stopOnFirstError)
        where TService : class
        where TImplementation : class, TService
    {
        List<Exception>? errors = null;

        var isImplementationServiceDisposable = descriptorChain.IsImplementationServiceDisposable
            || descriptorChain.IsImplementationServiceAsyncDisposable;

        if (isImplementationServiceDisposable && !descriptorChain.UseStandaloneImplementationService)
        {
            var error = DisposableImplementationServiceMustBeRegisteredAsStandaloneException(typeof(TImplementation));
            if (!error.TryAddTo(ref errors, stopOnFirstError))
                return new(error);
        }

        var hasDisposableContainer = HasDisposableContainer(descriptorChain);

        if (hasDisposableContainer && !descriptorChain.IsServiceInterface)
        {
            var error = DisposableContainerCannotBeUsedWhenServiceIsNotInterfaceException();
            if (!error.TryAddTo(ref errors, stopOnFirstError))
                return new(error);
        }

        if (hasDisposableContainer && !descriptorChain.IsDisposableContainerAllowed)
        {
            var error = DisposableContainerIsNotAllowedException();
            if (!error.TryAddTo(ref errors, stopOnFirstError))
                return new(error);
        }

        return new(errors);
    }

    [Obsolete]
    private static ServiceDecoratorDescriptorChainValidationResult _ValidateCore_TEMP<TService, TImplementation>(
        ServiceDecoratorDescriptorChain<TService, TImplementation> descriptorChain,
        bool stopOnFirstError)
        where TService : class
        where TImplementation : class, TService
    {
        List<Exception>? errors = null;

        var isImplementationServiceDisposable = descriptorChain.IsImplementationServiceDisposable
            || descriptorChain.IsImplementationServiceAsyncDisposable;

        if (isImplementationServiceDisposable && !descriptorChain.UseStandaloneImplementationService)
        {
            var error = DisposableImplementationServiceMustBeRegisteredAsStandaloneException(typeof(TImplementation));
            if (!error.TryAddTo(ref errors, stopOnFirstError))
                return new(error);
        }

        var hasDisposableContainer = HasDisposableContainer(descriptorChain);

        if (hasDisposableContainer && !descriptorChain.IsServiceInterface)
        {
            var error = DisposableContainerCannotBeUsedWhenServiceIsNotInterfaceException();
            if (!error.TryAddTo(ref errors, stopOnFirstError))
                return new(error);
        }

        if (hasDisposableContainer && !descriptorChain.IsDisposableContainerAllowed)
        {
            var error = DisposableContainerIsNotAllowedException();
            if (!error.TryAddTo(ref errors, stopOnFirstError))
                return new(error);
        }

        return new(errors);
    }

    private static void _Validate_TODO()
    {

    }

    private static bool HasDisposableContainer<TService, TImplementation>(
        ServiceDecoratorDescriptorChain<TService, TImplementation> descriptorChain)
        where TService : class
        where TImplementation : class, TService
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

    private static bool TryAddTo(this Exception error, ref List<Exception>? errors, bool stopOnFirstError)
    {
        if (stopOnFirstError)
            return false;

        errors ??= [];
        errors.Add(error);

        return true;
    }

    private static InvalidOperationException DisposableImplementationServiceMustBeRegisteredAsStandaloneException(
        Type implementationServiceType) =>
        new($"Decorated implementation type '{implementationServiceType.FullName}' requires disposal " +
            "and must be registered as standalone service. " +
            "Specify implementation service key or " +
            $"use {nameof(ServiceDecoratorDescriptorChainOptions)}" +
            $".{nameof(ServiceDecoratorDescriptorChainOptions.UseDefaultImplementationServiceKey)}.");

    private static NotSupportedException DisposableContainerCannotBeUsedWhenServiceIsNotInterfaceException() =>
        new("At least one inner decorator type requires disposal, " +
            "but the decorator disposable container cannot be used " +
            "when the decorated type is not an interface.");

    private static InvalidOperationException DisposableContainerIsNotAllowedException() =>
        new("At least one inner decorator type requires disposal, " +
            "but the use of a decorator disposable container is not allowed. " +
            $"Use {nameof(ServiceDecoratorDescriptorChainFluentBuilder<object, object>)}" +
            $".{nameof(ServiceDecoratorDescriptorChainFluentBuilder<object, object>.AllowDisposableContainer)} " +
            $"({nameof(ServiceDecoratorDescriptorChainOptions)}" +
            $".{nameof(ServiceDecoratorDescriptorChainOptions.IsDisposableContainerAllowed)}) " +
            "to allow the use of a decorator disposable container.");
}
