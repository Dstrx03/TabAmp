using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.IO;

public partial class TabFileReaderContextFactory
{
    public ITabFileReaderContext CreateContextForScope(IServiceScope scope, string path, CancellationToken cancellationToken)
    {
        var context = CreateTabFileReaderContext(scope);
        PopulateTabFileReaderContext(context, path, cancellationToken);
        return context;
    }

    private TabFileReaderContext CreateTabFileReaderContext(IServiceScope scope) =>
        scope.ServiceProvider.GetRequiredService<TabFileReaderContext>();

    private void PopulateTabFileReaderContext(TabFileReaderContext context, string path, CancellationToken cancellationToken)
    {
        var fileInfo = new FileInfo(path);
        context.FilePath = fileInfo.FullName;
        context.FileExtension = GetFileExtension(fileInfo);
        context.CancellationToken = cancellationToken;
    }

    private TabFileExtension GetFileExtension(FileInfo fileInfo)
    {
        return fileInfo.Extension.ToLowerInvariant() switch
        {
            ".gp5" => TabFileExtension.GP5,
            _ => TabFileExtension.Other,
        };
    }
}
