namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

internal readonly record struct Gp5IntByteText(int Size, byte Length, string Text)
{
    public const int LengthByteSize = 1;

    public Gp5IntByteText(string text)
        : this(text.Length + LengthByteSize, (byte)text.Length, text)
    {
    }

    public static implicit operator string(Gp5IntByteText textWrapper) => textWrapper.Text;
}
