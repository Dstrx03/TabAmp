using System;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.MusicalNotation;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Measures;

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
        HasHeavyAccentuatedNote = 0x02,
        HasGhostNote = 0x04,
        HasEffects = 0x08,
        HasDynamic = 0x10,
        _A01 = 0x20,
        HasAccentuatedNote = 0x40,
        HasFingering = 0x80
    }

    [Flags]
    public enum Secondary : byte
    {
        HasChangeAccidental = 0x02
    }
}
