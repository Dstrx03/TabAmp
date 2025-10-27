using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Decorator;
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
using TabAmp.Shared.Decorator.Fluent;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.DependencyInjection;

internal static class DependencyInjection
{
    public static IServiceCollection AddGp5(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IFileDeserializer<Gp5Score>, Gp5FileDeserializer>();

        serviceCollection.AddGp5Reader<IGp5BinaryPrimitivesReader, Gp5BinaryPrimitivesReader, Gp5BinaryPrimitivesReaderIntegrityValidator>();

        // TODO: AddReader API prototype

        serviceCollection.AddReader(new Config<IGp5BinaryPrimitivesReader, Gp5BinaryPrimitivesReader>
        {
            IntegrityValidator = new IntegrityValidatorDescriptor<IGp5BinaryPrimitivesReader>
                .For<Gp5BinaryPrimitivesReaderIntegrityValidator>()
        });

        serviceCollection.AddReader(Config.For<IGp5BinaryPrimitivesReader, Gp5BinaryPrimitivesReader>()
            .WithIntegrityValidator<Gp5BinaryPrimitivesReaderIntegrityValidator>());

        // TODO: AddReader API prototype

        serviceCollection.AddScoped<Gp5TextReader>()
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

        return serviceCollection;
    }

    private static IServiceCollection AddGp5Reader<TService, TReader, TIntegrityValidator>(
        this IServiceCollection serviceCollection)
        where TService : class
        where TReader : notnull, TService
        where TIntegrityValidator : notnull, TService
    {
        return serviceCollection.AddDecoratedScoped<TService, TReader>(builder => builder.With<TIntegrityValidator>());
    }

    private static IServiceCollection AddReader<TService, TReader>(
        this IServiceCollection serviceCollection,
        Config<TService, TReader> config)
        where TService : class
        where TReader : notnull, TService
    {
        var integrityValidator = config.IntegrityValidator;

        var builder = new ServiceDecoratorDescriptorChainFluentBuilder<TService>();

        integrityValidator?.Apply(builder, out builder);

        return null;
    }

    private static class Config
    {
        public static Config<TService, TReader> For<TService, TReader>()
            where TService : notnull
            where TReader : notnull, TService => new();
    }

    private readonly ref struct Config<TService, TReader>
        where TService : notnull
        where TReader : notnull, TService
    {
        public readonly IntegrityValidatorDescriptor<TService>? IntegrityValidator { get; init; }

        public Config<TService, TReader> WithIntegrityValidator<TIntegrityValidator>()
            where TIntegrityValidator : notnull, TService
        {
            return new()
            {
                IntegrityValidator = new IntegrityValidatorDescriptor<TService>.For<TIntegrityValidator>()
            };
        }
    }

    private abstract record IntegrityValidatorDescriptor<TService>
        where TService : notnull
    {
        public abstract void Apply(ServiceDecoratorDescriptorChainFluentBuilder<TService> builder,
            out ServiceDecoratorDescriptorChainFluentBuilder<TService> builder2);

        public sealed record For<TIntegrityValidator> : IntegrityValidatorDescriptor<TService>
            where TIntegrityValidator : notnull, TService
        {
            public override void Apply(ServiceDecoratorDescriptorChainFluentBuilder<TService> builder,
                out ServiceDecoratorDescriptorChainFluentBuilder<TService> builder2)
            {
                builder2 = builder.With<TIntegrityValidator>();
            }
        }
    }
}
