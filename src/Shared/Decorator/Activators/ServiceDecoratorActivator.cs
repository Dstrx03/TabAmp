using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.Shared.Decorator.Activators;

internal static class ServiceDecoratorActivator
{
    internal static TService CreateDecorator<TService, TDecorator>(IServiceProvider serviceProvider, TService service)
        where TService : notnull
        where TDecorator : notnull, TService
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(service);

        var constructorInfo = DiscoverDecoratorConstructorInfo<TService, TDecorator>();
        var parameters = ResolveDecoratorParameters(serviceProvider, service, constructorInfo);

        return (TService)constructorInfo.Invoke(parameters);
    }

    private static ConstructorInfo DiscoverDecoratorConstructorInfo<TService, TDecorator>()
        where TDecorator : TService
    {
        var serviceType = typeof(TService);
        var decoratorType = typeof(TDecorator);

        ConstructorInfo? constructorInfo = null;
        foreach (var constructor in decoratorType.GetConstructors())
        {
            foreach (var parameter in constructor.GetParameters())
            {
                if (parameter.ParameterType != serviceType)
                    continue;

                if (constructorInfo == constructor)
                    throw MultipleServiceTypeParametersDecoratorConstructorException(serviceType, decoratorType, constructorInfo);
                else if (constructorInfo is not null)
                    throw AmbiguousDecoratorConstructorException(decoratorType, constructorInfo, constructor);

                constructorInfo = constructor;
            }
        }

        return constructorInfo ?? throw MissingDecoratorConstructorException(serviceType, decoratorType);
    }

    private static object[] ResolveDecoratorParameters<TService>(
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

    private static InvalidOperationException MultipleServiceTypeParametersDecoratorConstructorException(
        Type serviceType,
        Type decoratorType,
        ConstructorInfo constructorInfo) =>
        new($"Unable to activate decorator type '{decoratorType.FullName}'. " +
            $"Constructor has multiple parameters of the decorated type '{serviceType.FullName}':{Environment.NewLine}" +
            $"{constructorInfo}");

    private static InvalidOperationException AmbiguousDecoratorConstructorException(
        Type decoratorType,
        ConstructorInfo constructorInfo,
        ConstructorInfo constructorInfoOther) =>
        new($"Unable to activate decorator type '{decoratorType.FullName}'. " +
            $"The following constructors are ambiguous:{Environment.NewLine}" +
            $"{constructorInfo}{Environment.NewLine}" +
            $"{constructorInfoOther}");

    private static InvalidOperationException MissingDecoratorConstructorException(Type serviceType, Type decoratorType) =>
        new($"Unable to activate decorator type '{decoratorType.FullName}'. " +
            $"Missing constructor with a parameter for the decorated type '{serviceType.FullName}'.");
}
