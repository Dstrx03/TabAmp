using System;

namespace TabAmp.Shared.Decorator.Extensions;

internal static class ServiceProviderDecoratorExtensions
{
    private static TService DecorateService<TService, TDecorator>(this IServiceProvider serviceProvider, TService service)
        where TDecorator : TService
    {
        return default;
    }
}
