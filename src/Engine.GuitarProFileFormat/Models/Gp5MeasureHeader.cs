using System;

namespace TabAmp.Engine.GuitarProFileFormat.Models;

public class Gp5MeasureHeader
{
    public byte? FirstBlankTodo { get; set; }// TODO: name
    public SomeFlags Flags { get; set; }// TODO: name, design
    public Gp5TimeSignature TimeSignature { get; set; }
    public byte? RepeatCount { get; set; }
    public Gp5Marker Marker { get; set; }
    public Gp5KeySignature KeySignature { get; set; }
    public byte? RepeatAltTodo { get; set; }// TODO: unpack?, name
    public byte? SecondBlankTodo { get; set; }// TODO: name
    public byte TripletFeel { get; set; }


    [Flags]
    public enum SomeFlags : byte// TODO: name, design
    {
        TimeSignatureNumerator = 0x01,
        TimeSignatureDenominator = 0x02,
        IsRepeatOpen = 0x04,
        RepeatClose = 0x08,
        RepeatAltTodo = 0x10,
        Marker = 0x20,
        KeySignature = 0x40,
        HasDoubleBar = 0x80
    }
}
