using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using TabAmp.Engine.Core.Score;

namespace TabAmp.Engine.Core.FileSerialization;

internal sealed class FileSerializationService : IFileSerializationService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public FileSerializationService(IServiceScopeFactory serviceScopeFactory) =>
        _serviceScopeFactory = serviceScopeFactory;

    public async Task<Gp5Score> ReadFileAsync(string path, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<FileSerializationContext>();
        context.FilePath = path;
        context.CancellationToken = cancellationToken;

        var deserializer = scope.ServiceProvider.GetRequiredService<IFileDeserializer>();
        await deserializer.ProcessAsync();

        return context.FileData;
    }

    public Task WriteFileAsync(Gp5Score input, string path, CancellationToken cancellationToken = default) =>
        throw new System.NotImplementedException();
}
