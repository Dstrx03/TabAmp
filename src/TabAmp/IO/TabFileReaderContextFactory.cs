using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.IO;

public partial class TabFileReaderContextFactory
{
    public ITabFileReaderContext CreateContextForScope(IServiceScope scope, string path, CancellationToken cancellationToken)
    {
        var context = GetTabFileReaderContext(scope);

        var fileInfo = new FileInfo(path);

        context.FilePath = fileInfo.FullName;
        context.FileExtension = GetFileExtension(fileInfo);
        context.CancellationToken = cancellationToken;

        return context;
    }

    private TabFileExtension GetFileExtension(FileInfo fileInfo)
    {
        var extension = fileInfo.Extension.ToLowerInvariant();
        return extension switch
        {
            ".gp5" => TabFileExtension.GP5,
            _ => TabFileExtension.Other,
        };
    }

    private TabFileReaderContext GetTabFileReaderContext(IServiceScope scope) =>
        scope.ServiceProvider.GetRequiredService<TabFileReaderContext>();
}
