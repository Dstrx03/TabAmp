using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Decorator;

namespace TabAmp.Shared.Decorator.DescriptorChain;

public abstract class ServiceDecoratorDescriptor<TService>(ServiceDecoratorDescriptor<TService>? next)
    where TService : notnull
{
    internal ServiceDecoratorDescriptor<TService>? Next { get; set; } = next;

    internal abstract TService DecorateService(IServiceProvider serviceProvider, TService service);

    protected TService DecorateService<TDecorator>(IServiceProvider serviceProvider, TService service)
        where TDecorator : notnull, TService
    {
        return serviceProvider.DecorateService<TService, TDecorator>(service);
    }

    [DebuggerDisplay("TDecorator = {typeof(TDecorator).Name}")]
    internal sealed class For<TDecorator>(ServiceDecoratorDescriptor<TService>? next) :
        ServiceDecoratorDescriptor<TService>(next)
        where TDecorator : notnull, TService
    {
        internal override TService DecorateService(IServiceProvider serviceProvider, TService service) =>
            DecorateService<TDecorator>(serviceProvider, service);
    }
}
