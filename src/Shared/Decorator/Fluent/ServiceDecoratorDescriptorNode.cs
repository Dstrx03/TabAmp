using System;
using Microsoft.Extensions.DependencyInjection.Decorator;

namespace TabAmp.Shared.Decorator.Fluent;

public abstract record ServiceDecoratorDescriptorNode<TService>(ServiceDecoratorDescriptorNode<TService>? Next, object _placeholder)
    where TService : notnull
{
    internal abstract TService DecorateService(TService service, IServiceProvider serviceProvider);

    internal sealed record Instance<TDecorator>(ServiceDecoratorDescriptorNode<TService>? Next) :
        ServiceDecoratorDescriptorNode<TService>(Next, null)
        where TDecorator : notnull, TService
    {
        internal override TService DecorateService(TService service, IServiceProvider serviceProvider) =>
            serviceProvider.DecorateService<TService, TDecorator>(service);
    }
}
