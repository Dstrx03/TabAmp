using System;
using Microsoft.Extensions.DependencyInjection.Decorator;

namespace TabAmp.Shared.Decorator;

public abstract record ServiceDecoratorDescriptorNode<TService>(
    ServiceDecoratorDescriptorNode<TService>? Next,
    string Name)
    where TService : notnull
{
    internal abstract TService DecorateService(TService service, IServiceProvider serviceProvider);

    internal sealed record For<TDecorator>(ServiceDecoratorDescriptorNode<TService>? Next) :
        ServiceDecoratorDescriptorNode<TService>(Next, typeof(TDecorator).Name)
        where TDecorator : notnull, TService
    {
        internal override TService DecorateService(TService service, IServiceProvider serviceProvider) =>
            serviceProvider.DecorateService<TService, TDecorator>(service);
    }
}
