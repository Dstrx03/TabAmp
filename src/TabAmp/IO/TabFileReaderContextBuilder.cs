using Microsoft.Extensions.DependencyInjection;
using TabAmp.Commands;

namespace TabAmp.IO;

public partial class TabFileReaderContextBuilder
{
    private readonly TabFileReaderContext _context;
    private readonly IPathParser _pathParser;

    public TabFileReaderContextBuilder(IServiceProvider serviceProvider, IPathParser pathParser) =>
        (_context, _pathParser) = (serviceProvider.GetRequiredService<TabFileReaderContext>(), pathParser);

    public bool IsContextBuilt { get; private set; }

    public ITabFileReaderContext GetContext()
    {
        if (!IsContextBuilt)
            throw new InvalidOperationException($"Cannot provide not built '{typeof(TabFileReaderContext)}'.");
        return _context;
    }

    public void BuildContext(ReadTabFileRequest request)
    {
        if (IsContextBuilt)
            throw new InvalidOperationException($"Cannot build '{typeof(TabFileReaderContext)}', it's already built for its scope.");
        SetContextData(request);
        IsContextBuilt = true;
    }

    private void SetContextData(ReadTabFileRequest request)
    {
        _context.PathInfo = _pathParser.Parse(request.Path);
        _context.CancellationToken = request.CancellationToken;
    }
}
