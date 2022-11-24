using TabAmp.Models;

namespace TabAmp.IO;

public class GP5ReadingProcedure : ITabFileReadingProcedure
{
    private readonly ITabFileReaderContext _context;
    private readonly GP5BasicTypesReader _reader;
    private readonly GP5Song _song;

    public GP5ReadingProcedure(ITabFileReaderContext context, GP5BasicTypesReader reader)
    {
        _context = context;
        _reader = reader;
        _song = new GP5Song();
    }

    public async Task<TabFile> ReadAsync()
    {
        await ReadVersionAsync();
        await ReadScoreInformationAsync();


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


        return new TabFile(_context.PathInfo, _song);
    }

    private async Task ReadVersionAsync()
    {
        _song.Version = await _reader.ReadNextByteSizeStringAsync(30);
    }

    private async Task ReadScoreInformationAsync()
    {
        var scoreInformation = new ScoreInformation
        {
            Title = await _reader.ReadNextIntByteSizeStringAsync(),
            Subtitle = await _reader.ReadNextIntByteSizeStringAsync(),
            Artist = await _reader.ReadNextIntByteSizeStringAsync(),
            Album = await _reader.ReadNextIntByteSizeStringAsync(),
            Words = await _reader.ReadNextIntByteSizeStringAsync(),
            Music = await _reader.ReadNextIntByteSizeStringAsync(),
            Copyright = await _reader.ReadNextIntByteSizeStringAsync(),
            Tab = await _reader.ReadNextIntByteSizeStringAsync(),
            Instructions = await _reader.ReadNextIntByteSizeStringAsync(),
            Notice = new List<string>()
        };

        var noticeCount = await _reader.ReadNextIntAsync();
        for (var i = 0; i < noticeCount; i++)
        {
            var noticeElement = await _reader.ReadNextIntByteSizeStringAsync();
            scoreInformation.Notice.Add(noticeElement);
        }

        _song.ScoreInformation = scoreInformation;
    }
}
