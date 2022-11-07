using Microsoft.Extensions.DependencyInjection;
using TabAmp.Commands;

namespace TabAmp.IO;

public partial class TabFileReaderContextFactory
{
    private const string TabFileExtensionGP5 = ".gp5";

    private readonly TabFileReaderContext _context;

    public TabFileReaderContextFactory(IServiceProvider serviceProvider) =>
        _context = serviceProvider.GetRequiredService<TabFileReaderContext>();

    public void CreateContext(ReadTabFileRequest request)
    {
        var fileInfo = new FileInfo(request.Path);

        _context.FilePath = fileInfo.FullName;
        _context.FileExtension = GetTabFileExtension(fileInfo);
        _context.CancellationToken = request.CancellationToken;

        SignContext();
    }

    private void SignContext() =>
        _context.Sign();

    private TabFileExtension GetTabFileExtension(FileInfo fileInfo)
    {
        var extension = fileInfo.Extension.ToLowerInvariant();
        return extension switch
        {
            TabFileExtensionGP5 => TabFileExtension.GP5,
            _ => TabFileExtension.Other,
        };
    }
}
