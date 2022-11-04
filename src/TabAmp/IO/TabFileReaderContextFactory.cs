using TabAmp.Commands;

namespace TabAmp.IO;

public partial class TabFileReaderContextFactory
{
    private const string GP5 = ".gp5";

    private readonly TabFileReaderContext _context;

    public TabFileReaderContextFactory(ITabFileReaderContext context) =>
        _context = (TabFileReaderContext)context;

    public ITabFileReaderContext CreateContextForScope(ReadTabFileRequest request)
    {
        var fileInfo = new FileInfo(request.Path);
        _context.FilePath = fileInfo.FullName;
        _context.FileExtension = GetTabFileExtension(fileInfo);
        _context.CancellationToken = request.CancellationToken;
        return _context;
    }

    private TabFileExtension GetTabFileExtension(FileInfo fileInfo)
    {
        var extension = fileInfo.Extension.ToLowerInvariant();
        return extension switch
        {
            GP5 => TabFileExtension.GP5,
            _ => TabFileExtension.Other,
        };
    }
}
