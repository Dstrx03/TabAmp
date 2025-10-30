using System;
using TabAmp.Shared.Decorator.DescriptorChain;

namespace TabAmp.Shared.Decorator.Fluent;

public readonly ref struct ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation>
    where TService : notnull
    where TImplementation : notnull, TService
{
    private readonly ServiceDecoratorDescriptor<TService> _descriptors;

    internal ServiceDecoratorDescriptorChainFluentBuilder(
        ServiceDecoratorDescriptor<TService> descriptors,
        bool isNormalized)
    {
        _descriptors = descriptors;
        IsNormalized = isNormalized;
    }

    internal ServiceDecoratorDescriptorChainFluentBuilder(bool isNormalized)
        : this(null!, isNormalized)
    {
    }

    internal readonly bool IsNormalized { get; }

    internal bool IsEmpty => _descriptors is null;
    internal bool IsSingle => !IsEmpty && _descriptors.Next is null;

    public ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService>.For<TDecorator>(_descriptors);
        return new(descriptor, IsNormalized);
    }

    internal ServiceDecoratorDescriptor<TService> BuildDescriptorChain()
    {
        if (IsEmpty)
            throw AtLeastOneDescriptorRequiredException(typeof(TService));

        if (IsNormalized || IsSingle)
            return _descriptors;

        return NormalizeDescriptorChain();
    }

    private ServiceDecoratorDescriptor<TService> NormalizeDescriptorChain()
    {
        ServiceDecoratorDescriptor<TService> descriptorChain = null!;
        var descriptor = _descriptors;
        while (descriptor is not null)
        {
            descriptorChain = descriptor with { Next = descriptorChain };
            descriptor = descriptor.Next;
        }

        return descriptorChain;
    }

    private static InvalidOperationException AtLeastOneDescriptorRequiredException(Type serviceType) =>
        new($"Cannot build decorator descriptor chain for the decorated type '{serviceType.FullName}'. " +
            "At least one decorator descriptor is required.");
}
