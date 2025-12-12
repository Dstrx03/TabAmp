using System;
using TabAmp.Shared.Decorator.Core.DescriptorChain;
using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Shared.Decorator.Fluent;

public readonly ref struct ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation>
    where TService : notnull
    where TImplementation : notnull, TService
{
    private readonly ServiceDecoratorDescriptor<TService> _descriptors;

    private ServiceDecoratorDescriptorChainFluentBuilder(ServiceDecoratorDescriptor<TService> descriptors) =>
        _descriptors = descriptors;

    internal bool IsEmpty => _descriptors is null;

    public ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService>.Node<TDecorator>();
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
        while (descriptor.Next is not null)
        {
            descriptorChain = descriptor.ToDescriptorChainNode(descriptorChain);
            descriptor = descriptor.Next;
        }

        var hasStandaloneImplementationService = true;
        descriptorChain = hasStandaloneImplementationService
            ? descriptor.ToDescriptorChainMetadataNode(descriptorChain)
            : descriptor.ToDescriptorChainNode(descriptorChain);

        return descriptorChain;
    }

    private static InvalidOperationException AtLeastOneDescriptorRequiredException() =>
        new($"Cannot build decorator descriptor chain for the decorated type '{typeof(TService).FullName}'. " +
            "At least one decorator descriptor is required.");
}
