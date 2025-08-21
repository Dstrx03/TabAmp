using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using TabAmp.Shared.Decorator.Fluent.Descriptor;

namespace TabAmp.Shared.Decorator.Fluent;

internal interface IServiceDecoratorFluentBuilder<TService>
{
    IServiceDecoratorFluentBuilderSelectLifetimeStage<TService> With<TDecorator>() where TDecorator : TService;
}

internal interface IServiceDecoratorFluentBuilderSelectLifetimeStage<TService> : IServiceDecoratorFluentBuilder<TService>
{
    IServiceCollection Scoped();
}

internal sealed class ServiceDecoratorFluentBuilder<TService, TImplementation> :
    IServiceDecoratorFluentBuilder<TService>,
    IServiceDecoratorFluentBuilderSelectLifetimeStage<TService>
{
    private readonly IServiceCollection _serviceCollection;
    private List<IServiceDecoratorDescriptor<TService>>? _descriptors;

    public ServiceDecoratorFluentBuilder(IServiceCollection serviceCollection) =>
        _serviceCollection = serviceCollection;

    public IServiceDecoratorFluentBuilderSelectLifetimeStage<TService> With<TDecorator>()
        where TDecorator : TService
    {
        var descriptor = new ServiceDecoratorDescriptor<TService, TDecorator>(); // TODO: generic constraints

        _descriptors ??= [];
        _descriptors.Add(descriptor);

        return this;
    }

    public IServiceCollection Scoped()
    {
        var descriptor = Todo_name();
        throw new System.NotImplementedException();
        return _serviceCollection;
    }

    private IServiceDecoratorDescriptorNode<TService> Todo_name()//TODO: name, nullable list
    {
        var node = (IServiceDecoratorDescriptorNode<TService>)null!;

        for (var i = _descriptors.Count - 1; i >= 0; i--)
            node = _descriptors[i].ToNode(node);

        return node;
    }
}
