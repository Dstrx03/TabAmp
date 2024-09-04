using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5BeatEffect
{
    public Primary PrimaryFlags { get; set; }
    public Secondary SecondaryFlags { get; set; }
    public sbyte? SlapEffect_TODO { get; set; }
    public object? TremoloBar_TODO { get; set; }
    public sbyte? Upstroke_TODO { get; set; }
    public sbyte? Downstroke_TODO { get; set; }
    public sbyte? PickStroke_TODO { get; set; }


    [Flags]
    public enum Primary : byte
    {
        // TODO: naming
        SlapEffect_TODO = 0x20,
        Stroke_TODO = 0x40
    }

    [Flags]
    public enum Secondary : byte
    {
        // TODO: naming
        PickStroke_TODO = 0x02,
        TremoloBar_TODO = 0x04
    }
}
