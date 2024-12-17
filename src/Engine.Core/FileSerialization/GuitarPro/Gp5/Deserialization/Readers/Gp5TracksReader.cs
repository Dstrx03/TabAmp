using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization.Readers;

internal class Gp5TracksReader : IGp5TracksReader
{
    private readonly IGp5BinaryPrimitivesReader _primitivesReader;
    private readonly IGp5TextReader _textReader;
    private readonly IGp5MusicalNotationReader _notationReader;
    private readonly IGp5RseReader _rseReader;

    public Gp5TracksReader(IGp5BinaryPrimitivesReader primitivesReader, IGp5TextReader textReader,
        IGp5MusicalNotationReader notationReader, IGp5RseReader rseReader) =>
        (_primitivesReader, _textReader, _notationReader, _rseReader) = (primitivesReader, textReader, notationReader, rseReader);

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
            RseInstrument = await _rseReader.ReadRseInstrumentAsync(),
            RseEqualizer = await _rseReader.ReadRseEqualizerAsync(Gp5Track.RseEqualizerBandsCount),
            RseInstrumentEffect = await _rseReader.ReadRseInstrumentEffectAsync()
        };

        async ValueTask<int[]> ReadStringsTuningAsync()
        {
            var stringsTuning = new int[Gp5Track.StringsTuningLength];
            for (var i = 0; i < stringsTuning.Length; i++)
            {
                stringsTuning[i] = await _primitivesReader.ReadIntAsync();
            }

            return stringsTuning;
        }
    }

    public async ValueTask<Gp5MixTable> ReadMixTableAsync()
    {
        var mixTable = new Gp5MixTable
        {
            Instrument = await _primitivesReader.ReadSignedByteAsync(),
            RseInstrument = await _rseReader.ReadRseInstrumentAsync(),
            Volume = await _primitivesReader.ReadSignedByteAsync(),
            Balance = await _primitivesReader.ReadSignedByteAsync(),
            Chorus = await _primitivesReader.ReadSignedByteAsync(),
            Reverb = await _primitivesReader.ReadSignedByteAsync(),
            Phaser = await _primitivesReader.ReadSignedByteAsync(),
            Tremolo = await _primitivesReader.ReadSignedByteAsync(),
            Tempo = await _notationReader.ReadTempoAsync()
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
            mixTable.Tempo.HideBeatsPerMinute = await _notationReader.ReadTempoHideBeatsPerMinuteAsync();
        }

        mixTable.PrimaryFlags = (Gp5MixTable.Primary)await _primitivesReader.ReadByteAsync();
        mixTable.WahWah = await _primitivesReader.ReadSignedByteAsync();
        mixTable.RseInstrumentEffect = await _rseReader.ReadRseInstrumentEffectAsync();

        return mixTable;
    }

    public async ValueTask<Gp5RseMasterEffect> ReadRseMasterEffectAsync()
    {
        return new Gp5RseMasterEffect
        {
            Volume = await _primitivesReader.ReadIntAsync(),
            _A01 = await _primitivesReader.ReadIntAsync(),
            Equalizer = await _rseReader.ReadRseEqualizerAsync(Gp5RseMasterEffect.EqualizerBandsCount)
        };
    }

    public ValueTask<int> ReadRseMasterEffectReverbAsync() =>
        _primitivesReader.ReadIntAsync();
}
