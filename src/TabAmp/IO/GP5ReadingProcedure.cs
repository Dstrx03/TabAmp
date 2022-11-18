using TabAmp.Models;

namespace TabAmp.IO;

public class GP5ReadingProcedure : ITabFileReadingProcedure
{
    private readonly ITabFileReaderContext _context;
    private readonly GP5BasicTypesReader _reader;

    public GP5ReadingProcedure(ITabFileReaderContext context, GP5BasicTypesReader reader) =>
        (_context, _reader) = (context, reader);

    public async Task<TabFile> ReadAsync()
    {
        var song = new GP5Song();
        song.Version = await _reader.ReadNextByteSizeStringAsync();
        return new TabFile
        {
            PathInfo = _context.PathInfo,
            Song = song
        };
    }
}
