using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5Track
{
    public const int NameMaxLength = 40;
    public const int StringsTuningLength = 7;
    public const int RseEqualizerBandsCount = 3;

    public Primary PrimaryFlags { get; set; }
    public string Name { get; set; }
    public int StringsCount { get; set; }
    public int[] StringsTuning { get; } = new int[StringsTuningLength];
    public int Port { get; set; }
    public int MainChannel { get; set; }
    public int EffectChannel { get; set; }
    public int FretsCount { get; set; }
    public int CapoFret { get; set; }
    public int Color { get; set; }
    public Secondary SecondaryFlags { get; set; }
    public byte RseAutoAccentuation { get; set; }
    public byte MidiBank { get; set; }
    public byte RseHumanPlaying { get; set; }
    public int Instrument_TODO { get; set; }
    public int Unknown8_TODO { get; set; }
    public int RseSoundBank { get; set; }
    public int EffectNumber_TODO { get; set; }
    public Gp5RseEqualizer RseEqualizer { get; set; }
    public string RseEffectName { get; set; }
    public string RseEffectCategoryName { get; set; }


    [Flags]
    public enum Primary : byte
    {
        // TODO: names that make sense
        IsPercussionTrack = 0x01,
        Is12StringedGuitarTrack = 0x02,
        IsBanjoTrack = 0x04,
        IsVisible = 0x08,
        IsSolo = 0x10,
        IsMute = 0x20,
        UseRSE = 0x40,
        IndicateTuning = 0x80
    }

    [Flags]
    public enum Secondary : short
    {
        // TODO: names that make sense
        Tablature = 0x0001,
        Notation = 0x0002,
        DiagramsAreBelow = 0x0004,
        ShowRhythm = 0x0008,
        ForceHorizontal = 0x0010,
        ForceChannels = 0x0020,
        DiagramList = 0x0040,
        DiagramsInScore = 0x0080,
        Unknown0 = 0x0100,
        AutoLetRing = 0x0200,
        AutoBrush = 0x0400,
        ExtendRhythmic = 0x0800
    }

    #region Unknown & Anonymous data
    public int _A01 { get; set; }
    public int _A02 { get; set; }
    public int _A03 { get; set; }
    public byte _A04 { get; set; }
    public byte _A05 { get; set; }
    public byte _A06 { get; set; }
    public byte _A07 { get; set; }
    public byte _A08 { get; set; }
    public byte _A09 { get; set; }
    public byte _A10 { get; set; }
    public byte _A11 { get; set; }
    public byte _A12 { get; set; }
    public byte _A13 { get; set; }
    public short _A14 { get; set; }
    #endregion
}
