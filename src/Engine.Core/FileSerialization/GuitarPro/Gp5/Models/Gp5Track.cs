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
    public int Unknown1_TODO { get; set; }
    public int Unknown2_TODO { get; set; }
    public int Unknown3_TODO { get; set; }
    public int Unknown4_TODO { get; set; }
    public int Unknown5_TODO { get; set; }
    public int Unknown6_TODO { get; set; }
    public int Instrument_TODO { get; set; }
    public int Unknown8_TODO { get; set; }
    public int SoundBank_TODO { get; set; }
    public int EffectNumber_TODO { get; set; }
    public Gp5RseEqualizer RseEqualizer { get; set; }
    public string RseEffect { get; set; }
    public string EffectCategory_TODO { get; set; }


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
}
