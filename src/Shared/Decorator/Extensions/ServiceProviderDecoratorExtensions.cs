using System;
using TabAmp.Shared.Decorator;

namespace Microsoft.Extensions.DependencyInjection.Decorator;

public static class ServiceProviderDecoratorExtensions
{
    public static TService DecorateService<TService, TDecorator>(this IServiceProvider serviceProvider, TService service)
        where TDecorator : TService
    {
        return DecoratorServiceActivator.CreateInstance<TService, TDecorator>(serviceProvider, service);
    }
}
