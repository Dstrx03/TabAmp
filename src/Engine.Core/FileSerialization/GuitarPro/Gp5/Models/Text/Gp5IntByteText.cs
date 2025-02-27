namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

internal readonly record struct Gp5IntByteText(int Size, byte Length, string Text)
{
    private const int LengthByteSize = 1;

    public Gp5IntByteText(string text)
        : this(CalculateSize(text), (byte)text.Length, text)
    {
    }

    public static int CalculateSize(string text) => text.Length + LengthByteSize;

    public static implicit operator string(Gp5IntByteText textWrapper) => textWrapper.Text;
}
