using TabAmp.Engine.Core.FileSerialization;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Context;
using TabAmp.Engine.Core.FileSerialization.Common.Components.IO.Serial;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.IntegrityValidators;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Readers;
using TabAmp.Engine.Core.Score;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddEngineCore(this IServiceCollection services)
    {
        services.AddTransient<IFileSerializationService, FileSerializationService>()
            .AddFileSerializationContext()
            .AddScoped<ISerialFileReader, PocSerialFileReader>();

        services.AddScoped<IFileDeserializer<Gp5Score>, Gp5FileDeserializer>()
            .AddScoped<Gp5BinaryPrimitivesReader>()
            .AddScoped<IGp5BinaryPrimitivesReader>(x => new Gp5BinaryPrimitivesReaderIntegrityValidator(x.GetRequiredService<Gp5BinaryPrimitivesReader>()))
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
        services.AddScoped<FileSerializationContextBuilder>()
            .AddScoped<FileSerializationContext>(x => x.GetRequiredService<FileSerializationContextBuilder>().GetConstructedContext());
}
