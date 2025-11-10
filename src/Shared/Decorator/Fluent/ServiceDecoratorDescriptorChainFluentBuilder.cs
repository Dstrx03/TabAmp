using System;
using TabAmp.Shared.Decorator.DescriptorChain;

namespace TabAmp.Shared.Decorator.Fluent;

public readonly ref struct ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation>
    where TService : notnull
    where TImplementation : notnull, TService
{
    private readonly ServiceDecoratorDescriptor<TService> _descriptors;

    internal bool IsEmpty => _descriptors is null;
    internal int Count => _descriptors?.Position ?? 0;

    private ServiceDecoratorDescriptorChainFluentBuilder(ServiceDecoratorDescriptor<TService> descriptors) =>
        _descriptors = descriptors;

    public ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService>.For<TDecorator>();
        return new(descriptor.AppendTo(_descriptors));
    }

    internal ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With(
        ServiceDecoratorDescriptor<TService>? descriptor)
    {
        if (descriptor is null)
            return this;

        return new(descriptor.AppendTo(_descriptors));
    }

    internal ServiceDecoratorDescriptorChain<TService> BuildDescriptorChain()
    {
        if (IsEmpty)
            throw AtLeastOneDescriptorRequiredException();

        ServiceDecoratorDescriptorChain<TService> descriptorChain = null!;
        var descriptor = _descriptors;
        while (descriptor is not null)
        {
            descriptorChain = descriptor.ToChain(descriptorChain);
            descriptor = descriptor.Next;
        }

        return descriptorChain;
    }

    private static InvalidOperationException AtLeastOneDescriptorRequiredException() =>
        new($"Cannot build decorator descriptor chain for the decorated type '{typeof(TService).FullName}'. " +
            "At least one decorator descriptor is required.");
}
