namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal readonly struct Gp5Color
{
    public byte Red { get; }
    public byte Green { get; }
    public byte Blue { get; }

    public Gp5Color(byte[] buffer)
    {
        Red = buffer[0];
        Green = buffer[1];
        Blue = buffer[2];
        _A01 = buffer[3];
    }

    public static implicit operator int(Gp5Color colorValue) => (colorValue.Red << 16) | (colorValue.Green << 8) | (colorValue.Blue);


    #region Unknown & Anonymous data
    public byte _A01 { get; }
    #endregion
}
