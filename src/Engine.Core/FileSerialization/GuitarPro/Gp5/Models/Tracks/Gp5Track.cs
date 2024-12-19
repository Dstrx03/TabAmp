using System;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Rse;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Tracks;

internal class Gp5Track
{
    public const int NameMaxLength = 40;
    public const int StringsTuningLength = 7;
    public const int RseEqualizerBandsCount = 3;

    public Primary PrimaryFlags { get; set; }
    public string Name { get; set; }
    public int StringsCount { get; set; }
    public int[] StringsTuning { get; set; }
    public int Port { get; set; }
    public int MainChannel { get; set; }
    public int EffectChannel { get; set; }
    public int FretsCount { get; set; }
    public int CapoPosition { get; set; }
    public int Color { get; set; }
    public Secondary SecondaryFlags { get; set; }
    public byte RseAutoAccentuation { get; set; }
    public byte MidiBank { get; set; }
    public byte RseHumanPlaying { get; set; }
    public Gp5RseInstrument RseInstrument { get; set; }
    public Gp5RseEqualizer RseEqualizer { get; set; }
    public Gp5RseInstrumentEffect RseInstrumentEffect { get; internal set; }


    [Flags]
    public enum Primary : byte
    {
        TrackTypePercussion = 0x01,
        Simulate12StringedGuitar = 0x02,
        ParticularForBanjo5thString = 0x04,
        _A01 = 0x08,
        TrackMixSolo = 0x10,
        TrackMixMute = 0x20,
        UseRse = 0x40,
        IndicateTuningOnTheScore = 0x80
    }

    [Flags]
    public enum Secondary : short
    {
        DisplayTablature = 0x0001,
        DisplayStandardNotation = 0x0002,
        DiagramsChordsBelowTheStandardNotation = 0x0004,
        ShowRhythmWithTab = 0x0008,
        ForceHorizontalBeams = 0x0010,
        ForceChannels11To16 = 0x0020,
        DiagramsListOnTopOfTheScore = 0x0040,
        DiagramsInTheScore = 0x0080,
        TrackTypeInstrument = 0x0100,
        AutoLetRing = 0x0200,
        AutoBrush = 0x0400,
        ExtendRhythmicInsideTheTab = 0x0800
    }

    #region Unknown & Anonymous data
    public int _A01 { get; set; }
    public int _A02 { get; set; }
    public int _A03 { get; set; }
    public byte _B01 { get; set; }
    public byte _B02 { get; set; }
    public byte _B03 { get; set; }
    public byte _B04 { get; set; }
    public byte _B05 { get; set; }
    public byte _B06 { get; set; }
    public byte _B07 { get; set; }
    public byte _B08 { get; set; }
    public byte _B09 { get; set; }
    public byte _B10 { get; set; }
    public short _C01 { get; set; }
    #endregion
}
