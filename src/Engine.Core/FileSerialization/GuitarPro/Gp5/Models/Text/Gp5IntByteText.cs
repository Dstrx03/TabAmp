namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

internal readonly struct Gp5IntByteText
{
    private const int LengthByteSize = 1;

    public string DecodedString { get; }
    public int Size { get; }
    public int Length => DecodedString.Length;
    public int MaxLength => Size - LengthByteSize;

    public Gp5IntByteText(string decodedString, int size)
    {
        DecodedString = decodedString;
        Size = size;
    }

    public Gp5IntByteText(string decodedString)
    {
        DecodedString = decodedString;
        Size = decodedString.Length + LengthByteSize;
    }

    public static implicit operator string(Gp5IntByteText textWrapper) => textWrapper.DecodedString;
}
