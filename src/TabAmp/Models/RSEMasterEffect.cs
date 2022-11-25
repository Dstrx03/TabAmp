namespace TabAmp.Models;

public class RSEMasterEffect
{
    public int Volume { get; set; }
    public int UnknownProperty_0 { get; set; }
    public List<sbyte> EqualizerKnobs { get; set; }
    public sbyte EqualizerGain { get; set; }
    public int Reverb { get; set; }
}
