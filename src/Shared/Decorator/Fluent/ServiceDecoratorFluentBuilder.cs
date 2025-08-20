using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

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
    private List<IDescriptor<TService>>? _descriptors;

    public ServiceDecoratorFluentBuilder(IServiceCollection serviceCollection) =>
        _serviceCollection = serviceCollection;

    public IServiceDecoratorFluentBuilderSelectLifetimeStage<TService> With<TDecorator>()
        where TDecorator : TService
    {
        var descriptor = new Descriptor<TService, TDecorator>();// TODO: generic constraints

        _descriptors ??= [];
        _descriptors.Add(descriptor);

        return this;
    }

    public IServiceCollection Scoped()
    {
        throw new System.NotImplementedException();
        return _serviceCollection;
    }
}
