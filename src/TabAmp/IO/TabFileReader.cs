using Microsoft.Extensions.DependencyInjection;
using TabAmp.Commands;
using TabAmp.Models;

namespace TabAmp.IO;

public class TabFileReader : ITabFileReader
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly TabFileReaderContextFactory _contextFactory;

    public TabFileReader(IServiceScopeFactory serviceScopeFactory, TabFileReaderContextFactory contextFactory) =>
        (_serviceScopeFactory, _contextFactory) = (serviceScopeFactory, contextFactory);

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

    private ITabFileReaderContext CreateContextForScope(IServiceScope scope, string path, CancellationToken cancellationToken) => 
        _contextFactory.CreateContextForScope(scope, path, cancellationToken);

    private ITabFileReadingProcedure GetReadingProcedure(IServiceScope scope, ITabFileReaderContext context)
    {
        return context.FileExtension switch
        {
            TabFileExtension.GP5 => GetRequiredService<GP5ReadingProcedure>(scope),
            _ => throw new Exception($"{context.FilePath} filename extension is not supproted."),
        };
    }

    private T GetRequiredService<T>(IServiceScope scope) =>
        scope.ServiceProvider.GetRequiredService<T>();
}
