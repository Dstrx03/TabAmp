using System;

namespace TabAmp.Shared.Decorator.DescriptorChain;

public abstract class ServiceDecoratorDescriptor<TService>
    where TService : notnull
{
    public const int MaxPosition = StartPosition + byte.MaxValue;

    private const int StartPosition = 1;

    internal ServiceDecoratorDescriptor<TService>? Next { get; private set; }
    internal int? Position { get; private set; }

    private ServiceDecoratorDescriptor()
    {
    }

    public bool IsAppended => Position is not null;

    internal ServiceDecoratorDescriptor<TService> AppendTo(ServiceDecoratorDescriptor<TService>? descriptors)
    {
        if (IsAppended)
            throw CannotAppendDescriptorAlreadyAppended();

        if (descriptors?.IsAppended == false)
            throw CannotAppendToNotAppendedDescriptor();

        if (descriptors?.Position >= MaxPosition)
            throw CannotAppendCollectionExceededSupportedLimit();

        Next = descriptors;
        Position = descriptors?.Position + 1 ?? StartPosition;

        return this;
    }

    internal ServiceDecoratorDescriptorChain<TService> ToChain(ServiceDecoratorDescriptorChain<TService> descriptorChain)
    {
        if (!IsAppended)
            throw CannotConvertDescriptorIsNotAppended();

        return CreateDescriptorChainNode(descriptorChain);
    }

    private protected abstract ServiceDecoratorDescriptorChain<TService> CreateDescriptorChainNode(
        ServiceDecoratorDescriptorChain<TService> descriptorChain);

    public class For<TDecorator> : ServiceDecoratorDescriptor<TService>
        where TDecorator : notnull, TService
    {
        private protected sealed override ServiceDecoratorDescriptorChain<TService> CreateDescriptorChainNode(
            ServiceDecoratorDescriptorChain<TService> descriptorChain)
        {
            return new ServiceDecoratorDescriptorChain<TService>.For<TDecorator>(next: descriptorChain);
        }
    }

    private static InvalidOperationException CannotAppendDescriptorAlreadyAppended() =>
        new("Cannot append decorator descriptor for 'TDecorator': " +
            "already appended at position 1. " +
            "Decorated type: 'TService'.");

    private static InvalidOperationException CannotAppendToNotAppendedDescriptor() =>
        new("Cannot append decorator descriptor for 'TDecorator1' " +
            "to descriptor for 'TDecorator2' that is not part of a collection. " +
            "Decorated type: 'TService'.");

    private static InvalidOperationException CannotAppendCollectionExceededSupportedLimit() =>
        new("Cannot append decorator descriptor for 'TDecorator': " +
            "collection exceeded the supported limit of 10. " +
            "Decorated type: 'TService'.");

    private static InvalidOperationException CannotConvertDescriptorIsNotAppended() =>
        new("Cannot convert decorator descriptor to a decorator descriptor chain node: " +
            "descriptor for 'TDecorator' is not part of a collection. " +
            "Decorated type: 'TService'.");
}
