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
        await ReadTracksAsync();
        await ReadMeasuresAsync();

        return new TabFile(_context.PathInfo, _song);
    }

    private async Task ReadVersionAsync()
    {
        _song.Version = await _reader.ReadNextByteSizeStringAsync(30);
        if (!_song.Version.Equals("FICHIER GUITAR PRO v5.10"))
            throw new InvalidOperationException();
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

    private async Task ReadTracksAsync()
    {
        _song.Tracks = new List<Track>();
        for (var i = 0; i < _song.TrackCount; i++)
        {
            var track = new Track();

            var isFirstMeasure = i == 0;
            if (isFirstMeasure)
                if (await _reader.ReadNextByteAsync() != 0)
                    throw new InvalidOperationException();

            track.Flags1 = await _reader.ReadNextByteAsync();

            track.IsPercussionTrack = (track.Flags1 & 0x01) > 0;
            track.Is12StringedGuitarTrack = (track.Flags1 & 0x02) > 0;
            track.IsBanjoTrack = (track.Flags1 & 0x04) > 0;
            track.IsVisible = (track.Flags1 & 0x08) > 0;
            track.IsSolo = (track.Flags1 & 0x10) > 0;
            track.IsMute = (track.Flags1 & 0x20) > 0;
            track.UseRSE = (track.Flags1 & 0x40) > 0;
            track.IndicateTuning = (track.Flags1 & 0x80) > 0;

            track.Name = await _reader.ReadNextByteSizeStringAsync(40);
            track.StringCount = await _reader.ReadNextIntAsync();
            track.StringTunings = new List<int>();
            for (var j = 0; j < 7; j++)
            {
                var stringTuning = await _reader.ReadNextIntAsync();
                track.StringTunings.Add(stringTuning);
            }
            track.Port = await _reader.ReadNextIntAsync();
            track.ChannelIndex = await _reader.ReadNextIntAsync() - 1;
            track.EffectChannel = await _reader.ReadNextIntAsync() - 1;
            track.FretCount = await _reader.ReadNextIntAsync();
            track.Offset = await _reader.ReadNextIntAsync();
            track.ColorR = await _reader.ReadNextByteAsync();
            track.ColorG = await _reader.ReadNextByteAsync();
            track.ColorB = await _reader.ReadNextByteAsync();
            if (await _reader.ReadNextByteAsync() != 0)
                throw new InvalidOperationException();

            track.Flags2 = await _reader.ReadNextShortAsync();

            track.Tablature = (track.Flags2 & 0x0001) > 0;
            track.Notation = (track.Flags2 & 0x0002) > 0;
            track.DiagramsAreBelow = (track.Flags2 & 0x0004) > 0;
            track.ShowRhythm = (track.Flags2 & 0x0008) > 0;
            track.ForceHorizontal = (track.Flags2 & 0x0010) > 0;
            track.ForceChannels = (track.Flags2 & 0x0020) > 0;
            track.DiagramList = (track.Flags2 & 0x0040) > 0;
            track.DiagramsInScore = (track.Flags2 & 0x0080) > 0;
            track.Unknown0 = (track.Flags2 & 0x0100) > 0;
            track.AutoLetRing = (track.Flags2 & 0x0200) > 0;
            track.AutoBrush = (track.Flags2 & 0x0400) > 0;
            track.ExtendRhythmic = (track.Flags2 & 0x0800) > 0;

            track.AutoAccentuation = await _reader.ReadNextByteAsync();
            track.Bank = await _reader.ReadNextByteAsync();

            track.TrackRSEHumanize = await _reader.ReadNextByteAsync();
            track.Unknown1 = await _reader.ReadNextIntAsync();
            track.Unknown2 = await _reader.ReadNextIntAsync();
            track.Unknown3 = await _reader.ReadNextIntAsync();
            track.Unknown4 = await _reader.ReadNextIntAsync();
            track.Unknown5 = await _reader.ReadNextIntAsync();
            track.Unknown6 = await _reader.ReadNextIntAsync();

            track.Instrument = await _reader.ReadNextIntAsync();
            track.Unknown8 = await _reader.ReadNextIntAsync();
            track.SoundBank = await _reader.ReadNextIntAsync();
            track.EffectNumber = await _reader.ReadNextIntAsync();

            track.EqualizerKnobs = new List<sbyte>();
            for (var j = 0; j < 3; j++)
            {
                var knob = await _reader.ReadNextSignedByteAsync();
                track.EqualizerKnobs.Add(knob);
            }
            track.EqualizerGain = await _reader.ReadNextSignedByteAsync();

            track.Effect = await _reader.ReadNextIntByteSizeStringAsync();
            track.EffectCategory = await _reader.ReadNextIntByteSizeStringAsync();

            _song.Tracks.Add(track);
        }

        if (await _reader.ReadNextByteAsync() != 0)
            throw new InvalidOperationException();
    }

    private async Task ReadMeasuresAsync()
    {
        _song.Measures = new List<Measure>();
        for (var i = 0; i < _song.MeasureCount; i++)
        {
            for (var j = 0; j < _song.TrackCount; j++)
            {
                var measure = new Measure();

                measure.BeatsCount = await _reader.ReadNextIntAsync();

                measure.Beats = new List<Beat>();
                for (var c = 0; c < measure.BeatsCount; c++)
                {
                    var beat = new Beat();

                    beat.Flags = await _reader.ReadNextByteAsync();

                    if ((beat.Flags & 0x40) > 0)
                        beat.Status = await _reader.ReadNextByteAsync();

                    beat.Duration = await _reader.ReadNextSignedByteAsync();

                    if ((beat.Flags & 0x02) > 0)
                    {
                        var chord = new Chord();
                        chord.NewFormat = await _reader.ReadNextBoolAsync();
                        if (!chord.NewFormat)
                        {
                            chord.Name = await _reader.ReadNextIntByteSizeStringAsync();
                            chord.FirstFret = await _reader.ReadNextIntAsync();
                            if (chord.FirstFret > -1)
                            {
                                var strings = new List<int>();
                                for (var q = 0; q < 6; q++)
                                {
                                    var fret = await _reader.ReadNextIntAsync();
                                    strings.Add(fret);
                                }
                                chord.Strings = strings;
                            }
                        }
                        else
                        {
                            chord.Sharp = await _reader.ReadNextBoolAsync();
                            chord.Unknown0 = await _reader.ReadNextByteAsync();
                            chord.Unknown1 = await _reader.ReadNextByteAsync();
                            chord.Unknown2 = await _reader.ReadNextByteAsync();
                            chord.Root = await _reader.ReadNextByteAsync();
                            chord.Type = await _reader.ReadNextByteAsync();
                            chord.Extension = await _reader.ReadNextByteAsync();
                            chord.Bass = await _reader.ReadNextIntAsync();
                            chord.Tonality = await _reader.ReadNextIntAsync();
                            chord.Add = await _reader.ReadNextBoolAsync();
                            chord.Name = await _reader.ReadNextByteSizeStringAsync(22);
                            chord.Fifth = await _reader.ReadNextByteAsync();
                            chord.Ninth = await _reader.ReadNextByteAsync();
                            chord.Eleventh = await _reader.ReadNextByteAsync();
                            chord.FirstFret = await _reader.ReadNextIntAsync();
                            var strings = new List<int>();
                            for (var q = 0; q < 7; q++)
                            {
                                var fret = await _reader.ReadNextIntAsync();
                                strings.Add(fret);
                            }
                            chord.Strings = strings;
                            chord.BarresCount = await _reader.ReadNextByteAsync();
                            var barreFrets = new List<byte>();
                            for (var q = 0; q < 5; q++)
                            {
                                var fret = await _reader.ReadNextByteAsync();
                                barreFrets.Add(fret);
                            }
                            chord.BarreFrets = barreFrets;
                            var barreStarts = new List<byte>();
                            for (var q = 0; q < 5; q++)
                            {
                                var start = await _reader.ReadNextByteAsync();
                                barreStarts.Add(start);
                            }
                            chord.BarreStarts = barreStarts;
                            var barreEnds = new List<byte>();
                            for (var q = 0; q < 5; q++)
                            {
                                var end = await _reader.ReadNextByteAsync();
                                barreEnds.Add(end);
                            }
                            chord.BarreEnds = barreEnds;
                            var omissions = new List<bool>();
                            for (var q = 0; q < 7; q++)
                            {
                                var omission = await _reader.ReadNextBoolAsync();
                                omissions.Add(omission);
                            }
                            chord.Omissions = omissions;
                            chord.Unknown3 = await _reader.ReadNextByteAsync();
                            var fingerings = new List<sbyte>();
                            for (var q = 0; q < 7; q++)
                            {
                                var fingering = await _reader.ReadNextSignedByteAsync();
                                fingerings.Add(fingering);
                            }
                            chord.Fingerings = fingerings;
                            chord.Show = await _reader.ReadNextBoolAsync();
                        }
                        beat.Chord = chord;
                    }

                    if ((beat.Flags & 0x04) > 0)
                        beat.Text = await _reader.ReadNextIntByteSizeStringAsync();

                    if ((beat.Flags & 0x08) > 0)
                    {
                        var beatEffect = new BeatEffect();

                        beatEffect.Flags1 = await _reader.ReadNextSignedByteAsync();
                        beatEffect.Flags2 = await _reader.ReadNextSignedByteAsync();

                        if ((beatEffect.Flags1 & 0x20) > 0)
                            beatEffect.SlapEffect = await _reader.ReadNextSignedByteAsync();

                        if ((beatEffect.Flags2 & 0x04) > 0)
                            beatEffect.TremoloBar = await ReadBendAsync();

                        if ((beatEffect.Flags1 & 0x40) > 0)
                            beatEffect.Stroke = await ReadBeatStrokeAsync();

                        if ((beatEffect.Flags2 & 0x02) > 0)
                            beatEffect.PickStroke = await _reader.ReadNextSignedByteAsync();

                        beat.BeatEffect = beatEffect;
                    }

                    if ((beat.Flags & 0x10) > 0)
                        beat.MixTableChange = await ReadMixTableChangeAsync();

                    measure.Beats.Add(beat);
                }

                _song.Measures.Add(measure);
            }
        }
    }

    private async Task<MixTableChange> ReadMixTableChangeAsync()
    {
        var mixTableChange = new MixTableChange();

        //TODO: implementation

        return mixTableChange;
    }

    private async Task<BeatStroke> ReadBeatStrokeAsync()
    {
        var beatStroke = new BeatStroke();

        beatStroke.StrokeDown = await _reader.ReadNextSignedByteAsync();
        beatStroke.StrokeUp = await _reader.ReadNextSignedByteAsync();

        return beatStroke;
    }

    private async Task<BendEffect> ReadBendAsync()
    {
        var bendEffect = new BendEffect();

        bendEffect.Type = await _reader.ReadNextSignedByteAsync();
        bendEffect.Value = await _reader.ReadNextIntAsync();
        bendEffect.PointCount = await _reader.ReadNextIntAsync();

        bendEffect.Points = new List<(int position, int value, bool vibrato)>();
        for (var i = 0; i < bendEffect.PointCount; i++)
        {
            var position = await _reader.ReadNextIntAsync();
            var value = await _reader.ReadNextIntAsync();
            var vibrato = await _reader.ReadNextBoolAsync();
            bendEffect.Points.Add((position, value, vibrato));
        }

        return bendEffect;
    }
}
