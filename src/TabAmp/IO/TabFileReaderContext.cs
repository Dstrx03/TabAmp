using TabAmp.Models;

namespace TabAmp.IO;

public partial class TabFileReaderContextBuilder
{
    private class TabFileReaderContext : ITabFileReaderContext
    {
        public PathInfo PathInfo { get; set; } = null;
        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;
    }
}
