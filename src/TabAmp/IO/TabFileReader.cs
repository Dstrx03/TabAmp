using Microsoft.Extensions.DependencyInjection;
using TabAmp.Commands;
using TabAmp.Models;

namespace TabAmp.IO;

public partial class TabFileReader : ITabFileReader
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TabFileReader(IServiceScopeFactory serviceScopeFactory) =>
        _serviceScopeFactory = serviceScopeFactory;

    public async Task<ReadTabFileResult> ReadAsync(string path, CancellationToken cancellationToken)
    {
        try
        {
            var song = await ReadSongUsingScopeAsync(path, cancellationToken);
            return new ReadTabFileResult(song);
        }
        catch (Exception e)
        {
            return new ReadTabFileResult(e);
        }
    }

    private async Task<Song> ReadSongUsingScopeAsync(string path, CancellationToken cancellationToken)
    {
        using var scope = CreateScope();
        var context = CreateContextForScope(scope, path, cancellationToken);
        var readingProcedure = GetReadingProcedure(scope, context);
        var song = await readingProcedure.ReadAsync();
        return song;
    }

    private IServiceScope CreateScope() =>
        _serviceScopeFactory.CreateScope();

    private ITabFileReaderContext CreateContextForScope(IServiceScope scope, string path, CancellationToken cancellationToken)
    {
        var context = GetRequiredService<TabFileReaderContext>(scope);
        context.Path = path;
        context.CancellationToken = cancellationToken;
        return context;
    }

    private ITabFileReadingProcedure GetReadingProcedure(IServiceScope scope, ITabFileReaderContext context) =>
        GetRequiredService<GP5ReadingProcedure>(scope);

    private T GetRequiredService<T>(IServiceScope scope) =>
        scope.ServiceProvider.GetRequiredService<T>();
}
