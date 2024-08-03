namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.BinaryPrimitives;

internal readonly struct Gp5Color
{
    public byte Red { get; }
    public byte Green { get; }
    public byte Blue { get; }
    public int RgbInt => Red << 16 | Green << 8 | Blue;

    public Gp5Color(byte[] buffer)
    {
        Red = buffer[0];
        Green = buffer[1];
        Blue = buffer[2];
        _A01 = buffer[3];
    }

    public Gp5Color(int rgbInt)
    {
        Red = (byte)((rgbInt >> 16) & 0xFF);
        Green = (byte)((rgbInt >> 8) & 0xFF);
        Blue = (byte)(rgbInt & 0xFF);
        _A01 = 0;
    }

    public static implicit operator int(Gp5Color colorWrapper) => colorWrapper.RgbInt;

    public static explicit operator Gp5Color(byte[] buffer) => new(buffer);
    public static explicit operator Gp5Color(int rgbInt) => new(rgbInt);


    #region Unknown & Anonymous data
    public byte _A01 { get; }
    #endregion
}
