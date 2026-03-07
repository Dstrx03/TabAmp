using System;
using System.Collections.Generic;
using System.Reflection;
using TabAmp.Shared.Decorator.Core.Extensions;

namespace TabAmp.Shared.Decorator.Core.Activators;

internal static class CtorHelper
{
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

    internal static TODO DiscoverAndValidateDecoratorConstructorInfo<TService, TDecorator>(ref List<Exception>? errors, bool stopOnFirstError) where TDecorator : TService
    {
        var e = new Exception("...");
        if (!e.TryAddTo(ref errors, stopOnFirstError))
            return TODO.NotOk(e);

        return TODO.Ok(null);
    }

    internal readonly ref struct TODO // TODO: private
    {
        internal ConstructorInfo ConstructorInfo { get; }
        internal Exception? Error { get; }

        private TODO(ConstructorInfo constructorInfo, Exception? error) =>
            (ConstructorInfo, Error) = (constructorInfo, error);

        internal bool HasError => Error is not null;

        public static TODO Ok(ConstructorInfo constructorInfo) => new(constructorInfo, error: null);
        public static TODO NotOk(Exception? error) => new(constructorInfo: null!, error);

        public void Deconstruct(out ConstructorInfo constructorInfo, out Exception? error) // TODO: not useful?
        {
            constructorInfo = ConstructorInfo;
            error = Error;
        }
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
