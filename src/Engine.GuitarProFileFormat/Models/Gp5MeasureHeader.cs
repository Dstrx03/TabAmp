using System;

namespace TabAmp.Engine.GuitarProFileFormat.Models;

public class Gp5MeasureHeader
{
    public byte? FirstBlankTodo { get; set; }// TODO: name
    public MeasureHeaderFlags Flags { get; set; }// TODO: flags naming convention
    public Gp5TimeSignature TimeSignature { get; set; }
    public byte? RepeatCount { get; set; }
    public Gp5Marker Marker { get; set; }
    public Gp5KeySignature KeySignature { get; set; }
    public AlternateEndingsFlags? AlternateEndings { get; set; }
    public byte? SecondBlankTodo { get; set; }// TODO: name
    public byte TripletFeel { get; set; }


    [Flags]
    public enum MeasureHeaderFlags : byte
    {
        HasTimeSignatureNumerator = 0x01,
        HasTimeSignatureDenominator = 0x02,
        HasRepeatOpen = 0x04,
        HasRepeatClose = 0x08,
        HasAlternateEndings = 0x10,
        HasMarker = 0x20,
        HasKeySignature = 0x40,
        HasDoubleBar = 0x80
    }

    [Flags]
    public enum AlternateEndingsFlags : byte
    {
        Ending1 = 0x01,
        Ending2 = 0x02,
        Ending3 = 0x04,
        Ending4 = 0x08,
        Ending5 = 0x10,
        Ending6 = 0x20,
        Ending7 = 0x40,
        Ending8 = 0x80
    }
}
