using Microsoft.Extensions.DependencyInjection;
using TabAmp.Models;

namespace TabAmp.IO;

public partial class TabFileReader : ITabFileReader
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TabFileReader(IServiceScopeFactory serviceScopeFactory) =>
        _serviceScopeFactory = serviceScopeFactory;

    public Task<Song> ReadAsync(string path, CancellationToken cancellationToken)
    {
        using var scope = CreateScope();
        var context = CreateContextForScope(scope, path, cancellationToken);
        var readingProcedure = GetReadingProcedure(scope, context);
        return readingProcedure.ReadAsync();
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
