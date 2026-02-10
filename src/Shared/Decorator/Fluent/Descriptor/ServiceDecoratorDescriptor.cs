using System;
using TabAmp.Shared.Decorator.Core.DescriptorChain;

namespace TabAmp.Shared.Decorator.Fluent.Descriptor;

public abstract class ServiceDecoratorDescriptor<TService>
    where TService : class
{
    internal ServiceDecoratorDescriptor<TService>? Next { get; private set; }

    public bool IsBound { get; private set; }

    private ServiceDecoratorDescriptor()
    {
    }

    internal ServiceDecoratorDescriptor<TService> AppendTo(ServiceDecoratorDescriptor<TService>? target)
    {
        if (IsBound)
            throw CannotAppendDescriptorAlreadyBoundException(this);

        if (target?.IsBound == false)
            throw CannotAppendTargetDescriptorIsNotBoundException(this, target);

        Next = target;
        IsBound = true;

        return this;
    }

    internal ServiceDecoratorDescriptorChain<TService, TImplementation> ToDescriptorChainNode<TImplementation>(
        ServiceDecoratorDescriptorChain<TService, TImplementation> descriptorChain,
        ServiceDecoratorDescriptorChainOptions options = default)
        where TImplementation : class, TService
    {
        if (!IsBound)
            throw CannotConvertToDescriptorChainNodeDescriptorIsNotBoundException(this);

        return CreateDescriptorChainNode(descriptorChain, options);
    }

    private protected abstract ServiceDecoratorDescriptorChain<TService, TImplementation> CreateDescriptorChainNode<TImplementation>(
        ServiceDecoratorDescriptorChain<TService, TImplementation> descriptorChain,
        ServiceDecoratorDescriptorChainOptions options)
        where TImplementation : class, TService;

    private protected abstract Type ToDecoratorType();

    public class For<TDecorator> : ServiceDecoratorDescriptor<TService>
        where TDecorator : class, TService
    {
        private protected sealed override ServiceDecoratorDescriptorChain<TService, TImplementation> CreateDescriptorChainNode<TImplementation>(
            ServiceDecoratorDescriptorChain<TService, TImplementation> descriptorChain,
            ServiceDecoratorDescriptorChainOptions options)
        {
            return ServiceDecoratorDescriptorChain<TService, TImplementation>
                .CreateNode<TDecorator>(next: descriptorChain, options: options);
        }

        private protected sealed override Type ToDecoratorType() => typeof(TDecorator);
    }

    private static InvalidOperationException CannotAppendDescriptorAlreadyBoundException(
        ServiceDecoratorDescriptor<TService> descriptor) =>
        new($"Cannot append decorator descriptor for '{descriptor.ToDecoratorType().FullName}': " +
            "descriptor is already bound to the chain configuration. " +
            $"Decorated type: '{typeof(TService).FullName}'.");

    private static ArgumentException CannotAppendTargetDescriptorIsNotBoundException(
        ServiceDecoratorDescriptor<TService> descriptor,
        ServiceDecoratorDescriptor<TService> target) =>
        new($"Cannot append decorator descriptor for '{descriptor.ToDecoratorType().FullName}' " +
            $"to decorator descriptor for '{target.ToDecoratorType().FullName}': " +
            "target descriptor is not bound to the chain configuration. " +
            $"Decorated type: '{typeof(TService).FullName}'.",
            nameof(target));

    private static InvalidOperationException CannotConvertToDescriptorChainNodeDescriptorIsNotBoundException(
        ServiceDecoratorDescriptor<TService> descriptor) =>
        new($"Cannot convert decorator descriptor for '{descriptor.ToDecoratorType().FullName}' " +
            "to decorator descriptor chain node: descriptor is not bound to the chain configuration. " +
            $"Decorated type: '{typeof(TService).FullName}'.");
}
