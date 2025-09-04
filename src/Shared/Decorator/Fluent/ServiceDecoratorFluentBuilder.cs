using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Decorator;

namespace TabAmp.Shared.Decorator.Fluent;

public interface IServiceDecoratorFluentBuilder<TService>
{
    IServiceDecoratorFluentBuilderSelectLifetimeStage<TService> With<TDecorator>() where TDecorator : notnull, TService;
}

public interface IServiceDecoratorFluentBuilderSelectLifetimeStage<TService> : IServiceDecoratorFluentBuilder<TService>
{
    IServiceCollection Scoped();
}

internal sealed class ServiceDecoratorFluentBuilder<TService, TImplementation>(IServiceCollection serviceCollection) :
    IServiceDecoratorFluentBuilder<TService>,
    IServiceDecoratorFluentBuilderSelectLifetimeStage<TService>
    where TService : class
    where TImplementation : class, TService
{
    private List<IDescriptor> _descriptors = [];

    public IServiceDecoratorFluentBuilderSelectLifetimeStage<TService> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        var descriptor = new Descriptor<TDecorator>();
        _descriptors.Add(descriptor);
        return this;
    }

    public IServiceCollection Scoped()
    {
        var descriptorChain = BuildDescriptorChain();

        serviceCollection.AddScoped<TService>(serviceProvider =>
            ComposeDecoratedService(serviceProvider, descriptorChain));

        return serviceCollection;
    }

    private IDescriptorNode BuildDescriptorChain()
    {
        if (_descriptors is null)
            throw TodoException();

        IDescriptorNode node = null!;
        for (var i = _descriptors.Count - 1; i >= 0; i--)
            node = _descriptors[i].ToNode(node);

        _descriptors.Clear();
        _descriptors = null!;

        return node;
    }

    private static TService ComposeDecoratedService(
        IServiceProvider serviceProvider,
        IDescriptorNode descriptorChain)
    {
        TService service = ActivatorUtilities.CreateInstance<TImplementation>(serviceProvider);

        var descriptor = descriptorChain;
        while (descriptor is not null)
        {
            service = descriptor.DecorateService(service, serviceProvider);
            descriptor = descriptor.Next;
        }

        return service;
    }

    private static InvalidOperationException TodoException() =>
        new($"TODO: message");

    private interface IDescriptor
    {
        IDescriptorNode ToNode(IDescriptorNode? next);
    }

    private interface IDescriptorNode
    {
        IDescriptorNode? Next { get; }
        TService DecorateService(TService service, IServiceProvider serviceProvider);
    }

    private record Descriptor<TDecorator> : IDescriptor
        where TDecorator : notnull, TService
    {
        public IDescriptorNode ToNode(IDescriptorNode? next) =>
            new DescriptorNode<TDecorator>(next);
    }

    private record DescriptorNode<TDecorator>(IDescriptorNode? Next) : IDescriptorNode
        where TDecorator : notnull, TService
    {
        public TService DecorateService(TService service, IServiceProvider serviceProvider) =>
            serviceProvider.DecorateService<TService, TDecorator>(service);
    }
}
