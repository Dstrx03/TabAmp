using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5BeatEffects
{
    public Primary PrimaryFlags { get; set; }
    public Secondary SecondaryFlags { get; set; }
    public byte? TappingSlappingPopping { get; set; }
    public object? TremoloBar_TODO { get; set; }
    public byte? UpstrokeDuration { get; set; }
    public byte? DownstrokeDuration { get; set; }
    public byte? PickStroke { get; set; }


    [Flags]
    public enum Primary : byte
    {
        HasTappingSlappingPopping = 0x20,
        HasStroke = 0x40
    }

    [Flags]
    public enum Secondary : byte
    {
        HasPickStroke = 0x02,

        // TODO: naming
        TremoloBar_TODO = 0x04
    }
}
