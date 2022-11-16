using Microsoft.Extensions.DependencyInjection;
using TabAmp.Commands;

namespace TabAmp.IO;

public partial class TabFileReaderContextBuilder
{
    private readonly TabFileReaderContext _context;
    private readonly IPathParser _pathParser;

    public TabFileReaderContextBuilder(IServiceProvider serviceProvider, IPathParser pathParser) =>
        (_context, _pathParser) = (serviceProvider.GetRequiredService<TabFileReaderContext>(), pathParser);

    public void BuildContext(ReadTabFileRequest request)
    {
        CheckContextCanBeBuilt();
        SetContextData(request);
        SignContext();
    }

    private void CheckContextCanBeBuilt()
    {
        if (_context.IsBuilt)
            throw new InvalidOperationException($"Cannot build '{typeof(TabFileReaderContext)}', it's already built for its scope.");
    }

    private void SetContextData(ReadTabFileRequest request)
    {
        _context.PathInfo = _pathParser.Parse(request.Path);
        _context.CancellationToken = request.CancellationToken;
    }

    private void SignContext() =>
        _context.Sign();
}
