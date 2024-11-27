using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5BeatEffects
{
    public Primary PrimaryFlags { get; set; }
    public Secondary SecondaryFlags { get; set; }
    public byte? TappingSlappingPopping { get; set; }
    public object? TremoloBar { get; set; }
    public byte? UpstrokeDuration { get; set; }
    public byte? DownstrokeDuration { get; set; }
    public byte? PickStroke { get; set; }


    [Flags]
    public enum Primary : byte
    {
        HasVibrato = 0x01,
        HasWideVibrato = 0x02,
        HasNaturalHarmonic = 0x04,
        HasArtificialHarmonic = 0x08,
        HasFadeIn = 0x10,
        HasTappingSlappingPopping = 0x20,
        HasStroke = 0x40,
    }

    [Flags]
    public enum Secondary : byte
    {
        HasStrokeRasgueado = 0x01,
        HasPickStroke = 0x02,
        HasTremoloBar = 0x04
    }
}
