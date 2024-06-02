namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.BinaryPrimitives;

internal readonly struct Gp5Bool
{
    public const byte FalseValue = 0;
    public const byte TrueValue = 1;

    public byte ByteValue { get; }

    public Gp5Bool(byte byteValue) =>
        ByteValue = byteValue;

    public bool ToBool() => ByteValue == TrueValue;

    public static implicit operator bool(Gp5Bool boolValue) => boolValue.ToBool();

    public static explicit operator Gp5Bool(byte byteValue) => new(byteValue);
}
