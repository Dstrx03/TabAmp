using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal class Gp5TodoReader : IGp5TodoReader
{
    private readonly IGp5BinaryPrimitivesReader _primitivesReader;
    private readonly IGp5TextReader _textReader;
    private readonly IGp5RseEqualizerReader _rseEqualizerReader;

    public Gp5TodoReader(IGp5BinaryPrimitivesReader primitivesReader, IGp5TextReader textReader,
        IGp5RseEqualizerReader rseEqualizerReader)
    {
        _primitivesReader = primitivesReader;
        _textReader = textReader;
        _rseEqualizerReader = rseEqualizerReader;
    }

    public ValueTask<Gp5ByteText> ReadVersionAsync() =>
        _textReader.ReadByteTextAsync(Gp5File.VersionMaxLength);

    public async ValueTask<Gp5ScoreInformation> ReadScoreInformationAsync()
    {
        var scoreInformation = new Gp5ScoreInformation
        {
            Title = await _textReader.ReadIntByteTextAsync(),
            Subtitle = await _textReader.ReadIntByteTextAsync(),
            Artist = await _textReader.ReadIntByteTextAsync(),
            Album = await _textReader.ReadIntByteTextAsync(),
            Words = await _textReader.ReadIntByteTextAsync(),
            Music = await _textReader.ReadIntByteTextAsync(),
            Copyright = await _textReader.ReadIntByteTextAsync(),
            Tab = await _textReader.ReadIntByteTextAsync(),
            Instructions = await _textReader.ReadIntByteTextAsync(),
            Notice = new string[await _primitivesReader.ReadIntAsync()]
        };

        for (var i = 0; i < scoreInformation.Notice.Length; i++)
        {
            scoreInformation.Notice[i] = await _textReader.ReadIntByteTextAsync();
        }

        return scoreInformation;
    }

    public async ValueTask<Gp5Lyrics> ReadLyricsAsync()
    {
        return new Gp5Lyrics
        {
            ApplyToTrack = await _primitivesReader.ReadIntAsync(),
            FirstLine = await ReadLyricsLineAsync(),
            SecondLine = await ReadLyricsLineAsync(),
            ThirdLine = await ReadLyricsLineAsync(),
            FourthLine = await ReadLyricsLineAsync(),
            FifthLine = await ReadLyricsLineAsync()
        };
    }

    private async ValueTask<Gp5LyricsLine> ReadLyricsLineAsync()
    {
        return new Gp5LyricsLine
        {
            StartFromBar = await _primitivesReader.ReadIntAsync(),
            Lyrics = await _textReader.ReadIntTextAsync()
        };
    }

    public async ValueTask<Gp5RseMasterEffect> ReadRseMasterEffectAsync()
    {
        return new Gp5RseMasterEffect
        {
            Volume = await _primitivesReader.ReadIntAsync(),
            _A01 = await _primitivesReader.ReadIntAsync(),
            Equalizer = await _rseEqualizerReader.ReadRseEqualizerAsync(Gp5RseMasterEffect.EqualizerBandsCount)
        };
    }

    public async ValueTask<Gp5PageSetup> ReadPageSetupAsync()
    {
        return new Gp5PageSetup
        {
            Width = await _primitivesReader.ReadIntAsync(),
            Height = await _primitivesReader.ReadIntAsync(),
            MarginLeft = await _primitivesReader.ReadIntAsync(),
            MarginRight = await _primitivesReader.ReadIntAsync(),
            MarginTop = await _primitivesReader.ReadIntAsync(),
            MarginBottom = await _primitivesReader.ReadIntAsync(),
            ScoreSizeProportion = await _primitivesReader.ReadIntAsync(),
            HeaderAndFooterFlags = (Gp5PageSetup.HeaderAndFooter)await _primitivesReader.ReadShortAsync(),
            Title = await _textReader.ReadIntByteTextAsync(),
            Subtitle = await _textReader.ReadIntByteTextAsync(),
            Artist = await _textReader.ReadIntByteTextAsync(),
            Album = await _textReader.ReadIntByteTextAsync(),
            Words = await _textReader.ReadIntByteTextAsync(),
            Music = await _textReader.ReadIntByteTextAsync(),
            WordsAndMusic = await _textReader.ReadIntByteTextAsync(),
            CopyrightFirstLine = await _textReader.ReadIntByteTextAsync(),
            CopyrightSecondLine = await _textReader.ReadIntByteTextAsync(),
            PageNumber = await _textReader.ReadIntByteTextAsync()
        };
    }

    public async ValueTask<Gp5Tempo> ReadHeaderTempoAsync()
    {
        return new Gp5Tempo
        {
            WordIndication = await _textReader.ReadIntByteTextAsync(),
            BeatsPerMinute = await _primitivesReader.ReadIntAsync(),
            HideBeatsPerMinute = await _primitivesReader.ReadBoolAsync()
        };
    }

    public async ValueTask<Gp5HeaderKeySignature> ReadHeaderKeySignatureAsync()
    {
        return new Gp5HeaderKeySignature
        {
            Key = await _primitivesReader.ReadSignedByteAsync(),
            _A01 = await _primitivesReader.ReadSignedByteAsync(),
            _A02 = await _primitivesReader.ReadSignedByteAsync(),
            _A03 = await _primitivesReader.ReadSignedByteAsync(),
            Octave = await _primitivesReader.ReadSignedByteAsync()
        };
    }

    public async ValueTask<Gp5MidiChannel> ReadMidiChannelAsync()
    {
        return new Gp5MidiChannel
        {
            Instrument = await _primitivesReader.ReadIntAsync(),
            Volume = await _primitivesReader.ReadByteAsync(),
            Balance = await _primitivesReader.ReadByteAsync(),
            Chorus = await _primitivesReader.ReadByteAsync(),
            Reverb = await _primitivesReader.ReadByteAsync(),
            Phaser = await _primitivesReader.ReadByteAsync(),
            Tremolo = await _primitivesReader.ReadByteAsync(),
            _A01 = await _primitivesReader.ReadByteAsync(),
            _A02 = await _primitivesReader.ReadByteAsync()
        };
    }

    public async ValueTask<Gp5MusicalDirections> ReadMusicalDirectionsAsync()
    {
        return new Gp5MusicalDirections
        {
            Coda = await _primitivesReader.ReadShortAsync(),
            DoubleCoda = await _primitivesReader.ReadShortAsync(),
            Segno = await _primitivesReader.ReadShortAsync(),
            SegnoSegno = await _primitivesReader.ReadShortAsync(),
            Fine = await _primitivesReader.ReadShortAsync(),
            DaCapo = await _primitivesReader.ReadShortAsync(),
            DaCapoAlCoda = await _primitivesReader.ReadShortAsync(),
            DaCapoAlDoubleCoda = await _primitivesReader.ReadShortAsync(),
            DaCapoAlFine = await _primitivesReader.ReadShortAsync(),
            DaSegno = await _primitivesReader.ReadShortAsync(),
            DaSegnoAlCoda = await _primitivesReader.ReadShortAsync(),
            DaSegnoAlDoubleCoda = await _primitivesReader.ReadShortAsync(),
            DaSegnoAlFine = await _primitivesReader.ReadShortAsync(),
            DaSegnoSegno = await _primitivesReader.ReadShortAsync(),
            DaSegnoSegnoAlCoda = await _primitivesReader.ReadShortAsync(),
            DaSegnoSegnoAlDoubleCoda = await _primitivesReader.ReadShortAsync(),
            DaSegnoSegnoAlFine = await _primitivesReader.ReadShortAsync(),
            DaCoda = await _primitivesReader.ReadShortAsync(),
            DaDoubleCoda = await _primitivesReader.ReadShortAsync()
        };
    }

    public ValueTask<int> ReadRseMasterEffectReverbAsync() =>
        _primitivesReader.ReadIntAsync();

    public async ValueTask<(int measureHeadersCount, int tracksCount)> ReadMeasureHeadersAndTracksCountAsync() =>
        (measureHeadersCount: await _primitivesReader.ReadIntAsync(), tracksCount: await _primitivesReader.ReadIntAsync());

    public async ValueTask<Gp5MeasureHeader> ReadMeasureHeaderAsync(bool isFirst)
    {
        var primaryFlags = (Gp5MeasureHeader.Primary)await _primitivesReader.ReadByteAsync();
        var measureHeader = new Gp5MeasureHeader
        {
            PrimaryFlags = primaryFlags
        };

        var hasTimeSignature = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasTimeSignature);
        var hasDenominator = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasTimeSignatureDenominator);

        if (hasTimeSignature)
            measureHeader.TimeSignature = await ReadTimeSignatureAsync(hasDenominator);

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasRepeatClose))
            measureHeader.RepeatsCount = await _primitivesReader.ReadByteAsync();

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasMarker))
            measureHeader.Marker = await ReadMarkerAsync();

        if (primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasKeySignature))
            measureHeader.KeySignature = await ReadKeySignatureAsync();

        var hasAlternateEndings = primaryFlags.HasFlag(Gp5MeasureHeader.Primary.HasAlternateEndings);
        if (isFirst)
        {
            if (hasTimeSignature)
                measureHeader.TimeSignature!.BeamGroups = await ReadTimeSignatureBeamGroupsAsync();

            if (hasAlternateEndings)
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _primitivesReader.ReadByteAsync();
        }
        else
        {
            if (hasAlternateEndings)
                measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _primitivesReader.ReadByteAsync();

            if (hasTimeSignature)
                measureHeader.TimeSignature!.BeamGroups = await ReadTimeSignatureBeamGroupsAsync();
        }

        if (!hasAlternateEndings)
            measureHeader.AlternateEndingsFlags = (Gp5MeasureHeader.AlternateEndings)await _primitivesReader.ReadByteAsync();

        measureHeader.TripletFeel = await _primitivesReader.ReadByteAsync();
        measureHeader._A01 = await _primitivesReader.ReadByteAsync();

        return measureHeader;
    }

    private async ValueTask<Gp5KeySignature> ReadKeySignatureAsync()
    {
        return new Gp5KeySignature
        {
            Key = await _primitivesReader.ReadSignedByteAsync(),
            IsMinorKey = await _primitivesReader.ReadBoolAsync()
        };
    }

    private async ValueTask<Gp5TimeSignature> ReadTimeSignatureAsync(bool hasDenominator)
    {
        return new Gp5TimeSignature
        {
            Numerator = await _primitivesReader.ReadByteAsync(),
            Denominator = hasDenominator ? await _primitivesReader.ReadByteAsync() : null
        };
    }

    private async ValueTask<Gp5TimeSignatureBeamGroups> ReadTimeSignatureBeamGroupsAsync()
    {
        return new Gp5TimeSignatureBeamGroups
        {
            FirstSpan = await _primitivesReader.ReadByteAsync(),
            SecondSpan = await _primitivesReader.ReadByteAsync(),
            ThirdSpan = await _primitivesReader.ReadByteAsync(),
            FourthSpan = await _primitivesReader.ReadByteAsync()
        };
    }

    private async ValueTask<Gp5Marker> ReadMarkerAsync()
    {
        return new Gp5Marker
        {
            Name = await _textReader.ReadIntByteTextAsync(),
            Color = await _primitivesReader.ReadColorAsync()
        };
    }

    public async ValueTask<Gp5Track> ReadTrackAsync()
    {
        return new Gp5Track
        {
            PrimaryFlags = (Gp5Track.Primary)await _primitivesReader.ReadByteAsync(),
            Name = await _textReader.ReadByteTextAsync(Gp5Track.NameMaxLength),
            StringsCount = await _primitivesReader.ReadIntAsync(),
            StringsTuning = await ReadStringsTuningAsync(),
            Port = await _primitivesReader.ReadIntAsync(),
            MainChannel = await _primitivesReader.ReadIntAsync(),
            EffectChannel = await _primitivesReader.ReadIntAsync(),
            FretsCount = await _primitivesReader.ReadIntAsync(),
            CapoPosition = await _primitivesReader.ReadIntAsync(),
            Color = await _primitivesReader.ReadColorAsync(),
            SecondaryFlags = (Gp5Track.Secondary)await _primitivesReader.ReadShortAsync(),
            RseAutoAccentuation = await _primitivesReader.ReadByteAsync(),
            MidiBank = await _primitivesReader.ReadByteAsync(),
            RseHumanPlaying = await _primitivesReader.ReadByteAsync(),
            _A01 = await _primitivesReader.ReadIntAsync(),
            _A02 = await _primitivesReader.ReadIntAsync(),
            _A03 = await _primitivesReader.ReadIntAsync(),
            _B01 = await _primitivesReader.ReadByteAsync(),
            _B02 = await _primitivesReader.ReadByteAsync(),
            _B03 = await _primitivesReader.ReadByteAsync(),
            _B04 = await _primitivesReader.ReadByteAsync(),
            _B05 = await _primitivesReader.ReadByteAsync(),
            _B06 = await _primitivesReader.ReadByteAsync(),
            _B07 = await _primitivesReader.ReadByteAsync(),
            _B08 = await _primitivesReader.ReadByteAsync(),
            _B09 = await _primitivesReader.ReadByteAsync(),
            _B10 = await _primitivesReader.ReadByteAsync(),
            _C01 = await _primitivesReader.ReadShortAsync(),
            RseInstrument = await _primitivesReader.ReadIntAsync(),
            _D01 = await _primitivesReader.ReadIntAsync(),
            RseSoundBank = await _primitivesReader.ReadIntAsync(),
            _E01 = await _primitivesReader.ReadIntAsync(),
            RseEqualizer = await _rseEqualizerReader.ReadRseEqualizerAsync(Gp5Track.RseEqualizerBandsCount),
            RseEffectName = await _textReader.ReadIntByteTextAsync(),
            RseEffectCategoryName = await _textReader.ReadIntByteTextAsync()
        };
    }

    private async ValueTask<int[]> ReadStringsTuningAsync()
    {
        var stringsTuning = new int[Gp5Track.StringsTuningLength];
        for (var i = 0; i < stringsTuning.Length; i++)
        {
            stringsTuning[i] = await _primitivesReader.ReadIntAsync();
        }

        return stringsTuning;
    }

    public ValueTask<byte> ReadMeasureBreakLineAsync() =>
        _primitivesReader.ReadByteAsync();

    public ValueTask<int> ReadMeasureBeatsCountAsync() =>
        _primitivesReader.ReadIntAsync();

    public async ValueTask<Gp5Beat> ReadBeatAsync()
    {
        var primaryFlags = (Gp5Beat.Primary)await _primitivesReader.ReadByteAsync();
        var beat = new Gp5Beat
        {
            PrimaryFlags = primaryFlags,
        };

        if (primaryFlags.HasFlag(Gp5Beat.Primary.HasStatus))
            beat.Status = await _primitivesReader.ReadByteAsync();

        beat.Duration = await _primitivesReader.ReadSignedByteAsync();

        if (primaryFlags.HasFlag(Gp5Beat.Primary.HasTuplet))
            beat.Tuplet = await _primitivesReader.ReadIntAsync();

        if (primaryFlags.HasFlag(Gp5Beat.Primary.Chord_TODO))
        {
            beat.Chord_TODO = null;
            throw new NotImplementedException("TODO: read chord");
        }

        if (primaryFlags.HasFlag(Gp5Beat.Primary.HasText))
            beat.Text = await _textReader.ReadIntByteTextAsync();

        if (primaryFlags.HasFlag(Gp5Beat.Primary.HasEffects))
            beat.Effects = await ReadBeatEffectsAsync();

        if (primaryFlags.HasFlag(Gp5Beat.Primary.HasMixTable))
            beat.MixTable = await ReadMixTableAsync();

        beat.NotesPresenceFlags = (Gp5Beat.NotesPresence)await _primitivesReader.ReadByteAsync();

        var notesCount =
            Convert.ToByte(beat.NotesPresenceFlags.HasFlag(Gp5Beat.NotesPresence.HasFirstStringNote)) +
            Convert.ToByte(beat.NotesPresenceFlags.HasFlag(Gp5Beat.NotesPresence.HasSecondStringNote)) +
            Convert.ToByte(beat.NotesPresenceFlags.HasFlag(Gp5Beat.NotesPresence.HasThirdStringNote)) +
            Convert.ToByte(beat.NotesPresenceFlags.HasFlag(Gp5Beat.NotesPresence.HasFourthStringNote)) +
            Convert.ToByte(beat.NotesPresenceFlags.HasFlag(Gp5Beat.NotesPresence.HasFifthStringNote)) +
            Convert.ToByte(beat.NotesPresenceFlags.HasFlag(Gp5Beat.NotesPresence.HasSixthStringNote)) +
            Convert.ToByte(beat.NotesPresenceFlags.HasFlag(Gp5Beat.NotesPresence.HasSeventhStringNote));

        if (notesCount > 0)
        {
            beat.Notes = new Gp5Note[notesCount];
            for (var i = 0; i < beat.Notes.Length; i++)
            {
                beat.Notes[i] = await ReadNoteAsync();
            }
        }

        beat.SecondaryFlags = (Gp5Beat.Secondary)await _primitivesReader.ReadShortAsync();

        if (beat.SecondaryFlags.HasFlag(Gp5Beat.Secondary.TODO))
        {
            beat.TODO = await _primitivesReader.ReadByteAsync();
            throw new NotImplementedException($"TODO: research unknown flag and data, value={beat.TODO}");
        }

        return beat;
    }

    private async ValueTask<Gp5MixTable> ReadMixTableAsync()
    {
        var mixTable = new Gp5MixTable
        {
        };

        var instrument = await _primitivesReader.ReadSignedByteAsync();

        // TODO: exact sequence as in ReadTrackAsync() #1
        var RseInstrument = await _primitivesReader.ReadIntAsync();
        var _D01 = await _primitivesReader.ReadIntAsync();
        var RseSoundBank = await _primitivesReader.ReadIntAsync();
        var _E01 = await _primitivesReader.ReadIntAsync();
        // TODO: *********************************

        var volume = await _primitivesReader.ReadSignedByteAsync();
        var balance = await _primitivesReader.ReadSignedByteAsync();
        var chorus = await _primitivesReader.ReadSignedByteAsync();
        var reverb = await _primitivesReader.ReadSignedByteAsync();
        var phaser = await _primitivesReader.ReadSignedByteAsync();
        var tremolo = await _primitivesReader.ReadSignedByteAsync();

        // TODO: almost same signature as in ReadHeaderTempoAsync()
        var tempoName = await _textReader.ReadIntByteTextAsync();
        var tempo = await _primitivesReader.ReadIntAsync();
        // *******************************************************

        // Transitions
        byte? volumeTransition = null;
        if (volume != -1)
            volumeTransition = await _primitivesReader.ReadByteAsync();

        byte? balanceTransition = null;
        if (balance != -1)
            balanceTransition = await _primitivesReader.ReadByteAsync();

        byte? chorusTransition = null;
        if (chorus != -1)
            chorusTransition = await _primitivesReader.ReadByteAsync();

        byte? reverbTransition = null;
        if (reverb != -1)
            reverbTransition = await _primitivesReader.ReadByteAsync();

        byte? phaserTransition = null;
        if (phaser != -1)
            phaserTransition = await _primitivesReader.ReadByteAsync();

        byte? tremoloTransition = null;
        if (tremolo != -1)
            tremoloTransition = await _primitivesReader.ReadByteAsync();

        byte? tempoTransition = null;
        bool? hideTempo = null;
        if (tempo != -1)
        {
            tempoTransition = await _primitivesReader.ReadByteAsync();
            hideTempo = await _primitivesReader.ReadBoolAsync();
        }
        // *********************************************

        var flags = await _primitivesReader.ReadByteAsync();

        var wahEffect = await _primitivesReader.ReadSignedByteAsync();

        // TODO: exact sequence as in ReadTrackAsync() #2
        var RseEffectName = await _textReader.ReadIntByteTextAsync();
        var RseEffectCategoryName = await _textReader.ReadIntByteTextAsync();
        // TODO: *********************************

        return mixTable;
    }

    private async ValueTask<Gp5BeatEffects> ReadBeatEffectsAsync()
    {
        var primaryFlags = (Gp5BeatEffects.Primary)await _primitivesReader.ReadByteAsync();
        var secondaryFlags = (Gp5BeatEffects.Secondary)await _primitivesReader.ReadByteAsync();
        var beatEffects = new Gp5BeatEffects
        {
            PrimaryFlags = primaryFlags,
            SecondaryFlags = secondaryFlags
        };

        if (primaryFlags.HasFlag(Gp5BeatEffects.Primary.HasTappingSlappingPopping))
            beatEffects.TappingSlappingPopping = await _primitivesReader.ReadByteAsync();

        if (secondaryFlags.HasFlag(Gp5BeatEffects.Secondary.HasTremoloBar))
            beatEffects.TremoloBar = await ReadBendAsync();

        if (primaryFlags.HasFlag(Gp5BeatEffects.Primary.HasStroke))
        {
            beatEffects.UpstrokeDuration = await _primitivesReader.ReadByteAsync();
            beatEffects.DownstrokeDuration = await _primitivesReader.ReadByteAsync();
        }

        if (secondaryFlags.HasFlag(Gp5BeatEffects.Secondary.HasPickStroke))
            beatEffects.PickStroke = await _primitivesReader.ReadByteAsync();

        return beatEffects;
    }

    private async ValueTask<Gp5Bend> ReadBendAsync()
    {
        var bend = new Gp5Bend
        {
            Shape = await _primitivesReader.ReadByteAsync(),
            PitchShift = await _primitivesReader.ReadIntAsync(),
            Points = new (int, int, byte)[await _primitivesReader.ReadIntAsync()]
        };

        for (var i = 0; i < bend.Points.Length; i++)
        {
            var timePosition = await _primitivesReader.ReadIntAsync();
            var pitchShift = await _primitivesReader.ReadIntAsync();
            var vibrato = await _primitivesReader.ReadByteAsync();

            bend.Points[i] = (timePosition, pitchShift, vibrato);
        }

        return bend;
    }

    private async ValueTask<Gp5Note> ReadNoteAsync()
    {
        throw new NotImplementedException("TODO: read note");
        return new Gp5Note();
    }
}
