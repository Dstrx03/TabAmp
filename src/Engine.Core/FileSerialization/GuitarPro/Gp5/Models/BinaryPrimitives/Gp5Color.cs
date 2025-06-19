namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.BinaryPrimitives;

internal readonly record struct Gp5Color(byte Red, byte Green, byte Blue, byte _A01)
{
    #region Unknown & Anonymous data
    public byte _A01 { get; init; } = _A01;
    #endregion
}
