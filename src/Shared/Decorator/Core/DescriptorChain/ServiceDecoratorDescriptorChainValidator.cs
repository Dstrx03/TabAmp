using System;
using TabAmp.Shared.Decorator.Fluent;
using TabAmp.Shared.Fuse;
using TabAmp.Shared.Fuse.Extensions;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

public static class ServiceDecoratorDescriptorChainValidator
{
    public static FuseResult Validate<TService, TImplementation>(
        ServiceDecoratorDescriptorChain<TService, TImplementation> descriptorChain,
        FuseScope scope = default)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(descriptorChain);

        if (ValidateChain(descriptorChain, scope.ToInner()).ShouldStop(ref scope))
            return scope;

        if (ValidateDescriptors(descriptorChain, scope.ToInner()).ShouldStop(ref scope))
            return scope;

        return scope;
    }

    private static FuseResult ValidateChain<TService, TImplementation>(
        ServiceDecoratorDescriptorChain<TService, TImplementation> descriptorChain,
        FuseScope scope = default)
        where TService : class
        where TImplementation : class, TService
    {
        var isImplementationServiceDisposable =
            descriptorChain.IsImplementationServiceDisposable ||
            descriptorChain.IsImplementationServiceAsyncDisposable;

        if (isImplementationServiceDisposable && !descriptorChain.UseStandaloneImplementationService)
        {
            var error = DisposableImplementationServiceMustBeRegisteredAsStandaloneException(typeof(TImplementation));
            if (error.ShouldStop(ref scope)) return scope;
        }

        return scope;
    }

    private static FuseResult ValidateDescriptors<TService, TImplementation>(
        ServiceDecoratorDescriptorChain<TService, TImplementation> descriptorChain,
        FuseScope scope = default)
        where TService : class
        where TImplementation : class, TService
    {
        var hasDisposableContainer = HasDisposableContainer(descriptorChain);

        if (hasDisposableContainer && !descriptorChain.IsServiceInterface)
        {
            var error = DisposableContainerCannotBeUsedWhenServiceIsNotInterfaceException();
            if (error.ShouldStop(ref scope)) return scope;
        }

        if (hasDisposableContainer && !descriptorChain.IsDisposableContainerAllowed)
        {
            var error = DisposableContainerIsNotAllowedException();
            if (error.ShouldStop(ref scope)) return scope;
        }

        return scope;
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
