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
        song.Version = await _reader.ReadNextByteSizeStringAsync(30);

        // Score Info
        var title = await _reader.ReadNextIntByteSizeStringAsync();
        var subtitle = await _reader.ReadNextIntByteSizeStringAsync();
        var artist = await _reader.ReadNextIntByteSizeStringAsync();
        var album = await _reader.ReadNextIntByteSizeStringAsync();
        var words = await _reader.ReadNextIntByteSizeStringAsync();
        var music = await _reader.ReadNextIntByteSizeStringAsync();
        var copyright = await _reader.ReadNextIntByteSizeStringAsync();
        var tab = await _reader.ReadNextIntByteSizeStringAsync();
        var instructions = await _reader.ReadNextIntByteSizeStringAsync();

        var noticeCount = await _reader.ReadNextIntAsync();
        var notice = new List<string>();
        for (var i = 0; i < noticeCount; i++)
            notice.Add(await _reader.ReadNextIntByteSizeStringAsync());

        // Lyrics
        var trackChoice = await _reader.ReadNextIntAsync();
        var startingMeasure1 = await _reader.ReadNextIntAsync();
        var lyrics1 = await _reader.ReadNextIntSizeStringAsync();
        var startingMeasure2 = await _reader.ReadNextIntAsync();
        var lyrics2 = await _reader.ReadNextIntSizeStringAsync();
        var startingMeasure3 = await _reader.ReadNextIntAsync();
        var lyrics3 = await _reader.ReadNextIntSizeStringAsync();
        var startingMeasure4 = await _reader.ReadNextIntAsync();
        var lyrics4 = await _reader.ReadNextIntSizeStringAsync();
        var startingMeasure5 = await _reader.ReadNextIntAsync();
        var lyrics5 = await _reader.ReadNextIntSizeStringAsync();

        // RSE Master Effect
        var masterEffectVolume = await _reader.ReadNextIntAsync();
        var masterEffectUnknwnTodo = await _reader.ReadNextIntAsync();


        return new TabFile(_context.PathInfo, song);
    }
}
