using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5Beat
{
    public Primary PrimaryFlags { get; set; }
    public byte? Status { get; set; }
    public sbyte Duration { get; set; }
    public int? Tuplet { get; set; }
    public object? Chord_TODO { get; set; }
    public string? Text { get; set; }
    public object? Effect_TODO { get; set; }
    public object? MixTable_TODO { get; set; }
    public NotesPresence NotesPresenceFlags { get; set; }
    public Gp5Note[]? Notes { get; set; }
    public Secondary SecondaryFlags { get; set; }
    public byte? TODO { get; set; }


    [Flags]
    public enum Primary : byte
    {
        HasDot = 0x01,
        HasText = 0x04,
        HasTuplet = 0x20,
        HasStatus = 0x40,

        // TODO: naming
        Chord_TODO = 0x02,
        Effect_TODO = 0x08,
        MixTable_TODO = 0x10
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
        // TODO: naming
        TODO = 0x0800
    }
}
