using TabAmp.Engine.Core.FileSerialization;
using TabAmp.Engine.Core.FileSerialization.Common.Context;
using TabAmp.Engine.Core.FileSerialization.Common.Processor;
using TabAmp.Engine.Core.FileSerialization.Common.SerialFileReader;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5;
using TabAmp.Engine.Core.Score;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddEngineCore(this IServiceCollection services)
    {
        services.AddTransient<IFileSerializationService, FileSerializationService>()
            .AddScoped<FileSerializationContext>()
            .AddScoped<ISerialFileReader, PocSerialFileReader>();

        services.AddScoped<IFileDeserializer<Gp5Score>, Gp5FileDeserializer>()
            .AddScoped<Gp5PrimitivesSerialDecoder>()
            .AddScoped<Gp5CompositeTypesSerialDecoder>();

        return services;
    }
}
