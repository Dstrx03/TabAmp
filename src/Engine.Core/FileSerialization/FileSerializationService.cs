using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Context;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;

namespace TabAmp.Engine.Core.FileSerialization;

internal class FileSerializationService : IFileSerializationService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FileSerializationService(IServiceScopeFactory serviceScopeFactory) =>
        _serviceScopeFactory = serviceScopeFactory;

    public async Task<T> ReadFileAsync<T>(string filePath, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        scope.ServiceProvider.GetRequiredService<ScopedFileSerializationContextContainer>()
            .CreateContext(filePath, cancellationToken);

        var deserializer = scope.ServiceProvider.GetRequiredService<IFileDeserializer<T>>();
        var file = await deserializer.DeserializeAsync();
        if (deserializer is IExactFileDeserializer exactDeserializer)
            exactDeserializer.ValidateExactDeserialization();

        return file;
    }
}
