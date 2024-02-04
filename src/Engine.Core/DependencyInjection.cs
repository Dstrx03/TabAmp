using TabAmp.Engine.Core.FileSerialization;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddEngineCore(this IServiceCollection services)
    {
        services.AddTransient<IFileSerializationService, FileSerializationService>()
            .AddScoped<FileSerializationContext>()
            .AddScoped<IFileDeserializer<object>, AFileDeserializer>();

        return services;
    }
}
