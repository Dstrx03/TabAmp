using TabAmp.Engine.Core.FileSerialization;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Context;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;
using TabAmp.Engine.Core.FileSerialization.Common.Components.SerialFileReader;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;
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
            .AddScoped<IGp5StringsReader, Gp5StringsReader>()
            .AddScoped<IGp5TodoReader, Gp5TodoReader>()
            .AddScoped<IGp5RseEqualizerReader, Gp5RseEqualizerReader>()
            .AddScoped<IGp5ColorReader, Gp5ColorReader>();

        return services;
    }

    private static IServiceCollection AddFileSerializationContext(this IServiceCollection services) =>
        services.AddScoped<FileSerializationContextBuilder>()
            .AddScoped<FileSerializationContext>(x => x.GetRequiredService<FileSerializationContextBuilder>().GetConstructedContext());
}
