using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5Beat
{
    public Primary PrimaryFlags { get; set; }
    public byte? Status_TODO { get; set; }
    public sbyte Duration_TODO { get; set; }
    public object? Chord_TODO { get; set; }
    public string? Text { get; set; }
    public object? Effect_TODO { get; set; }
    public object? MixTable_TODO { get; set; }
    public NotesStrings_Todo NotesStringsFlags_TODO { get; set; }
    public Secondary SecondaryFlags { get; set; }
    public byte? TODO { get; set; }


    [Flags]
    public enum Primary : byte
    {
        HasText = 0x04,

        // TODO: naming
        Status_TODO = 0x40,
        Chord_TODO = 0x02,
        Effect_TODO = 0x08,
        MixTable_TODO = 0x10
    }

    [Flags]
    public enum NotesStrings_Todo : byte
    {
        // TODO: naming
        Sev = 0x01,
        Six = 0x02,
        Fiv = 0x04,
        Fou = 0x08,
        Thi = 0x10,
        Sec = 0x20,
        Fir = 0x40
    }

    [Flags]
    public enum Secondary : short
    {
        // TODO: naming
        TODO = 0x0800
    }
}
