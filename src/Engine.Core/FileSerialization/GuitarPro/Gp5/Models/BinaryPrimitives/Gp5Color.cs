using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.BinaryPrimitives;

internal readonly struct Gp5Color
{
    public byte Red { get; }
    public byte Green { get; }
    public byte Blue { get; }

    public Gp5Color(byte red, byte green, byte blue, byte _a01)
    {
        Red = red;
        Green = green;
        Blue = blue;
        _A01 = _a01;
    }

    public static explicit operator Gp5Color(ReadOnlySpan<byte> buffer) =>
        new(red: buffer[0], green: buffer[1], blue: buffer[2], _a01: buffer[3]);


    #region Unknown & Anonymous data
    public byte _A01 { get; }
    #endregion
}
