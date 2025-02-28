namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.BinaryPrimitives;

internal readonly record struct Gp5Bool(byte Value)
{
    public const byte FalseValue = 0;
    public const byte TrueValue = 1;

    public Gp5Bool(bool value)
        : this(value ? TrueValue : FalseValue)
    {
    }

    public static implicit operator bool(Gp5Bool boolWrapper) => boolWrapper.Value == TrueValue;

    public static explicit operator Gp5Bool(byte value) => new(value);
    public static explicit operator Gp5Bool(bool value) => new(value);
}
