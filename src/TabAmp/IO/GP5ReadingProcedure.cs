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
        var tabFile = new TabFile(_context.PathInfo, song);

        // Version
        song.Version = await _reader.ReadNextByteSizeStringAsync(30);

        // Score Info
        var scoreInformation = new ScoreInformation();
        scoreInformation.Title = await _reader.ReadNextIntByteSizeStringAsync();
        scoreInformation.Subtitle = await _reader.ReadNextIntByteSizeStringAsync();
        scoreInformation.Artist = await _reader.ReadNextIntByteSizeStringAsync();
        scoreInformation.Album = await _reader.ReadNextIntByteSizeStringAsync();
        scoreInformation.Words = await _reader.ReadNextIntByteSizeStringAsync();
        scoreInformation.Music = await _reader.ReadNextIntByteSizeStringAsync();
        scoreInformation.Copyright = await _reader.ReadNextIntByteSizeStringAsync();
        scoreInformation.Tab = await _reader.ReadNextIntByteSizeStringAsync();
        scoreInformation.Instructions = await _reader.ReadNextIntByteSizeStringAsync();

        var noticeCount = await _reader.ReadNextIntAsync();
        scoreInformation.Notice = new List<string>();
        for (var i = 0; i < noticeCount; i++)
            scoreInformation.Notice.Add(await _reader.ReadNextIntByteSizeStringAsync());

        song.ScoreInformation = scoreInformation;

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


        return tabFile;
    }
}
