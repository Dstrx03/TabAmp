using System;
using System.Linq;
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
            //.AddScoped<Gp5BinaryPrimitivesReader>()
            //.AddScoped<IGp5BinaryPrimitivesReader>(x => new Gp5BinaryPrimitivesReaderIntegrityValidator(x.GetRequiredService<Gp5BinaryPrimitivesReader>()))
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

    private static IServiceCollection AddGp5Reader<TService, TImplementation, TIntegrityValidator>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
        where TIntegrityValidator : class, TService
    {
        services.AddScoped<TImplementation>();
        services.AddScoped<TService>(x =>
        {
            TService reader = x.GetRequiredService<TImplementation>();
            //reader = Decorate<TService, TIntegrityValidator>(reader, x);
            reader = Decorate_NoLambdaExpr<TService, TIntegrityValidator>(reader, x);
            return reader;
        });
        return services;
    }

    private static TService Decorate<TService, TDecorator>(TService service, IServiceProvider serviceProvider)
        where TDecorator : TService
    {
        var serviceType = typeof(TService);
        var decoratorType = typeof(TDecorator);

        var constructors = decoratorType.GetConstructors()
            .Where(c => c.GetParameters().Any(p => p.ParameterType == serviceType));

        if (constructors.Count() != 1)
            throw new Exception($"TODO: {decoratorType.Name} multiple ctors");

        var constructor = constructors.Single();

        var parameters = constructor.GetParameters().Select(p =>
            p.ParameterType == serviceType ? service : serviceProvider.GetRequiredService(p.ParameterType)
        ).ToArray();

        return (TService)constructor.Invoke(parameters);
    }

    private static TService Decorate_NoLambdaExpr<TService, TDecorator>(TService service, IServiceProvider serviceProvider)
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
                    throw new Exception($"TODO: {decoratorType.Name} multiple ctors");

                constructorInfo = constructor;
                break;
            }
        }

        if (constructorInfo is null)
            throw new Exception($"TODO: {decoratorType.Name} no ctors");

        var parameterInfo = constructorInfo.GetParameters();
        var parameters = new object[parameterInfo.Length];
        for (var i = 0; i < parameterInfo.Length; i++)
        {
            var parameterType = parameterInfo[i].ParameterType;
            parameters[i] = parameterType == serviceType ? service! : serviceProvider.GetRequiredService(parameterType);
        }

        return (TService)constructorInfo.Invoke(parameters);
    }
}
