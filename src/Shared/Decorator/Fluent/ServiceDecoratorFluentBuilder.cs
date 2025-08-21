using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Shared.Decorator.Fluent;

public interface IServiceDecoratorFluentBuilder<TService>
{
    IServiceDecoratorFluentBuilderSelectLifetimeStage<TService> With<TDecorator>() where TDecorator : notnull, TService;
}

public interface IServiceDecoratorFluentBuilderSelectLifetimeStage<TService> : IServiceDecoratorFluentBuilder<TService>
{
    IServiceCollection Scoped();
}

internal sealed class ServiceDecoratorFluentBuilder<TService, TImplementation> :
    IServiceDecoratorFluentBuilder<TService>,
    IServiceDecoratorFluentBuilderSelectLifetimeStage<TService>
    where TService : class
    where TImplementation : class, TService
{
    private readonly IServiceCollection _serviceCollection;
    private readonly List<IServiceDecoratorDescriptor<TService>> _descriptors = [];

    public ServiceDecoratorFluentBuilder(IServiceCollection serviceCollection) =>
        _serviceCollection = serviceCollection;

    public IServiceDecoratorFluentBuilderSelectLifetimeStage<TService> With<TDecorator>()
        where TDecorator : notnull, TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService, TDecorator>();
        _descriptors.Add(descriptor);
        return this;
    }

    public IServiceCollection Scoped()
    {
        var head = CreateDescriptorLinkedList();//TODO: name

        _serviceCollection.AddScoped<TImplementation>();
        _serviceCollection.AddScoped<TService>(serviceProvider => CreateDecoratedService(serviceProvider, head));

        return _serviceCollection;
    }

    private IServiceDecoratorDescriptorNode<TService> CreateDescriptorLinkedList()
    {
        if (_descriptors.Count == 0)
            throw new InvalidOperationException();// TODO: message

        var node = (IServiceDecoratorDescriptorNode<TService>)null!;
        for (var i = _descriptors.Count - 1; i >= 0; i--)
            node = _descriptors[i].ToNode(node);

        _descriptors.Clear();

        return node;
    }

    private static TService CreateDecoratedService(//TODO: name
        IServiceProvider serviceProvider,
        IServiceDecoratorDescriptorNode<TService> head)//TODO: name
    {
        TService service = serviceProvider.GetRequiredService<TImplementation>();

        var descriptor = head;
        while (descriptor is not null)
        {
            service = descriptor.DecorateService(service, serviceProvider);
            descriptor = descriptor.Next;
        }

        return service;
    }
}
