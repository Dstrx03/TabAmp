using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.BinaryPrimitives;

internal readonly record struct Gp5Color(byte Red, byte Green, byte Blue, byte _A01)
{
    public Gp5Color(ReadOnlySpan<byte> buffer)
        : this(Red: buffer[0], Green: buffer[1], Blue: buffer[2], _A01: buffer[3])
    {
    }

    public static explicit operator Gp5Color(ReadOnlySpan<byte> buffer) => new(buffer);


    #region Unknown & Anonymous data
    public byte _A01 { get; init; } = _A01;
    #endregion
}
