using System;
using TabAmp.Shared.Decorator.Core.DescriptorChain;
using TabAmp.Shared.Decorator.Core.Extensions;
using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Shared.Decorator.Fluent;

public readonly ref struct ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation>
    where TService : class
    where TImplementation : class, TService
{
    private readonly ServiceDecoratorDescriptor<TService> _descriptors;

    internal bool IsDisposableContainerAllowed { get; } = false;

    private ServiceDecoratorDescriptorChainFluentBuilder(
        ServiceDecoratorDescriptor<TService> descriptors,
        bool isDisposableContainerAllowed)
    {
        _descriptors = descriptors;
        IsDisposableContainerAllowed = isDisposableContainerAllowed;
    }

    internal bool IsEmpty => _descriptors is null;
    internal bool UseStandaloneImplementationService => //typeof(TImplementation).IsDisposable();

    public ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With<TDecorator>()
        where TDecorator : class, TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService>.For<TDecorator>();
        return new(descriptor.AppendTo(_descriptors), IsDisposableContainerAllowed);
    }

    internal ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> With(
        ServiceDecoratorDescriptor<TService>? descriptor)
    {
        if (descriptor is null)
            return this;

        return new(descriptor.AppendTo(_descriptors), IsDisposableContainerAllowed);
    }

    internal ServiceDecoratorDescriptorChainFluentBuilder<TService, TImplementation> AllowDisposableContainer() =>
        new(_descriptors, isDisposableContainerAllowed: true);

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

        var options = ComposeDescriptorChainOptions();
        descriptorChain = descriptor.ToDescriptorChainNode(descriptorChain, options);

        return descriptorChain;
    }

    private ServiceDecoratorDescriptorChainOptions ComposeDescriptorChainOptions()
    {
        ServiceDecoratorDescriptorChainOptions options = new();

        if (UseStandaloneImplementationService)
            options |= ServiceDecoratorDescriptorChainOptions.UseDefaultImplementationServiceKey;

        if (IsDisposableContainerAllowed)
            options |= ServiceDecoratorDescriptorChainOptions.IsDisposableContainerAllowed;

        return options;
    }

    private static InvalidOperationException AtLeastOneDescriptorRequiredException() =>
        new($"Cannot build decorator descriptor chain for the decorated type '{typeof(TService).FullName}'. " +
            "At least one decorator descriptor is required.");
}
