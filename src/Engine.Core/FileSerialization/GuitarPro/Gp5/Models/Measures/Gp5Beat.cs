using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Measures;

internal class Gp5Beat
{
    public Primary PrimaryFlags { get; set; }
    public byte? Status { get; set; }
    public sbyte Duration { get; set; }
    public int? Tuplet { get; set; }
    public Gp5Chord? Chord { get; set; }
    public string? Text { get; set; }
    public Gp5BeatEffects? Effects { get; set; }
    public Gp5MixTable? MixTable { get; set; }
    public NotesPresence NotesPresenceFlags { get; set; }
    public Gp5Note[]? Notes { get; set; }
    public Secondary SecondaryFlags { get; set; }
    public byte? TODO { get; set; }


    [Flags]
    public enum Primary : byte
    {
        HasDot = 0x01,
        HasChord = 0x02,
        HasText = 0x04,
        HasEffects = 0x08,
        HasMixTable = 0x10,
        HasTuplet = 0x20,
        HasStatus = 0x40
    }

    [Flags]
    public enum NotesPresence : byte
    {
        HasSeventhStringNote = 0x01,
        HasSixthStringNote = 0x02,
        HasFifthStringNote = 0x04,
        HasFourthStringNote = 0x08,
        HasThirdStringNote = 0x10,
        HasSecondStringNote = 0x20,
        HasFirstStringNote = 0x40
    }

    [Flags]
    public enum Secondary : short
    {
        // TODO: transparent naming
        // TODO: manual QA
        Ottava_TODO = 0x0010,
        OttavaBassa_TODO = 0x0020,
        Quindicesima_TODO = 0x0040,
        QuindicesimaBassa_TODO = 0x0100,
        BreakBeam_TODO = 0x0001,
        ForceBeam_TODO = 0x0004,
        ForceBracket_TODO = 0x2000,
        BreakSecondaryTuplet_TODO = 0x1000,
        Down_TODO = 0x0002,
        Up_TODO = 0x0008,
        Start_TODO = 0x0200,
        End_TODO = 0x0400,
        BreakSecondary_TODO = 0x0800
    }
}
