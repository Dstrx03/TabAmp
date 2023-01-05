namespace TabAmp.Models;

public class Beat
{
    public byte Flags { get; set; }
    public byte? Status { get; set; }
    public sbyte Duration { get; set; }
    public Chord Chord { get; set; }
    public string Text { get; set; }
    public BeatEffect BeatEffect { get; set; }
    public MixTableChange MixTableChange { get; set; }
    public Notes Notes { get; set; }
    public short Flags2 { get; set; }
    public byte DisplayBreakSecondary { get; set; }
}
