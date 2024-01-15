namespace TabAmp.Engine.GuitarProFileFormat.Models;

public class Gp5MidiChannel
{
    public int Instrument { get; set; }
    public byte Volume { get; set; }
    public byte Balance { get; set; }
    public byte Chorus { get; set; }
    public byte Reverb { get; set; }
    public byte Phaser { get; set; }
    public byte Tremolo { get; set; }

    public byte Blank1 { get; set; }
    public byte Blank2 { get; set; }
}
