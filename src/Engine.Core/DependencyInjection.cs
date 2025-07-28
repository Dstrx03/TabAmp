using System;
using System.Reflection;
using TabAmp.Engine.Core.FileSerialization;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Context;
using TabAmp.Engine.Core.FileSerialization.Common.Components.IO.Serial;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.BinaryPrimitives;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.DocumentComponents;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Effects;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Measures;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.MusicalNotation;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Rse;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Text;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Tracks;
using TabAmp.Engine.Core.Score;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddEngineCore(this IServiceCollection services)
    {
        services.AddTransient<IFileSerializationService, FileSerializationService>()
            .AddFileSerializationContext()
            .AddScoped<ISerialFileReader, SerialFileReader>();

        services.AddScoped<IFileDeserializer<Gp5Score>, Gp5FileDeserializer>()
            .AddGp5Reader<IGp5BinaryPrimitivesReader, Gp5BinaryPrimitivesReader, Gp5BinaryPrimitivesReaderIntegrityValidator>()
            .AddScoped<Gp5TextReader>()
            .AddScoped<IGp5TextReader>(x => new Gp5TextReaderIntegrityValidator(x.GetRequiredService<Gp5TextReader>()))
            .AddScoped<Gp5DocumentComponentsReader>()
            .AddScoped<IGp5DocumentComponentsReader>(x => new Gp5DocumentComponentsReaderIntegrityValidator(x.GetRequiredService<Gp5DocumentComponentsReader>()))
            .AddScoped<Gp5MusicalNotationReader>()
            .AddScoped<IGp5MusicalNotationReader>(x => new Gp5MusicalNotationReaderIntegrityValidator(x.GetRequiredService<Gp5MusicalNotationReader>()))
            .AddScoped<Gp5TracksReader>()
            .AddScoped<IGp5TracksReader>(x => new Gp5TracksReaderIntegrityValidator(x.GetRequiredService<Gp5TracksReader>()))
            .AddScoped<Gp5MeasuresReader>()
            .AddScoped<IGp5MeasuresReader>(x => new Gp5MeasuresReaderIntegrityValidator(x.GetRequiredService<Gp5MeasuresReader>()))
            .AddScoped<Gp5EffectsReader>()
            .AddScoped<IGp5EffectsReader>(x => new Gp5EffectsReaderIntegrityValidator(x.GetRequiredService<Gp5EffectsReader>()))
            .AddScoped<Gp5RseReader>()
            .AddScoped<IGp5RseReader>(x => new Gp5RseReaderIntegrityValidator(x.GetRequiredService<Gp5RseReader>()));

        return services;
    }

    private static IServiceCollection AddFileSerializationContext(this IServiceCollection services) =>
        services.AddScoped<ScopedFileSerializationContextContainer>()
            .AddScoped<FileSerializationContext>(x => x.GetRequiredService<ScopedFileSerializationContextContainer>().Context);

    private static IServiceCollection AddGp5Reader<TService, TReader, TIntegrityValidator>(this IServiceCollection services)
        where TService : class
        where TReader : class, TService
        where TIntegrityValidator : class, TService
    {
        services.AddScoped<TReader>();
        services.AddScoped<TService>(x =>
        {
            TService reader = x.GetRequiredService<TReader>();
            reader = x.DecorateService<TService, TIntegrityValidator>(reader);
            return reader;
        });
        return services;
    }

    private static TService DecorateService<TService, TDecorator>(this IServiceProvider serviceProvider, TService service)
        where TDecorator : TService
    {
        var constructorInfo = DiscoverDecoratorConstructorInfo<TService, TDecorator>();
        var parameters = ResolveDecoratorParameters(service, constructorInfo, serviceProvider);

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

                if (constructorInfo is not null)
                    throw AmbiguousDecoratorConstructorException(decoratorType, constructorInfo, constructor);

                constructorInfo = constructor;
                break;
            }
        }

        return constructorInfo ?? throw MissingDecoratorConstructorException(decoratorType, serviceType);
    }

    private static object[] ResolveDecoratorParameters<TService>(
        TService service,
        ConstructorInfo constructorInfo,
        IServiceProvider serviceProvider)
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

    private static InvalidOperationException AmbiguousDecoratorConstructorException(
        Type decoratorType,
        ConstructorInfo constructorInfo,
        ConstructorInfo constructorInfoOther) =>
        new($"Unable to activate decorator type '{decoratorType.FullName}'. " +
            $"The following constructors are ambiguous:{Environment.NewLine}" +
            $"{constructorInfo}{Environment.NewLine}" +
            $"{constructorInfoOther}");

    private static InvalidOperationException MissingDecoratorConstructorException(Type decoratorType, Type serviceType) =>
        new($"Unable to activate decorator type '{decoratorType.FullName}'. " +
            $"Missing constructor with a parameter for the decorated type '{serviceType.FullName}'.");
}
