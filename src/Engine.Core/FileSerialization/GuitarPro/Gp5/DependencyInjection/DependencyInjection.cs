using Microsoft.Extensions.DependencyInjection;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;
using TabAmp.Engine.Core.FileSerialization.Common.DependencyInjection.Reader;
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

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.DependencyInjection;

internal static class DependencyInjection
{
    public static IServiceCollection AddGp5(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IFileDeserializer<Gp5Score>, Gp5FileDeserializer>();

        serviceCollection.AddReader(ReaderOptions.For<IGp5BinaryPrimitivesReader, Gp5BinaryPrimitivesReader>()
            .WithIntegrityValidator<Gp5BinaryPrimitivesReaderIntegrityValidator>());
        serviceCollection.AddReader(ReaderOptions.For<IGp5TextReader, Gp5TextReader>()
            .WithIntegrityValidator<Gp5TextReaderIntegrityValidator>());
        serviceCollection.AddReader(ReaderOptions.For<IGp5DocumentComponentsReader, Gp5DocumentComponentsReader>()
            .WithIntegrityValidator<Gp5DocumentComponentsReaderIntegrityValidator>());
        serviceCollection.AddReader(ReaderOptions.For<IGp5MusicalNotationReader, Gp5MusicalNotationReader>()
            .WithIntegrityValidator<Gp5MusicalNotationReaderIntegrityValidator>());
        serviceCollection.AddReader(ReaderOptions.For<IGp5TracksReader, Gp5TracksReader>()
            .WithIntegrityValidator<Gp5TracksReaderIntegrityValidator>());
        serviceCollection.AddReader(ReaderOptions.For<IGp5MeasuresReader, Gp5MeasuresReader>()
            .WithIntegrityValidator<Gp5MeasuresReaderIntegrityValidator>());
        serviceCollection.AddReader(ReaderOptions.For<IGp5EffectsReader, Gp5EffectsReader>()
            .WithIntegrityValidator<Gp5EffectsReaderIntegrityValidator>());
        serviceCollection.AddReader(ReaderOptions.For<IGp5RseReader, Gp5RseReader>()
            .WithIntegrityValidator<Gp5RseReaderIntegrityValidator>());

        return serviceCollection;
    }

    private static IServiceCollection AddGp5Reader<TService, TReader, TIntegrityValidator>(
        this IServiceCollection serviceCollection)
        where TService : class
        where TReader : class, TService
        where TIntegrityValidator : class, TService
    {
        //serviceCollection.AddDecoratedScoped<TService, TReader>(builder => builder.With<TIntegrityValidator>());
        //serviceCollection.AddDecoratedScoped(ConfigureDescriptorChain.For<TService, TReader>().With<TIntegrityValidator>());
        //serviceCollection.AddDecorated(ConfigureDescriptorChain.For<TService, TReader>().With<TIntegrityValidator>().BuildDescriptorChain(), ServiceLifetime.Scoped);

        // *** TODO: AddReader API prototype ***

        serviceCollection.AddReader(new ReaderOptions<IGp5BinaryPrimitivesReader, Gp5BinaryPrimitivesReader>
        {
            IntegrityValidator = new IIntegrityValidatorDescriptor<IGp5BinaryPrimitivesReader>.For<Gp5BinaryPrimitivesReaderIntegrityValidator>()
        });

        serviceCollection.AddReader(ReaderOptions.For<IGp5BinaryPrimitivesReader, Gp5BinaryPrimitivesReader>()
            .WithIntegrityValidator<Gp5BinaryPrimitivesReaderIntegrityValidator>());

        // *** TODO: AddReader API prototype *** 

        return serviceCollection;
    }
}
