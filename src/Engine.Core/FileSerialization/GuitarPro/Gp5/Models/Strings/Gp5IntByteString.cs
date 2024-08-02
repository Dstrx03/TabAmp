namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Strings;

internal readonly struct Gp5IntByteString
{
    private const int LengthSize = 1;

    public string DecodedString { get; }
    public int Size { get; }
    public int MaxLength => Size - LengthSize;

    public Gp5IntByteString(string decodedString, int size)
    {
        DecodedString = decodedString;
        Size = size;
    }

    public static implicit operator string(Gp5IntByteString stringWrapper) => stringWrapper.DecodedString;
}
