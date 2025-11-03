using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Decorator;

namespace TabAmp.Shared.Decorator.DescriptorChain;

public abstract class ServiceDecoratorDescriptor<TService>
    where TService : notnull
{
    internal int? TODO_ID_NAME { get; private set; }
    internal ServiceDecoratorDescriptor<TService>? Next { get; private set; }

    internal ServiceDecoratorDescriptor<TService> AppendTo(ServiceDecoratorDescriptor<TService> todo1)
    {
        if (TODO_ID_NAME is not null)
            throw new Exception($"TODO {nameof(AppendTo)} {nameof(TODO_ID_NAME)}:{TODO_ID_NAME}");

        TODO_ID_NAME = todo1?.TODO_ID_NAME + 1 ?? 0;
        Next = todo1;

        return this;
    }

    internal void TODO_METHOD_NAME(ServiceDecoratorDescriptor<TService> todo2)
    {
        if (TODO_ID_NAME is null)
            throw new Exception($"TODO {nameof(TODO_METHOD_NAME)} {nameof(TODO_ID_NAME)}:{TODO_ID_NAME}");

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
