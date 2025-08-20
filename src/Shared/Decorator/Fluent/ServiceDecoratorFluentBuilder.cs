using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Fluent;

internal interface IServiceDecoratorFluentBuilder<TService>// TODO: name
{
    IServiceDecoratorFluentBuilder<TService> With<TDecorator>() where TDecorator : TService;
}

internal interface IServiceDecoratorFluentBuilderTODOStage<TService> : IServiceDecoratorFluentBuilder<TService>
{
    IServiceCollection Scoped();
}

internal sealed class ServiceDecoratorFluentBuilder<TService, TImplementation> : IServiceDecoratorFluentBuilderTODOStage<TService>
{
    private readonly IServiceCollection _serviceCollection;
    private List<IDescriptor<TService>>? _descriptors;

    public ServiceDecoratorFluentBuilder(IServiceCollection serviceCollection) =>
        _serviceCollection = serviceCollection;

    public IServiceDecoratorFluentBuilder<TService> With<TDecorator>()
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
