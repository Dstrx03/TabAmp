namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.BinaryPrimitives;

internal readonly struct Gp5Bool
{
    public const byte FalseValue = 0;
    public const byte TrueValue = 1;

    public byte ByteValue { get; }
    public bool BoolValue => ByteValue == TrueValue;

    public Gp5Bool(byte byteValue) =>
        ByteValue = byteValue;

    public Gp5Bool(bool boolValue) =>
        ByteValue = boolValue ? TrueValue : FalseValue;

    public static implicit operator bool(Gp5Bool boolWrapper) => boolWrapper.BoolValue;

    public static explicit operator Gp5Bool(byte byteValue) => new(byteValue);
    public static explicit operator Gp5Bool(bool boolValue) => new(boolValue);
}
