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
        await ReadMeasureHeadersAsync();

        for (var i = 0; i < _song.TrackCount; i++)
        {
            var isFirstMeasure = i == 0;
            if (isFirstMeasure)
                if (await _reader.ReadNextByteAsync() != 0)
                    throw new InvalidOperationException();

            var flags1 = await _reader.ReadNextByteAsync();

            var isPercussionTrack = (flags1 & 0x01) > 0;
            var is12StringedGuitarTrack = (flags1 & 0x02) > 0;
            var isBanjoTrack = (flags1 & 0x04) > 0;
            var isVisible = (flags1 & 0x08) > 0;
            var isSolo = (flags1 & 0x10) > 0;
            var isMute = (flags1 & 0x20) > 0;
            var useRSE = (flags1 & 0x40) > 0;
            var indicateTuning = (flags1 & 0x80) > 0;

            var name = await _reader.ReadNextByteSizeStringAsync(40);
            var stringCount = await _reader.ReadNextIntAsync();
            var stringTunings = new List<int>();
            for (var j = 0; j < 7; j++)
            {
                var stringTuning = await _reader.ReadNextIntAsync();
                stringTunings.Add(stringTuning);
            }
            var port = await _reader.ReadNextIntAsync();
            var channelIndex = await _reader.ReadNextIntAsync() - 1;
            var effectChannel = await _reader.ReadNextIntAsync() - 1;
            var fretCount = await _reader.ReadNextIntAsync();
            var offset = await _reader.ReadNextIntAsync();
            var colorR = await _reader.ReadNextByteAsync();
            var colorG = await _reader.ReadNextByteAsync();
            var colorB = await _reader.ReadNextByteAsync();
            if (await _reader.ReadNextByteAsync() != 0)
                throw new InvalidOperationException();

            var flags2 = await _reader.ReadNextShortAsync();

            var tablature = (flags2 & 0x0001) > 0;
            var notation = (flags2 & 0x0002) > 0;
            var diagramsAreBelow = (flags2 & 0x0004) > 0;
            var showRhythm = (flags2 & 0x0008) > 0;
            var forceHorizontal = (flags2 & 0x0010) > 0;
            var forceChannels = (flags2 & 0x0020) > 0;
            var diagramList = (flags2 & 0x0040) > 0;
            var diagramsInScore = (flags2 & 0x0080) > 0;
            var unknown = (flags2 & 0x0100) > 0;
            var autoLetRing = (flags2 & 0x0200) > 0;
            var autoBrush = (flags2 & 0x0400) > 0;
            var extendRhythmic = (flags2 & 0x0800) > 0;

            var autoAccentuation = await _reader.ReadNextByteAsync();
            var bank = await _reader.ReadNextByteAsync();

            var trackRSEHumanize = await _reader.ReadNextByteAsync();
            var unknown1 = await _reader.ReadNextIntAsync();
            var unknown2 = await _reader.ReadNextIntAsync();
            var unknown3 = await _reader.ReadNextIntAsync();
            var unknown4 = await _reader.ReadNextIntAsync();
            var unknown5 = await _reader.ReadNextIntAsync();
            var unknown6 = await _reader.ReadNextIntAsync();

            var instrument = await _reader.ReadNextIntAsync();
            var unknown8 = await _reader.ReadNextIntAsync();
            var soundBank = await _reader.ReadNextIntAsync();
            var effectNumber = await _reader.ReadNextIntAsync();

            var equalizerKnobs = new List<sbyte>();
            for (var j = 0; j < 3; j++)
            {
                var knob = await _reader.ReadNextSignedByteAsync();
                equalizerKnobs.Add(knob);
            }
            var equalizerGain = await _reader.ReadNextSignedByteAsync();

            var effect = await _reader.ReadNextIntByteSizeStringAsync();
            var effectCategory = await _reader.ReadNextIntByteSizeStringAsync();
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
            if (midiChannel.Blank1 != 0 || midiChannel.Blank2 != 0)
                throw new InvalidOperationException();
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

    private async Task ReadMeasureHeadersAsync()
    {
        _song.MeasureHeaders = new List<MeasureHeader>();
        for (var i = 0; i < _song.MeasureCount; i++)
        {
            var measureHeader = new MeasureHeader();

            var isFirstMeasure = i == 0;
            if (!isFirstMeasure)
            {
                if (await _reader.ReadNextByteAsync() != 0)
                    throw new InvalidOperationException();
            }

            measureHeader.Flags = await _reader.ReadNextByteAsync();

            measureHeader.IsRepeatOpen = (measureHeader.Flags & 0x04) > 0;
            measureHeader.HasDoubleBar = (measureHeader.Flags & 0x80) > 0;

            if ((measureHeader.Flags & 0x01) > 0)
                measureHeader.Numerator = await _reader.ReadNextSignedByteAsync();
            if ((measureHeader.Flags & 0x02) > 0)
                measureHeader.Denominator = await _reader.ReadNextSignedByteAsync();
            if ((measureHeader.Flags & 0x08) > 0)
                measureHeader.RepeatClose = await _reader.ReadNextSignedByteAsync();
            if ((measureHeader.Flags & 0x20) > 0)
            {
                var marker = new Marker();
                marker.Title = await _reader.ReadNextIntByteSizeStringAsync();
                marker.ColorR = await _reader.ReadNextByteAsync();
                marker.ColorG = await _reader.ReadNextByteAsync();
                marker.ColorB = await _reader.ReadNextByteAsync();
                if (await _reader.ReadNextByteAsync() != 0)
                    throw new InvalidOperationException();
                measureHeader.Marker = marker;
            }
            if ((measureHeader.Flags & 0x40) > 0)
            {
                measureHeader.Root = await _reader.ReadNextSignedByteAsync();
                measureHeader.Type = await _reader.ReadNextSignedByteAsync();
            }
            if ((measureHeader.Flags & 0x10) > 0)
                measureHeader.RepeatAlternative = await _reader.ReadNextByteAsync();
            if ((measureHeader.Flags & 0x03) > 0)
            {
                measureHeader.Beams = new List<byte>();
                for (var j = 0; j < 4; j++)
                {
                    var beam = await _reader.ReadNextByteAsync();
                    measureHeader.Beams.Add(beam);
                }
            }
            if ((measureHeader.Flags & 0x10) == 0)
            {
                if (await _reader.ReadNextByteAsync() != 0)
                    throw new InvalidOperationException();
            }
            measureHeader.TripletFeel = await _reader.ReadNextByteAsync();

            _song.MeasureHeaders.Add(measureHeader);
        }
    }
}
