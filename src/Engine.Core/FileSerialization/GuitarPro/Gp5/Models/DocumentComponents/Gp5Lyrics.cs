namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.DocumentComponents;

internal class Gp5Lyrics
{
    public const int LinesLength = 5;

    public int ApplyToTrack { get; set; }
    public (int startFromBar, string text)[] Lines { get; set; }
}
