namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

internal readonly struct Gp5ByteText
{
    public string DecodedString { get; }
    public int MaxLength { get; }
    public int Length => DecodedString.Length;
    public int TrailingBytesCount => MaxLength - Length;

    public Gp5ByteText(string decodedString, int maxLength)
    {
        DecodedString = decodedString;
        MaxLength = maxLength;
    }

    public static implicit operator string(Gp5ByteText textWrapper) => textWrapper.DecodedString;
}
