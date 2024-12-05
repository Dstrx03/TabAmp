using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;
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
        var tempo = await ReadTempoAsync();
        tempo.HideBeatsPerMinute = await _primitivesReader.ReadBoolAsync();

        return tempo;
    }

    private async ValueTask<Gp5Tempo> ReadTempoAsync()
    {
        return new Gp5Tempo
        {
            WordIndication = await _textReader.ReadIntByteTextAsync(),
            BeatsPerMinute = await _primitivesReader.ReadIntAsync()
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
            RseInstrument = await ReadRseInstrumentAsync(),
            RseEqualizer = await _rseEqualizerReader.ReadRseEqualizerAsync(Gp5Track.RseEqualizerBandsCount),
            RseInstrumentEffect = await ReadRseInstrumentEffectAsync()
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

        if (primaryFlags.HasFlag(Gp5Beat.Primary.HasChord))
            beat.Chord = await ReadChordAsync();

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

        // throw new NotImplementedException("TODO: complete beat reading, test flags.");

        beat.SecondaryFlags = (Gp5Beat.Secondary)await _primitivesReader.ReadShortAsync();

        if (beat.SecondaryFlags.HasFlag(Gp5Beat.Secondary.BreakSecondary_TODO))
        {
            beat.TODO = await _primitivesReader.ReadByteAsync();
            throw new NotImplementedException($"TODO: research unknown flag and data, value={beat.TODO}");
        }

        return beat;
    }

    private async ValueTask<Gp5Chord> ReadChordAsync()
    {
        // TODO: apply more transparent naming for Gp5Chord model properties
        // TODO: apply cleaner reading code
        // TODO: use consts
        // TODO: manual QA
        throw new NotImplementedException("TODO: complete chord reading.");

        var isNewFormat = await _primitivesReader.ReadBoolAsync();

        // TODO: move to the integrity validation layer
        if (!isNewFormat)
            throw new FileSerializationIntegrityException("Expected chord to have ~new~ format.");

        var chord = new Gp5Chord
        {
            Sharp = await _primitivesReader.ReadBoolAsync(),
            _A01 = await _primitivesReader.ReadByteAsync(),
            _A02 = await _primitivesReader.ReadByteAsync(),
            _A03 = await _primitivesReader.ReadByteAsync(),
            Root = await _primitivesReader.ReadByteAsync(),
            Type = await _primitivesReader.ReadByteAsync(),
            Extension = await _primitivesReader.ReadByteAsync(),
            Bass = await _primitivesReader.ReadIntAsync(),
            Tonality = await _primitivesReader.ReadIntAsync(),
            Add = await _primitivesReader.ReadBoolAsync(),
            Name = await _textReader.ReadByteTextAsync(22),
            FifthTonality = await _primitivesReader.ReadByteAsync(),
            NinthTonality = await _primitivesReader.ReadByteAsync(),
            EleventhTonality = await _primitivesReader.ReadByteAsync(),
            Fret = await _primitivesReader.ReadIntAsync()
        };

        var frets = new int[7];
        chord.Frets = frets;
        for (var i = 0; i < frets.Length; i++)
            frets[i] = await _primitivesReader.ReadIntAsync();

        chord.BarresCount = await _primitivesReader.ReadByteAsync();

        var barreFrets = new byte[5];
        chord.BarreFrets = barreFrets;
        for (var i = 0; i < barreFrets.Length; i++)
            barreFrets[i] = await _primitivesReader.ReadByteAsync();

        var barreStarts = new byte[5];
        chord.BarreStarts = barreStarts;
        for (var i = 0; i < barreStarts.Length; i++)
            barreStarts[i] = await _primitivesReader.ReadByteAsync();

        var barreEnds = new byte[5];
        chord.BarreEnds = barreEnds;
        for (var i = 0; i < barreEnds.Length; i++)
            barreEnds[i] = await _primitivesReader.ReadByteAsync();

        var omissions = new bool[7];
        chord.Omissions = omissions;
        for (var i = 0; i < omissions.Length; i++)
            omissions[i] = await _primitivesReader.ReadBoolAsync();

        chord._B01 = await _primitivesReader.ReadByteAsync();

        var fingerings = new sbyte[7];
        chord.Fingerings = fingerings;
        for (var i = 0; i < fingerings.Length; i++)
            fingerings[i] = await _primitivesReader.ReadSignedByteAsync();

        chord.Show = await _primitivesReader.ReadBoolAsync();

        return chord;
    }

    private async ValueTask<Gp5RseInstrument> ReadRseInstrumentAsync()
    {
        return new Gp5RseInstrument
        {
            Instrument = await _primitivesReader.ReadIntAsync(),
            _A01 = await _primitivesReader.ReadIntAsync(),
            SoundBank = await _primitivesReader.ReadIntAsync(),
            _B01 = await _primitivesReader.ReadIntAsync()
        };
    }

    private async ValueTask<Gp5RseInstrumentEffect> ReadRseInstrumentEffectAsync()
    {
        return new Gp5RseInstrumentEffect
        {
            Name = await _textReader.ReadIntByteTextAsync(),
            CategoryName = await _textReader.ReadIntByteTextAsync()
        };
    }

    private async ValueTask<Gp5MixTable> ReadMixTableAsync()
    {
        var mixTable = new Gp5MixTable
        {
            Instrument = await _primitivesReader.ReadSignedByteAsync(),
            RseInstrument = await ReadRseInstrumentAsync(),
            Volume = await _primitivesReader.ReadSignedByteAsync(),
            Balance = await _primitivesReader.ReadSignedByteAsync(),
            Chorus = await _primitivesReader.ReadSignedByteAsync(),
            Reverb = await _primitivesReader.ReadSignedByteAsync(),
            Phaser = await _primitivesReader.ReadSignedByteAsync(),
            Tremolo = await _primitivesReader.ReadSignedByteAsync(),
            Tempo = await ReadTempoAsync()
        };

        bool HasValueChange(int value) => value != -1;

        if (HasValueChange(mixTable.Volume))
            mixTable.VolumeTransition = await _primitivesReader.ReadByteAsync();

        if (HasValueChange(mixTable.Balance))
            mixTable.BalanceTransition = await _primitivesReader.ReadByteAsync();

        if (HasValueChange(mixTable.Chorus))
            mixTable.ChorusTransition = await _primitivesReader.ReadByteAsync();

        if (HasValueChange(mixTable.Reverb))
            mixTable.ReverbTransition = await _primitivesReader.ReadByteAsync();

        if (HasValueChange(mixTable.Phaser))
            mixTable.PhaserTransition = await _primitivesReader.ReadByteAsync();

        if (HasValueChange(mixTable.Tremolo))
            mixTable.TremoloTransition = await _primitivesReader.ReadByteAsync();

        if (HasValueChange(mixTable.Tempo.BeatsPerMinute))
        {
            mixTable.TempoTransition = await _primitivesReader.ReadByteAsync();
            mixTable.Tempo.HideBeatsPerMinute = await _primitivesReader.ReadBoolAsync();
        }

        mixTable.PrimaryFlags = (Gp5MixTable.Primary)await _primitivesReader.ReadByteAsync();
        mixTable.WahWah = await _primitivesReader.ReadSignedByteAsync();
        mixTable.RseInstrumentEffect = await ReadRseInstrumentEffectAsync();

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
            Type = await _primitivesReader.ReadByteAsync(),
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
        var primaryFlags = (Gp5Note.Primary)await _primitivesReader.ReadByteAsync();

        // TODO: move to the integrity validation layer
        if (!primaryFlags.HasFlag(Gp5Note.Primary._A01))
            throw new FileSerializationIntegrityException("note expected to have primary flag _A01");

        var note = new Gp5Note
        {
            PrimaryFlags = primaryFlags,
            Type = await _primitivesReader.ReadByteAsync()
        };

        if (primaryFlags.HasFlag(Gp5Note.Primary.HasDynamic))
            note.Dynamic = await _primitivesReader.ReadByteAsync();

        note.Fret = await _primitivesReader.ReadByteAsync();

        if (primaryFlags.HasFlag(Gp5Note.Primary.HasFingering))
        {
            note.LeftHandFingering = await _primitivesReader.ReadSignedByteAsync();
            note.RightHandFingering = await _primitivesReader.ReadSignedByteAsync();
        }

        if (primaryFlags.HasFlag(Gp5Note.Primary.HasSoundDuration))
            note.SoundDuration = await _primitivesReader.ReadDoubleAsync();

        note.SecondaryFlags = (Gp5Note.Secondary)await _primitivesReader.ReadByteAsync();

        if (primaryFlags.HasFlag(Gp5Note.Primary.HasEffects))
            note.Effects = await ReadNoteEffectsAsync();

        return note;
    }

    private async ValueTask<Gp5NoteEffects> ReadNoteEffectsAsync()
    {
        // TODO: test flags
        var primaryFlags = (Gp5NoteEffects.Primary)await _primitivesReader.ReadByteAsync();
        var secondaryFlags = (Gp5NoteEffects.Secondary)await _primitivesReader.ReadByteAsync();
        var noteEffects = new Gp5NoteEffects
        {
            PrimaryFlags = primaryFlags,
            SecondaryFlags = secondaryFlags
        };

        if (primaryFlags.HasFlag(Gp5NoteEffects.Primary.HasBend))
            noteEffects.Bend = await ReadBendAsync();

        if (primaryFlags.HasFlag(Gp5NoteEffects.Primary.HasGraceNote))
        {
            noteEffects.GraceNote = null;
            throw new NotImplementedException("TODO: read grace note.");
        }

        if (secondaryFlags.HasFlag(Gp5NoteEffects.Secondary.HasTremoloPicking))
        {
            noteEffects.TremoloPicking = null;
            throw new NotImplementedException("TODO: read tremolo picking.");
        }

        if (secondaryFlags.HasFlag(Gp5NoteEffects.Secondary.HasSlide))
        {
            noteEffects.Slide = null;
            throw new NotImplementedException("TODO: read slide.");
        }

        if (secondaryFlags.HasFlag(Gp5NoteEffects.Secondary.HasHarmonic))
        {
            noteEffects.Harmonic = null;
            throw new NotImplementedException("TODO: read harmonic.");
        }

        if (secondaryFlags.HasFlag(Gp5NoteEffects.Secondary.HasTrill))
        {
            noteEffects.Trill = null;
            throw new NotImplementedException("TODO: read trill.");
        }

        return noteEffects;
    }
}
