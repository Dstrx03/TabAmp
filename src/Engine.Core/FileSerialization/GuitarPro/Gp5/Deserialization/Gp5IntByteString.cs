namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

internal readonly struct Gp5IntByteString
{
    public const int LengthByteSize = 1;

    public string DecodedString { get; }
    public int MaxLength { get; }

    public Gp5IntByteString(string decodedString, int maxLength)
    {
        DecodedString = decodedString;
        MaxLength = maxLength;
    }

    public override string ToString() => DecodedString;

    public static implicit operator string(Gp5IntByteString stringValue) => stringValue.ToString();
}
