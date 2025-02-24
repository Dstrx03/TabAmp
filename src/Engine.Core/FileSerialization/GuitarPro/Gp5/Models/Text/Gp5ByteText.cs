namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Text;

internal readonly record struct Gp5ByteText(byte Length, string Text, int MaxLength)
{
    public int TrailingBytesCount => MaxLength - Length;

    public Gp5ByteText(string text, int maxLength)
        : this((byte)text.Length, text, maxLength)
    {
    }

    public static implicit operator string(Gp5ByteText textWrapper) => textWrapper.Text;
}
