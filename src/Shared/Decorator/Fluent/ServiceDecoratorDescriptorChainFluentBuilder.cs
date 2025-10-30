using System;

namespace TabAmp.Shared.Decorator.Fluent;

public readonly ref struct ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation>(
    ServiceDecoratorDescriptor<TService> descriptors,
    bool isNormalized)
    where TService : notnull
    where TImplementation : notnull, TService
{
    public ServiceDecoratorDescriptorChainFluentBuilder(bool isNormalized)
        : this(null!, isNormalized)
    {
    }

    internal bool IsEmpty => descriptors is null;
    internal bool IsSingle => !IsEmpty && descriptors.Next is null;
    internal bool IsNormalized => isNormalized;

    public ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService>.For<TDecorator>(descriptors);
        return new(descriptor, isNormalized);
    }

    internal ServiceDecoratorDescriptor<TService> BuildDescriptorChain()
    {
        if (IsEmpty)
            throw AtLeastOneDescriptorRequiredException(typeof(TService));

        if (IsNormalized || IsSingle)
            return descriptors;

        return NormalizeDescriptorChain();
    }

    private ServiceDecoratorDescriptor<TService> NormalizeDescriptorChain()
    {
        ServiceDecoratorDescriptor<TService> descriptorChain = null!;
        var descriptor = descriptors;
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
