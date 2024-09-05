using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5BeatEffect
{
    public Primary PrimaryFlags { get; set; }
    public Secondary SecondaryFlags { get; set; }
    public sbyte? SlapEffect_TODO { get; set; }
    public object? TremoloBar_TODO { get; set; }
    public sbyte? UpstrokeDuration { get; set; }
    public sbyte? DownstrokeDuration { get; set; }
    public sbyte? PickStroke { get; set; }


    [Flags]
    public enum Primary : byte
    {
        HasStroke = 0x40,

        // TODO: naming
        SlapEffect_TODO = 0x20
    }

    [Flags]
    public enum Secondary : byte
    {
        HasPickStroke = 0x02,

        // TODO: naming
        TremoloBar_TODO = 0x04
    }
}
