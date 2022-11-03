using Microsoft.Extensions.DependencyInjection;
using TabAmp.Commands;

namespace TabAmp.IO;

public partial class TabFileReaderContextFactory
{
    public ITabFileReaderContext CreateContextForScope(IServiceScope scope, ReadTabFileRequest request)
    {
        var context = CreateTabFileReaderContext(scope);
        InitTabFileReaderContext(context, request);
        return context;
    }

    private TabFileReaderContext CreateTabFileReaderContext(IServiceScope scope) =>
        scope.ServiceProvider.GetRequiredService<TabFileReaderContext>();

    private void InitTabFileReaderContext(TabFileReaderContext context, ReadTabFileRequest request)
    {
        var fileInfo = new FileInfo(request.Path);
        context.FilePath = fileInfo.FullName;
        context.FileExtension = GetFileExtension(fileInfo);
        context.CancellationToken = request.CancellationToken;
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
