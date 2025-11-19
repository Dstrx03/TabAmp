using System;

namespace TabAmp.Shared.Decorator.DescriptorChain;

public abstract class ServiceDecoratorDescriptor<TService>
    where TService : notnull
{
    private const int MinPosition = 1;
    private const int MaxPosition = int.MaxValue;

    internal ServiceDecoratorDescriptor<TService>? Next { get; private set; }
    internal int? Position { get; private set; }

    private ServiceDecoratorDescriptor()
    {
    }

    public bool IsAppended => Position is not null;

    private protected abstract string? DecoratorTypeFullName { get; }

    internal ServiceDecoratorDescriptor<TService> AppendTo(ServiceDecoratorDescriptor<TService>? descriptors)
    {
        if (IsAppended)
            throw CannotAppendDescriptorAlreadyAppended(this);

        if (descriptors?.IsAppended == false)
            throw CannotAppendToNotAppendedDescriptor(this, descriptors);

        if (descriptors?.Position >= MaxPosition)
            throw CannotAppendCollectionExceededSupportedLimit(this);

        Next = descriptors;
        Position = descriptors?.Position + 1 ?? MinPosition;

        return this;
    }

    internal ServiceDecoratorDescriptorChain<TService> ToChain(ServiceDecoratorDescriptorChain<TService> descriptorChain)
    {
        if (!IsAppended)
            throw CannotConvertToChainDescriptorIsNotAppended(this);

        return CreateDescriptorChainNode(descriptorChain);
    }

    private protected abstract ServiceDecoratorDescriptorChain<TService> CreateDescriptorChainNode(
        ServiceDecoratorDescriptorChain<TService> descriptorChain);

    public class For<TDecorator> : ServiceDecoratorDescriptor<TService>
        where TDecorator : notnull, TService
    {
        private protected sealed override string? DecoratorTypeFullName => typeof(TDecorator).FullName;

        private protected sealed override ServiceDecoratorDescriptorChain<TService> CreateDescriptorChainNode(
            ServiceDecoratorDescriptorChain<TService> descriptorChain)
        {
            return new ServiceDecoratorDescriptorChain<TService>.For<TDecorator>(next: descriptorChain);
        }
    }

    private static InvalidOperationException CannotAppendDescriptorAlreadyAppended(
        ServiceDecoratorDescriptor<TService> descriptor) =>
        new($"Cannot append decorator descriptor for '{descriptor.DecoratorTypeFullName}': " +
            $"already appended at position {descriptor.Position}. " +
            $"Decorated type: '{typeof(TService).FullName}'.");

    private static InvalidOperationException CannotAppendToNotAppendedDescriptor(
        ServiceDecoratorDescriptor<TService> descriptor,
        ServiceDecoratorDescriptor<TService> descriptors) =>
        new($"Cannot append decorator descriptor for '{descriptor.DecoratorTypeFullName}' " +
            $"to descriptor for '{descriptors.DecoratorTypeFullName}' that is not part of a collection. " +
            $"Decorated type: '{typeof(TService).FullName}'.");

    private static InvalidOperationException CannotAppendCollectionExceededSupportedLimit(
        ServiceDecoratorDescriptor<TService> descriptor) =>
        new($"Cannot append decorator descriptor for '{descriptor.DecoratorTypeFullName}': " +
            $"collection exceeded the supported limit of {MaxPosition}. " +
            $"Decorated type: '{typeof(TService).FullName}'.");

    private static InvalidOperationException CannotConvertToChainDescriptorIsNotAppended(
        ServiceDecoratorDescriptor<TService> descriptor) =>
        new("Cannot convert decorator descriptor to a decorator descriptor chain node: " +
            $"descriptor for '{descriptor.DecoratorTypeFullName}' is not part of a collection. " +
            $"Decorated type: '{typeof(TService).FullName}'.");
}
