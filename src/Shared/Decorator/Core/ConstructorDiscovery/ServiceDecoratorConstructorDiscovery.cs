using System;
using System.Reflection;
using TabAmp.Shared.Fuse;
using TabAmp.Shared.Fuse.Extensions;

namespace TabAmp.Shared.Decorator.Core.ConstructorDiscovery;

internal static class ServiceDecoratorConstructorDiscovery
{
    internal static FuseResult<ConstructorInfo> DiscoverConstructor<TService, TDecorator>(FuseScope scope = default)
        where TDecorator : TService
    {
        var serviceType = typeof(TService);
        var decoratorType = typeof(TDecorator);

        var isConstructorMissing = true;

        ConstructorInfo? constructorInfo = null;
        foreach (var constructor in decoratorType.GetConstructors())
        {
            var serviceTypeParametersCount = 0;

            foreach (var parameter in constructor.GetParameters())
            {
                if (parameter.ParameterType == serviceType)
                    serviceTypeParametersCount++;
            }

            var isConstructorDiscovered = serviceTypeParametersCount > 0;
            var useConstructor = isConstructorDiscovered;

            if (isConstructorDiscovered)
                isConstructorMissing = false;

            if (serviceTypeParametersCount > 1)
            {
                var error = MultipleServiceTypeParametersDecoratorConstructorException(serviceType, constructor);
                if (error.ShouldStop(ref scope)) return scope.ToResult<ConstructorInfo>();
                useConstructor = false;
            }

            if (isConstructorDiscovered && constructorInfo is not null)
            {
                var error = AmbiguousDecoratorConstructorException(constructorInfo, constructor);
                if (error.ShouldStop(ref scope)) return scope.ToResult<ConstructorInfo>();
                useConstructor = false;
            }

            if (useConstructor)
                constructorInfo = constructor;
        }

        if (isConstructorMissing)
        {
            var error = MissingDecoratorConstructorException(serviceType);
            if (error.ShouldStop(ref scope)) return scope.ToResult<ConstructorInfo>();
        }

        return scope.ToResult(constructorInfo!);
    }

    private static InvalidOperationException MultipleServiceTypeParametersDecoratorConstructorException(
        Type serviceType,
        ConstructorInfo constructorInfo) =>
        new($"Constructor has multiple parameters of the decorated type '{serviceType.FullName}':{Environment.NewLine}" +
            $"{constructorInfo}");

    private static InvalidOperationException AmbiguousDecoratorConstructorException(
        ConstructorInfo constructorInfo,
        ConstructorInfo constructorInfoOther) =>
        new($"The following constructors are ambiguous:{Environment.NewLine}" +
            $"{constructorInfo}{Environment.NewLine}" +
            $"{constructorInfoOther}");

    private static InvalidOperationException MissingDecoratorConstructorException(Type serviceType) =>
        new($"Missing constructor with a parameter for the decorated type '{serviceType.FullName}'.");
}
