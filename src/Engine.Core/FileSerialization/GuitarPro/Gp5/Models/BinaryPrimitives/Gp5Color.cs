using System;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.BinaryPrimitives;

internal readonly record struct Gp5Color(byte Red, byte Green, byte Blue, byte _A01)
{
    private const int RedPosition = 0, GreenPosition = 1, BluePosition = 2, _A01Position = 3;

    public static Gp5Color Read(ReadOnlySpan<byte> source) => new()
    {
        Red = source[RedPosition],
        Green = source[GreenPosition],
        Blue = source[BluePosition],
        _A01 = source[_A01Position]
    };

    public static void Write(Span<byte> destination, Gp5Color color)
    {
        destination[RedPosition] = color.Red;
        destination[GreenPosition] = color.Green;
        destination[BluePosition] = color.Blue;
        destination[_A01Position] = color._A01;
    }


    #region Unknown & Anonymous data
    public byte _A01 { get; init; } = _A01;
    #endregion
}
