namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Wrappers;

internal struct Gp5Boolean
{
    public const byte FalseValue = 0;
    public const byte TrueValue = 1;

    public byte ByteValue { get; }
    public bool BooleanValue => ByteValue == TrueValue;

    public Gp5Boolean(byte byteValue) =>
        ByteValue = byteValue;

    public static implicit operator bool(Gp5Boolean b) => b.BooleanValue;
}
