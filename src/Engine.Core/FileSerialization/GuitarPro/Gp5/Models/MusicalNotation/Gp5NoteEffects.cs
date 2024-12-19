using System;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Effects;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.MusicalNotation;

internal class Gp5NoteEffects
{
    public Primary PrimaryFlags { get; set; }
    public Secondary SecondaryFlags { get; set; }
    public Gp5Bend? Bend { get; set; }
    public Gp5GraceNote? GraceNote { get; set; }
    public byte? TremoloPicking { get; set; }
    public Slide? SlideFlags { get; set; }
    public Gp5Harmonic? Harmonic { get; set; }
    public byte? TrillFret { get; set; }
    public byte? TrillPeriod { get; set; }


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

    [Flags]
    public enum Slide : byte
    {
        HasShiftSlide = 0x01,
        HasLegatoSlide = 0x02,
        HasSlideOutDownwards = 0x04,
        HasSlideOutUpwards = 0x08,
        HasSlideInFromBelow = 0x10,
        HasSlideInFromAbove = 0x20
    }
}
