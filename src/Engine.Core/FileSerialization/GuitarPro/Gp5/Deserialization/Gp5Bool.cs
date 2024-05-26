namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal readonly struct Gp5Bool
{
    public const byte FalseValue = 0;
    public const byte TrueValue = 1;

    public byte ByteValue { get; }

    public Gp5Bool(byte byteValue) =>
        ByteValue = byteValue;

    public static implicit operator bool(Gp5Bool boolValue) => boolValue.ByteValue == TrueValue;

    public static explicit operator Gp5Bool(byte byteValue) => new(byteValue);
}
