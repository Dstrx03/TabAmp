namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Strings;

internal readonly struct Gp5IntByteString
{
    private const int LengthByteSize = 1;

    public string DecodedString { get; }
    public int Size { get; }
    public int Length => DecodedString.Length;
    public int MaxLength => Size - LengthByteSize;

    public Gp5IntByteString(string decodedString, int size)
    {
        DecodedString = decodedString;
        Size = size;
    }

    public Gp5IntByteString(string decodedString)
    {
        DecodedString = decodedString;
        Size = decodedString.Length + LengthByteSize;
    }

    public static implicit operator string(Gp5IntByteString stringWrapper) => stringWrapper.DecodedString;
}
