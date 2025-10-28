using System;

namespace TabAmp.Shared.Decorator.Fluent;

public readonly ref struct ServiceDecoratorDescriptorChainFluentBuilder<TService>(
    ServiceDecoratorDescriptor<TService> descriptors,
    bool isNormalized)
    where TService : notnull
{
    internal bool IsEmpty => descriptors is null;
    internal bool IsSingle => !IsEmpty && descriptors.Next is null;
    internal bool IsNormalized => isNormalized;

    public ServiceDecoratorDescriptorChainFluentBuilder<TService> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService>.For<TDecorator>(descriptors);
        return new(descriptor, isNormalized);
    }

    internal ServiceDecoratorDescriptor<TService> BuildDescriptorChain()
    {
        if (IsEmpty)
            throw AtLeastOneDescriptorRequiredException(typeof(TService));

        return IsNormalized || IsSingle ? descriptors : NormalizeDescriptorChain();
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
