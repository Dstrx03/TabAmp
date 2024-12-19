namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models.Tracks;

internal class Gp5MidiChannel
{
    public int Instrument { get; set; }
    public byte Volume { get; set; }
    public byte Balance { get; set; }
    public byte Chorus { get; set; }
    public byte Reverb { get; set; }
    public byte Phaser { get; set; }
    public byte Tremolo { get; set; }


    #region Unknown & Anonymous data
    public byte _A01 { get; set; }
    public byte _A02 { get; set; }
    #endregion
}
