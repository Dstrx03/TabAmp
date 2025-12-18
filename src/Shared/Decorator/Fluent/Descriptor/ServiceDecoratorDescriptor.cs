using System;
using TabAmp.Shared.Decorator.Core.DescriptorChain;

namespace TabAmp.Shared.Decorator.Fluent.Descriptor;

public abstract class ServiceDecoratorDescriptor<TService>
    where TService : notnull
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

    internal ServiceDecoratorDescriptorChain<TService> ToDescriptorChainNode(
        ServiceDecoratorDescriptorChain<TService> descriptorChain,
        bool useStandaloneImplementationService = false)
    {
        if (!IsBound)
            throw CannotConvertToDescriptorChainNodeDescriptorIsNotBoundException(this);

        if (useStandaloneImplementationService)
            return CreateDescriptorChainHeadNode(descriptorChain);

        return CreateDescriptorChainNode(descriptorChain);
    }

    private protected abstract ServiceDecoratorDescriptorChain<TService> CreateDescriptorChainNode(
        ServiceDecoratorDescriptorChain<TService> descriptorChain);

    private protected abstract ServiceDecoratorDescriptorChain<TService> CreateDescriptorChainHeadNode(
        ServiceDecoratorDescriptorChain<TService> descriptorChain);

    private protected abstract Type ToDecoratorType();

    public class Node<TDecorator> : ServiceDecoratorDescriptor<TService>
        where TDecorator : notnull, TService
    {
        private protected sealed override ServiceDecoratorDescriptorChain<TService> CreateDescriptorChainNode(
            ServiceDecoratorDescriptorChain<TService> descriptorChain)
        {
            return new ServiceDecoratorDescriptorChain<TService>.Node<TDecorator>(next: descriptorChain);
        }

        private protected sealed override ServiceDecoratorDescriptorChain<TService> CreateDescriptorChainHeadNode(
            ServiceDecoratorDescriptorChain<TService> descriptorChain)
        {
            return new ServiceDecoratorDescriptorChain<TService>.HeadNode<TDecorator>(
                next: descriptorChain,
                options: 0);
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
