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
        await ReadLyricsAsync();
        await ReadRSEMasterEffectAsync();
        await ReadPageSetupAsync();
        await ReadTempoAsync();
        await ReadKeyOctaveAsync();
        await ReadMidiChannelsAsync();
        await ReadMusicalDirectionsAsync();
        await ReadRSEMasterEffectReverbAsync();
        await ReadMeasureTrackCountAsync();

        for (var i = 0; i < _song.MeasureCount; i++)
        {
            byte blankByte0 = 1;
            if (i > 0)
            {
                blankByte0 = await _reader.ReadNextByteAsync();
            }

            var flags = await _reader.ReadNextByteAsync();
            sbyte numerator;
            sbyte denominator;
            sbyte repeatClose;
            string markerTitle;
            byte markerColorR;
            byte markerColorG;
            byte markerColorB;
            sbyte root;
            sbyte type;
            byte repeatAlternative;
            byte beams1;
            byte beams2;
            byte beams3;
            byte beams4;
            if ((flags & 0x01) > 0)
                numerator = await _reader.ReadNextSignedByteAsync();
            if ((flags & 0x02) > 0)
                denominator = await _reader.ReadNextSignedByteAsync();
            var isRepeatOpen = (flags & 0x04) > 0;
            if ((flags & 0x08) > 0)
                repeatClose = await _reader.ReadNextSignedByteAsync();
            if ((flags & 0x20) > 0)
            {
                markerTitle = await _reader.ReadNextIntByteSizeStringAsync();
                markerColorR = await _reader.ReadNextByteAsync();
                markerColorG = await _reader.ReadNextByteAsync();
                markerColorB = await _reader.ReadNextByteAsync();
                var blankMarkerColorByte = await _reader.ReadNextByteAsync();
            }
            if ((flags & 0x40) > 0)
            {
                root = await _reader.ReadNextSignedByteAsync();
                type = await _reader.ReadNextSignedByteAsync();
            }
            if ((flags & 0x10) > 0)
                repeatAlternative = await _reader.ReadNextByteAsync();
            var hasDoubleBar = (flags & 0x80) > 0;
            if ((flags & 0x03) > 0)
            {
                beams1 = await _reader.ReadNextByteAsync();
                beams2 = await _reader.ReadNextByteAsync();
                beams3 = await _reader.ReadNextByteAsync();
                beams4 = await _reader.ReadNextByteAsync();
            }
            byte blankByte1 = 1;
            if ((flags & 0x10) == 0)
                blankByte1 = await _reader.ReadNextByteAsync();
            var tripletFeel = await _reader.ReadNextByteAsync();
        }

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

    private async Task ReadLyricsAsync()
    {
        var lyrics = new Lyrics
        {
            TrackChoice = await _reader.ReadNextIntAsync(),
            Lines = new List<(int startingMeasure, string line)>()
        };

        for (var i = 0; i < 5; i++)
        {
            var startingMeasure = await _reader.ReadNextIntAsync();
            var line = await _reader.ReadNextIntSizeStringAsync();
            lyrics.Lines.Add((startingMeasure, line));
        }

        _song.Lyrics = lyrics;
    }

    private async Task ReadRSEMasterEffectAsync()
    {
        var masterEffect = new RSEMasterEffect
        {
            Volume = await _reader.ReadNextIntAsync(),
            UnknownProperty_0 = await _reader.ReadNextIntAsync(),
            EqualizerKnobs = new List<sbyte>()
        };

        for (var i = 0; i < 10; i++)
        {
            var knob = await _reader.ReadNextSignedByteAsync();
            masterEffect.EqualizerKnobs.Add(knob);
        }

        masterEffect.EqualizerGain = await _reader.ReadNextSignedByteAsync();

        _song.RSEMasterEffect = masterEffect;
    }

    private async Task ReadPageSetupAsync()
    {
        var pageSetup = new PageSetup
        {
            Width = await _reader.ReadNextIntAsync(),
            Height = await _reader.ReadNextIntAsync(),
            MarginLeft = await _reader.ReadNextIntAsync(),
            MarginRight = await _reader.ReadNextIntAsync(),
            MarginTop = await _reader.ReadNextIntAsync(),
            MarginBottom = await _reader.ReadNextIntAsync(),
            ScoreSizeProportion = await _reader.ReadNextIntAsync(),
            HeaderAndFooter = await _reader.ReadNextShortAsync(),
            Title = await _reader.ReadNextIntByteSizeStringAsync(),
            Subtitle = await _reader.ReadNextIntByteSizeStringAsync(),
            Artist = await _reader.ReadNextIntByteSizeStringAsync(),
            Album = await _reader.ReadNextIntByteSizeStringAsync(),
            Words = await _reader.ReadNextIntByteSizeStringAsync(),
            Music = await _reader.ReadNextIntByteSizeStringAsync(),
            WordsAndMusic = await _reader.ReadNextIntByteSizeStringAsync(),
            Copyright = await _reader.ReadNextIntByteSizeStringAsync(),
            CopyrightAdditional = await _reader.ReadNextIntByteSizeStringAsync(),
            PageNumber = await _reader.ReadNextIntByteSizeStringAsync()
        };

        _song.PageSetup = pageSetup;
    }

    private async Task ReadTempoAsync()
    {
        var tempo = new Tempo
        {
            Name = await _reader.ReadNextIntByteSizeStringAsync(),
            Value = await _reader.ReadNextIntAsync(),
            Hide = await _reader.ReadNextBoolAsync()
        };

        _song.Tempo = tempo;
    }

    private async Task ReadKeyOctaveAsync()
    {
        _song.Key = await _reader.ReadNextSignedByteAsync();
        _song.Octave = await _reader.ReadNextIntAsync();
    }

    private async Task ReadMidiChannelsAsync()
    {
        _song.MidiChannels = new List<MidiChannel>();
        for (var i = 0; i < 64; i++)
        {
            var midiChannel = new MidiChannel
            {
                Instrument = await _reader.ReadNextIntAsync(),
                Volume = await _reader.ReadNextSignedByteAsync(),
                Balance = await _reader.ReadNextSignedByteAsync(),
                Chorus = await _reader.ReadNextSignedByteAsync(),
                Reverb = await _reader.ReadNextSignedByteAsync(),
                Phaser = await _reader.ReadNextSignedByteAsync(),
                Tremolo = await _reader.ReadNextSignedByteAsync(),
                Blank1 = await _reader.ReadNextSignedByteAsync(),
                Blank2 = await _reader.ReadNextSignedByteAsync()
            };
            _song.MidiChannels.Add(midiChannel);
        }
    }

    private async Task ReadMusicalDirectionsAsync()
    {
        _song.MusicalDirections = new List<(string, short)>
        {
            ("Coda", await _reader.ReadNextShortAsync()),
            ("Double Coda", await _reader.ReadNextShortAsync()),
            ("Segno", await _reader.ReadNextShortAsync()),
            ("Segno Segno", await _reader.ReadNextShortAsync()),
            ("Fine", await _reader.ReadNextShortAsync()),
            ("Da Capo", await _reader.ReadNextShortAsync()),
            ("Da Capo al Coda", await _reader.ReadNextShortAsync()),
            ("Da Capo al Double Coda", await _reader.ReadNextShortAsync()),
            ("Da Capo al Fine", await _reader.ReadNextShortAsync()),
            ("Da Segno", await _reader.ReadNextShortAsync()),
            ("Da Segno al Coda", await _reader.ReadNextShortAsync()),
            ("Da Segno al Double Coda", await _reader.ReadNextShortAsync()),
            ("Da Segno al Fine", await _reader.ReadNextShortAsync()),
            ("Da Segno Segno", await _reader.ReadNextShortAsync()),
            ("Da Segno Segno al Coda", await _reader.ReadNextShortAsync()),
            ("Da Segno Segno al Double Coda", await _reader.ReadNextShortAsync()),
            ("Da Segno Segno al Fine", await _reader.ReadNextShortAsync()),
            ("Da Coda", await _reader.ReadNextShortAsync()),
            ("Da Double Coda", await _reader.ReadNextShortAsync())
        };
    }

    private async Task ReadRSEMasterEffectReverbAsync()
    {
        _song.RSEMasterEffect.Reverb = await _reader.ReadNextIntAsync();
    }

    private async Task ReadMeasureTrackCountAsync()
    {
        _song.MeasureCount = await _reader.ReadNextIntAsync();
        _song.TrackCount = await _reader.ReadNextIntAsync();
    }
}
