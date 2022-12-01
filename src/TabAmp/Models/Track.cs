namespace TabAmp.Models;

public class Track
{
    public byte Flags1 { get; set; }
    public bool IsPercussionTrack { get; set; }
    public bool Is12StringedGuitarTrack { get; set; }
    public bool IsBanjoTrack { get; set; }
    public bool IsVisible { get; set; }
    public bool IsSolo { get; set; }
    public bool IsMute { get; set; }
    public bool UseRSE { get; set; }
    public bool IndicateTuning { get; set; }
    public string Name { get; set; }
    public int StringCount { get; set; }
    public List<int> StringTunings { get; set; }
    public int Port { get; set; }
    public int ChannelIndex { get; set; }
    public int EffectChannel { get; set; }
    public int FretCount { get; set; }
    public int Offset { get; set; }
    public byte ColorR { get; set; }
    public byte ColorG { get; set; }
    public byte ColorB { get; set; }
    public short Flags2 { get; set; }
    public bool Tablature { get; set; }
    public bool Notation { get; set; }
    public bool DiagramsAreBelow { get; set; }
    public bool ShowRhythm { get; set; }
    public bool ForceHorizontal { get; set; }
    public bool ForceChannels { get; set; }
    public bool DiagramList { get; set; }
    public bool DiagramsInScore { get; set; }
    public bool Unknown0 { get; set; }
    public bool AutoLetRing { get; set; }
    public bool AutoBrush { get; set; }
    public bool ExtendRhythmic { get; set; }
    public byte AutoAccentuation { get; set; }
    public byte Bank { get; set; }
    public byte TrackRSEHumanize { get; set; }
    public int Unknown1 { get; set; }
    public int Unknown2 { get; set; }
    public int Unknown3 { get; set; }
    public int Unknown4 { get; set; }
    public int Unknown5 { get; set; }
    public int Unknown6 { get; set; }
    public int Instrument { get; set; }
    public int Unknown8 { get; set; }
    public int SoundBank { get; set; }
    public int EffectNumber { get; set; }
    public List<sbyte> EqualizerKnobs { get; set; }
    public sbyte EqualizerGain { get; set; }
    public string Effect { get; set; }
    public string EffectCategory { get; set; }
}
