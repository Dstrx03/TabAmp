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
        catch (TabFileReaderException e)
        {
            return new ReadTabFileResult(e);
        }
    }

    private async Task<Song> ReadSongUsingScopeAsync(ReadTabFileRequest request)
    {
        using var scope = CreateScope();
        CreateContextForScope(scope, request);
        var procedure = GetReadingProcedureForScope(scope);
        var song = await procedure.ReadAsync();
        return song;
    }

    private IServiceScope CreateScope() =>
        _serviceScopeFactory.CreateScope();

    private ITabFileReaderContext CreateContextForScope(IServiceScope scope, ReadTabFileRequest request) =>
        GetRequiredService<TabFileReaderContextFactory>(scope).CreateContextForScope(request);

    private ITabFileReadingProcedure GetReadingProcedureForScope(IServiceScope scope) =>
        GetRequiredService<TabFileReadingProcedureFactory>(scope).GetReadingProcedure();

    private T GetRequiredService<T>(IServiceScope scope) =>
        scope.ServiceProvider.GetRequiredService<T>();
}
