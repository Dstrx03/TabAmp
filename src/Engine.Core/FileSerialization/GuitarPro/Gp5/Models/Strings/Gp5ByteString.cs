namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Strings;

internal readonly struct Gp5ByteString
{
    public string DecodedString { get; }
    public int MaxLength { get; }
    public int Length => DecodedString.Length;
    public int TrailingBytesCount => MaxLength - Length;

    public Gp5ByteString(string decodedString, int maxLength)
    {
        DecodedString = decodedString;
        MaxLength = maxLength;
    }

    public static implicit operator string(Gp5ByteString stringWrapper) => stringWrapper.DecodedString;
}
