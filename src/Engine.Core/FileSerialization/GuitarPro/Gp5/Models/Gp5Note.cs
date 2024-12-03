using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5Note
{
    public Primary PrimaryFlags { get; set; }
    public byte? Type_TODO { get; set; }
    public sbyte? Dynamic_TODO { get; set; }
    public sbyte? Fret_TODO { get; set; }
    public sbyte? LeftHandFinger_TODO { get; set; }
    public sbyte? RightHandFinger_TODO { get; set; }
    public double? DurationPercent_TODO { get; set; }
    public Secondary SecondaryFlags { get; set; }
    public object? Effects_TODO { get; set; }


    [Flags]
    public enum Primary : byte
    {
        // TODO: transparent naming
        // TODO: manual QA
        heavyAccentuatedNote_TODO = 0x02,
        ghostNote_TODO = 0x04,
        accentuatedNote_TODO = 0x40,
        nonEmpty_TODO = 0x20,
        dynamic_TODO = 0x10,
        fingering_TODO = 0x80,
        soundDuration_TODO = 0x01,
        hasEffects_TODO = 0x08
    }

    [Flags]
    public enum Secondary : byte
    {
        // TODO: transparent naming
        // TODO: manual QA
        swapAccidentals_TODO = 0x02
    }
}
