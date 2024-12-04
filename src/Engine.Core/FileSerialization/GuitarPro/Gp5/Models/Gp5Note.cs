using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5Note
{
    public Primary PrimaryFlags { get; set; }
    public byte Type { get; set; }
    public byte? Dynamic { get; set; }
    public byte Fret { get; set; }
    public sbyte? LeftHandFingering { get; set; }
    public sbyte? RightHandFingering { get; set; }
    public double? SoundDuration { get; set; }
    public Secondary SecondaryFlags { get; set; }
    public Gp5NoteEffects? Effects { get; set; }


    [Flags]
    public enum Primary : byte
    {
        HasSoundDuration = 0x01,
        HasEffects = 0x08,
        HasDynamic = 0x10,
        _A01 = 0x20,
        HasFingering = 0x80,

        // TODO: transparent naming
        // TODO: manual QA
        heavyAccentuatedNote_TODO = 0x02,
        ghostNote_TODO = 0x04,
        accentuatedNote_TODO = 0x40,
    }

    [Flags]
    public enum Secondary : byte
    {
        // TODO: transparent naming
        // TODO: manual QA
        swapAccidentals_TODO = 0x02
    }

    public string primaryFlags { get; internal set; }
    public string secondaryFlags { get; internal set; }
}
