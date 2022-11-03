using Microsoft.Extensions.DependencyInjection;
using TabAmp.Commands;
using TabAmp.Models;

namespace TabAmp.IO;

public class TabFileReader : ITabFileReader
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TabFileReader(IServiceScopeFactory serviceScopeFactory) =>
        _serviceScopeFactory = serviceScopeFactory;

    public async Task<ReadTabFileResult> ReadAsync(ReadTabFileRequest request)
    {
        try
        {
            var song = await ReadSongUsingScopeAsync(request);
            return new ReadTabFileResult(song);
        }
        catch (Exception e)
        {
            return new ReadTabFileResult(e);
        }
    }

    private async Task<Song> ReadSongUsingScopeAsync(ReadTabFileRequest request)
    {
        using var scope = CreateScope();
        CreateContextForScope(scope, request);
        var procedure = GetReadingProcedure(scope);
        var song = await procedure.ReadAsync();
        return song;
    }

    private IServiceScope CreateScope() =>
        _serviceScopeFactory.CreateScope();

    private ITabFileReaderContext CreateContextForScope(IServiceScope scope, ReadTabFileRequest request) =>
        GetRequiredService<TabFileReaderContextFactory>(scope).CreateContextForScope(request);

    private ITabFileReadingProcedure GetReadingProcedure(IServiceScope scope)
    {
        var context = GetRequiredService<ITabFileReaderContext>(scope);
        return context.FileExtension switch
        {
            TabFileExtension.GP5 => GetRequiredService<GP5ReadingProcedure>(scope),
            _ => throw new Exception($"{context.FilePath} filename extension is not supproted."),
        };
    }

    private T GetRequiredService<T>(IServiceScope scope) =>
        scope.ServiceProvider.GetRequiredService<T>();
}
