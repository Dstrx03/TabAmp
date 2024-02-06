using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Context;
using TabAmp.Engine.Core.FileSerialization.Common.Components.Processor;

namespace TabAmp.Engine.Core.FileSerialization;

internal sealed class FileSerializationService : IFileSerializationService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FileSerializationService(IServiceScopeFactory serviceScopeFactory) =>
        _serviceScopeFactory = serviceScopeFactory;

    public async Task<T> ReadFileAsync<T>(string filePath, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<FileSerializationContext>();
        context.Foo(filePath, cancellationToken);

        var deserializer = scope.ServiceProvider.GetRequiredService<IFileDeserializer<T>>();
        return await deserializer.DeserializeAsync();
    }
}
