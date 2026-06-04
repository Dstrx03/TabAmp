using System;
using System.Reflection;

namespace TabAmp.Shared.Decorator.Core.ConstructorDiscovery;

internal static class ServiceDecoratorConstructorDiscovery
{
    internal static ConstructorInfo DiscoverConstructor<TService, TDecorator>()
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
