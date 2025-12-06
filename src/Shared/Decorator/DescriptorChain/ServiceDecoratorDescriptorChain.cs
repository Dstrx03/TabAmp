using System;
using Microsoft.Extensions.DependencyInjection.Decorator;

namespace TabAmp.Shared.Decorator.DescriptorChain;

internal abstract class ServiceDecoratorDescriptorChain<TService>
    where TService : notnull
{
    internal ServiceDecoratorDescriptorChain<TService>? Next { get; }
    internal bool TODO_NAME { get; } = true;

    private ServiceDecoratorDescriptorChain(ServiceDecoratorDescriptorChain<TService>? next) =>
        Next = next;

    internal abstract TService DecorateService(IServiceProvider serviceProvider, TService service);

    internal sealed class For<TDecorator>(ServiceDecoratorDescriptorChain<TService>? next) :
        ServiceDecoratorDescriptorChain<TService>(next)
        where TDecorator : notnull, TService
    {
        internal override TService DecorateService(IServiceProvider serviceProvider, TService service) =>
            serviceProvider.DecorateService<TService, TDecorator>(service);
    }
}
