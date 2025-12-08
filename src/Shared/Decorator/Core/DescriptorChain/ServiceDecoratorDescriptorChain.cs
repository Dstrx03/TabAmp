using System;
using TabAmp.Shared.Decorator.Core.Activators;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

internal abstract class ServiceDecoratorDescriptorChain<TService>
    where TService : notnull
{
    internal ServiceDecoratorDescriptorChain<TService>? Next { get; }
    internal bool TODO_NAME { get; } = true;

    private ServiceDecoratorDescriptorChain(ServiceDecoratorDescriptorChain<TService>? next) =>
        Next = next;

    internal abstract TService CreateDecorator(IServiceProvider serviceProvider, TService service);

    internal sealed class For<TDecorator>(ServiceDecoratorDescriptorChain<TService>? next) :
        ServiceDecoratorDescriptorChain<TService>(next)
        where TDecorator : notnull, TService
    {
        internal override TService CreateDecorator(IServiceProvider serviceProvider, TService service) =>
            ServiceDecoratorActivator.CreateDecorator<TService, TDecorator>(serviceProvider, service);
    }
}
