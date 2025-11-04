using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Decorator;

namespace TabAmp.Shared.Decorator.DescriptorChain;

public abstract class ServiceDecoratorDescriptor<TService>
    where TService : notnull
{
    internal int? Position { get; private set; }
    internal ServiceDecoratorDescriptor<TService>? Next { get; private set; }

    internal ServiceDecoratorDescriptor<TService> AppendTo(ServiceDecoratorDescriptor<TService> descriptor)
    {
        if (Position is not null)//TODO: check descriptor.Position is not null
            throw new Exception($"TODO {nameof(AppendTo)} {nameof(Position)}:{Position}");

        Position = descriptor?.Position + 1 ?? 0;
        Next = descriptor;

        return this;
    }

    internal void ChainTo(ServiceDecoratorDescriptor<TService> todo2)
    {
        if (Position is null)
            throw new Exception($"TODO {nameof(ChainTo)} {nameof(Position)}:{Position}");

        Next = todo2;
    }

    internal abstract TService DecorateService(IServiceProvider serviceProvider, TService service);

    protected TService DecorateService<TDecorator>(IServiceProvider serviceProvider, TService service)
        where TDecorator : notnull, TService
    {
        return serviceProvider.DecorateService<TService, TDecorator>(service);
    }

    [DebuggerDisplay("TDecorator = {typeof(TDecorator).Name}")]
    internal sealed class For<TDecorator> : ServiceDecoratorDescriptor<TService>
        where TDecorator : notnull, TService
    {
        internal override TService DecorateService(IServiceProvider serviceProvider, TService service) =>
            DecorateService<TDecorator>(serviceProvider, service);
    }
}
