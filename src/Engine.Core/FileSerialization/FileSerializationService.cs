using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TabAmp.Engine.Core.FileSerialization;

internal sealed class FileSerializationService : IFileSerializationService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FileSerializationService(IServiceScopeFactory serviceScopeFactory) =>
        _serviceScopeFactory = serviceScopeFactory;

    public async Task<TFileData> ReadFileAsync<TFileData>(string path, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<FileSerializationContext>();
        var deserializer = scope.ServiceProvider.GetRequiredService<IFileDeserializer>();

        context.Initialize(path, cancellationToken);
        await deserializer.ProcessAsync();

        throw new NotImplementedException();
    }
}
