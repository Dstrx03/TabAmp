namespace TabAmp.Models;

public class MixTableChange
{
    public sbyte Instrument { get; set; }
    public int RSEInstrument { get; set; }
    public int RSEUnknown0 { get; set; }
    public int RSESoundBank { get; set; }
    public int RSEEffectNumber { get; set; }
    public sbyte Volume { get; set; }
    public sbyte Balance { get; set; }
    public sbyte Chorus { get; set; }
    public sbyte Reverb { get; set; }
    public sbyte Phaser { get; set; }
    public sbyte Tremolo { get; set; }
    public string TempoName { get; set; }
    public int Tempo { get; internal set; }
    public sbyte VolumeDuration { get; set; }
    public sbyte BalanceDuration { get; set; }
    public sbyte ChorusDuration { get; set; }
    public sbyte ReverbDuration { get; set; }
    public sbyte PhaserDuration { get; set; }
    public sbyte TremoloDuration { get; set; }
    public sbyte TempoDuration { get; set; }
    public bool HideTempo { get; set; }
    public sbyte Flags { get; set; }
    public sbyte WahValue { get; set; }
    public string RSEEffect { get; set; }
    public string RSEEffectCategory { get; set; }
}
