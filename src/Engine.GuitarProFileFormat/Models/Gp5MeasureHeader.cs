using System;

namespace TabAmp.Engine.GuitarProFileFormat.Models;

public class Gp5MeasureHeader
{
    public TodoFlags Flags { get; set; }// TODO: name, design
    public Gp5TimeSignature TimeSignature { get; set; }
    public byte? RepeatClose { get; set; } // TODO: name, design
    public Gp5Marker Marker { get; set; }

    [Flags]
    public enum TodoFlags : byte// TODO: name, design
    {
        TimeSignatureNumerator = 0x01,
        TimeSignatureDenominator = 0x02,
        RepeatClose = 0x08,
        Marker = 0x20,

        TimeSignature = TimeSignatureNumerator | TimeSignatureDenominator
    }
}
