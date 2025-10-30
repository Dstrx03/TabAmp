using System;
using Microsoft.Extensions.DependencyInjection.Decorator;

namespace TabAmp.Shared.Decorator.DescriptorChain;

internal abstract record ServiceDecoratorDescriptor<TService>(
    ServiceDecoratorDescriptor<TService>? Next,
    string Name)
    where TService : notnull
{
    internal abstract TService DecorateService(IServiceProvider serviceProvider, TService service);

    internal sealed record For<TDecorator>(ServiceDecoratorDescriptor<TService>? Next) :
        ServiceDecoratorDescriptor<TService>(Next, typeof(TDecorator).Name)
        where TDecorator : notnull, TService
    {
        internal override TService DecorateService(IServiceProvider serviceProvider, TService service) =>
            serviceProvider.DecorateService<TService, TDecorator>(service);
    }
}
