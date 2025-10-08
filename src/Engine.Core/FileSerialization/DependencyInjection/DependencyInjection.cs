using Microsoft.Extensions.DependencyInjection;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Context;
using TabAmp.Engine.Core.FileSerialization.Common.Components.IO.Serial;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.DependencyInjection;

namespace TabAmp.Engine.Core.FileSerialization.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddFileSerialization(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddService();
        serviceCollection.AddContext();
        serviceCollection.AddSerialFileReader();

        serviceCollection.AddGp5();

        return serviceCollection;
    }

    private static IServiceCollection AddService(this IServiceCollection serviceCollection) =>
        serviceCollection.AddTransient<IFileSerializationService, FileSerializationService>();

    private static IServiceCollection AddContext(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ScopedFileSerializationContextContainer>();
        serviceCollection.AddScoped<FileSerializationContext>(sp =>
            sp.GetRequiredService<ScopedFileSerializationContextContainer>().Context);

        return serviceCollection;
    }

    private static IServiceCollection AddSerialFileReader(this IServiceCollection serviceCollection) =>
        serviceCollection.AddScoped<ISerialFileReader, SerialFileReader>();
}
