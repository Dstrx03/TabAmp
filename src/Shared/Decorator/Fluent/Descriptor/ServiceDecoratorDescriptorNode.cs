using System;
using Microsoft.Extensions.DependencyInjection.Decorator;

namespace TabAmp.Shared.Decorator.Fluent.Descriptor;

internal interface IServiceDecoratorDescriptorNode<TService>
{
    IServiceDecoratorDescriptorNode<TService>? Next { get; }
    TService DecorateService(TService service, IServiceProvider serviceProvider);
}

internal sealed record ServiceDecoratorDescriptorNode<TService, TDecorator>(IServiceDecoratorDescriptorNode<TService>? Next) :
    IServiceDecoratorDescriptorNode<TService>
    where TService : notnull
    where TDecorator : notnull, TService
{
    public TService DecorateService(TService service, IServiceProvider serviceProvider) =>
        serviceProvider.DecorateService<TService, TDecorator>(service);
}
