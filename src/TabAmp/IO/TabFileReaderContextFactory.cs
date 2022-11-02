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

    private TabFileExtension GetFileExtension(FileInfo fileInfo) =>
        fileInfo.Extension.ToLowerInvariant() == ".gp5"
            ? TabFileExtension.GP5 : TabFileExtension.Other;

    private TabFileReaderContext GetTabFileReaderContext(IServiceScope scope) =>
        scope.ServiceProvider.GetRequiredService<TabFileReaderContext>();
}
