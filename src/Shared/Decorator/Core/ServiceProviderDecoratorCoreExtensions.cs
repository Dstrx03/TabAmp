using System;
using TabAmp.Shared.Decorator.Core.Activators;

namespace TabAmp.Shared.Decorator.Core;

public static class ServiceProviderDecoratorCoreExtensions
{
    public static TService DecorateService<TService, TDecorator>(this IServiceProvider serviceProvider, TService service)
        where TService : notnull
        where TDecorator : notnull, TService
    {
        return ServiceDecoratorActivator.CreateDecorator<TService, TDecorator>(serviceProvider, service);
    }
}
