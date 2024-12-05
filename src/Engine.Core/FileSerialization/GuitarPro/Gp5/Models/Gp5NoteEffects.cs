using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5NoteEffects
{
    public Primary PrimaryFlags { get; set; }
    public Secondary SecondaryFlags { get; set; }
    public Gp5Bend? Bend { get; set; }
    public object? GraceNote { get; set; }
    public object? TremoloPicking { get; set; }
    public object? Slide { get; set; }
    public object? Harmonic { get; set; }
    public object? Trill { get; set; }


    [Flags]
    public enum Primary : byte
    {
        HasBend = 0x01,
        HasHammerOnPullOff = 0x02,
        HasLetRing = 0x08,
        HasGraceNote = 0x10
    }

    [Flags]
    public enum Secondary : byte
    {
        HasStaccato = 0x01,
        HasPalmMute = 0x02,
        HasTremoloPicking = 0x04,
        HasSlide = 0x08,
        HasHarmonic = 0x10,
        HasTrill = 0x20,
        HasVibrato = 0x40
    }
}
