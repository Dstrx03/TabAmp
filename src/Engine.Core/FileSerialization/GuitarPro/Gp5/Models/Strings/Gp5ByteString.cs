namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Strings;

internal readonly struct Gp5ByteString
{
    public string DecodedString { get; }
    public int TrailingBytesCount { get; }

    public Gp5ByteString(string decodedString, int maxLength)
    {
        DecodedString = decodedString;
        TrailingBytesCount = maxLength - decodedString.Length;
    }

    public static implicit operator string(Gp5ByteString stringValue) => stringValue.DecodedString;
}
