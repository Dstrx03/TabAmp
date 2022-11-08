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
            var song = await ReadSongAsync(request);
            return new ReadTabFileResult(song);
        }
        catch (TabFileReaderException e)
        {
            return new ReadTabFileResult(e);
        }
    }

    private async Task<Song> ReadSongAsync(ReadTabFileRequest request)
    {
        using var scope = CreateScope();
        BuildContextForScope(scope, request);
        var song = await ReadSongUsingScopeAsync(scope);
        return song;
    }

    private IServiceScope CreateScope() =>
        _serviceScopeFactory.CreateScope();

    private void BuildContextForScope(IServiceScope scope, ReadTabFileRequest request)
    {
        var contextBuilder = GetRequiredService<TabFileReaderContextBuilder>(scope);
        contextBuilder.SetContextData(request);
        contextBuilder.SignContext();
    }

    private Task<Song> ReadSongUsingScopeAsync(IServiceScope scope)
    {
        var readingProcedureFactory = GetRequiredService<TabFileReadingProcedureFactory>(scope);
        var readingProcedure = readingProcedureFactory.GetReadingProcedure();
        return readingProcedure.ReadAsync();
    }

    private T GetRequiredService<T>(IServiceScope scope) =>
        scope.ServiceProvider.GetRequiredService<T>();
}
