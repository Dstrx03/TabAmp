using Microsoft.Extensions.DependencyInjection;
using TabAmp.Commands;

namespace TabAmp.IO;

public partial class TabFileReaderContextBuilder
{
    private const string TabFileExtensionGP5 = ".gp5";

    private readonly TabFileReaderContext _context;

    public TabFileReaderContextBuilder(IServiceProvider serviceProvider) =>
        _context = serviceProvider.GetRequiredService<TabFileReaderContext>();

    public void SetContextData(ReadTabFileRequest request)
    {
        var fileInfo = new FileInfo(request.Path);

        _context.FilePath = fileInfo.FullName;
        _context.FileExtension = GetTabFileExtension(fileInfo);
        _context.CancellationToken = request.CancellationToken;
    }

    private TabFileExtension GetTabFileExtension(FileInfo fileInfo)
    {
        var extension = fileInfo.Extension.ToLowerInvariant();
        return extension switch
        {
            TabFileExtensionGP5 => TabFileExtension.GP5,
            _ => TabFileExtension.Other,
        };
    }

    public void SignContext() =>
        _context.Sign();
}
