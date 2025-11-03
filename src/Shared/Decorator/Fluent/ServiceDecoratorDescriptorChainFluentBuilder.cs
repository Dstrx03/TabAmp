using System;
using TabAmp.Shared.Decorator.DescriptorChain;

namespace TabAmp.Shared.Decorator.Fluent;

public readonly ref struct ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation>(
    ServiceDecoratorDescriptor<TService> descriptors)
    where TService : notnull
    where TImplementation : notnull, TService
{
    internal bool IsEmpty => descriptors is null;

    public ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService>.For<TDecorator>();
        return new(descriptor.AppendTo(descriptors));
    }

    public ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With(
        ServiceDecoratorDescriptor<TService>? descriptor)
    {
        if (descriptor is null)
            return this;

        return new(descriptor.AppendTo(descriptors));
    }

    internal ServiceDecoratorDescriptor<TService> BuildDescriptorChain()
    {
        if (IsEmpty)
            throw AtLeastOneDescriptorRequiredException(typeof(TService));

        ServiceDecoratorDescriptor<TService> descriptorChain = null!;
        var descriptor = descriptors;
        while (descriptor is not null)
        {
            var next = descriptor.Next;
            descriptor.TODO_METHOD_NAME(descriptorChain);//descriptor.Next = descriptorChain;
            descriptorChain = descriptor;
            descriptor = next;
        }

        return descriptorChain;
    }

    private static InvalidOperationException AtLeastOneDescriptorRequiredException(Type serviceType) =>
        new($"Cannot build decorator descriptor chain for the decorated type '{serviceType.FullName}'. " +
            "At least one decorator descriptor is required.");
}
