using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5GraceNote
{
    public byte Fret { get; set; }
    public byte Dynamic { get; set; }
    public byte Transition { get; set; }
    public byte Duration { get; set; }
    public Primary PrimaryFlags { get; set; }


    [Flags]
    public enum Primary : byte
    {
        HasDeadNote = 0x01,
        PositionOnTheBeat = 0x02
    }
}
