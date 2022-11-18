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
            var tabFile = await ReadTabFileAsync(request);
            return new ReadTabFileResult(tabFile);
        }
        catch (TabFileReaderException e)
        {
            return new ReadTabFileResult(e);
        }
    }

    private async Task<TabFile> ReadTabFileAsync(ReadTabFileRequest request)
    {
        using var scope = CreateScope();
        BuildContextForScope(scope, request);
        var tabFile = await ReadTabFileUsingScopeAsync(scope);
        return tabFile;
    }

    private IServiceScope CreateScope() =>
        _serviceScopeFactory.CreateScope();

    private void BuildContextForScope(IServiceScope scope, ReadTabFileRequest request)
    {
        var contextBuilder = GetRequiredService<TabFileReaderContextBuilder>(scope);
        contextBuilder.BuildContext(request);
    }

    private Task<TabFile> ReadTabFileUsingScopeAsync(IServiceScope scope)
    {
        var readingProcedureFactory = GetRequiredService<TabFileReadingProcedureFactory>(scope);
        var readingProcedure = readingProcedureFactory.GetReadingProcedure();
        return readingProcedure.ReadAsync();
    }

    private T GetRequiredService<T>(IServiceScope scope) where T : notnull =>
        scope.ServiceProvider.GetRequiredService<T>();
}
