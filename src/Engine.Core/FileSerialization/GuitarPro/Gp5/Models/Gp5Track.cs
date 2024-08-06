using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5Track
{
    public const int NameStringMaxLength = 40;
    public const int TODO_TUNINGS_STRINGS = 7;

    public Primary PrimaryFlags { get; set; }
    public string Name { get; set; }

    public int StringsCount_TODO { get; set; }
    public int?[] StringsTunings_TODO { get; } = new int?[TODO_TUNINGS_STRINGS];

    public int Port { get; set; }
    public int MainChannel { get; set; }
    public int EffectChannel { get; set; }
    public int FretsCount { get; set; }


    [Flags]
    public enum Primary : byte
    {
        // TODO: correct names
        IsPercussionTrack = 0x01,
        Is12StringedGuitarTrack = 0x02,
        IsBanjoTrack = 0x04,
        IsVisible = 0x08,
        IsSolo = 0x10,
        IsMute = 0x20,
        UseRSE = 0x40,
        IndicateTuning = 0x80
    }
}
