using System;

namespace TabAmp.Shared.Decorator.Extensions;

public static class ServiceProviderDecoratorExtensions
{
    public static TService DecorateService<TService, TDecorator>(this IServiceProvider serviceProvider, TService service)
        where TDecorator : TService
    {
        return DecoratorServiceActivator.CreateInstance<TService, TDecorator>(serviceProvider, service);
    }
}
