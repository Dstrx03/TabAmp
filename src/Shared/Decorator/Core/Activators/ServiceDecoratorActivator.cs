using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Core.Activators;

internal static class ServiceDecoratorActivator
{
    internal static TService CreateDecorator<TService, TDecorator>(IServiceProvider serviceProvider, TService service)
        where TService : class
        where TDecorator : class, TService
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(service);

        var constructorInfo = ResolveDecoratorConstructorInfo<TService, TDecorator>();
        var parameters = ResolveDecoratorConstructorParameters(serviceProvider, service, constructorInfo);

        return (TService)constructorInfo.Invoke(parameters);
    }

    private static ConstructorInfo ResolveDecoratorConstructorInfo<TService, TDecorator>()
        where TDecorator : TService
    {
        return ServiceDecoratorConstructorDiscovery.DiscoverConstructor<TService, TDecorator>();
    }

    private static object[] ResolveDecoratorConstructorParameters<TService>(
        IServiceProvider serviceProvider,
        TService service,
        ConstructorInfo constructorInfo)
    {
        var serviceType = typeof(TService);

        var parametersInfo = constructorInfo.GetParameters();
        var parameters = new object[parametersInfo.Length];
        for (var i = 0; i < parametersInfo.Length; i++)
        {
            var parameterType = parametersInfo[i].ParameterType;
            parameters[i] = parameterType == serviceType ?
                service! : serviceProvider.GetRequiredService(parameterType);
        }

        return parameters;
    }
}
