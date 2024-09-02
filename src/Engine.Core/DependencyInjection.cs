using TabAmp.Engine.Core.FileSerialization;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Context;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;
using TabAmp.Engine.Core.FileSerialization.Common.Components.SerialFileReader;
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
            .AddScoped<Gp5StringsReader>()
            .AddScoped<Gp5TodoReader>()
            .AddScoped<IGp5BinaryPrimitivesReader>(x => new Gp5BinaryPrimitivesReaderIntegrityValidator(x.GetRequiredService<Gp5BinaryPrimitivesReader>()))
            .AddScoped<IGp5StringsReader>(x => new Gp5StringsReaderIntegrityValidator(x.GetRequiredService<Gp5StringsReader>()))
            .AddScoped<IGp5TodoReader>(x => new Gp5TodoReaderIntegrityValidator(x.GetRequiredService<Gp5TodoReader>()))
            .AddScoped<IGp5RseEqualizerReader, Gp5RseEqualizerReader>();

        return services;
    }

    private static IServiceCollection AddFileSerializationContext(this IServiceCollection services) =>
        services.AddScoped<FileSerializationContextBuilder>()
            .AddScoped<FileSerializationContext>(x => x.GetRequiredService<FileSerializationContextBuilder>().GetConstructedContext());
}
