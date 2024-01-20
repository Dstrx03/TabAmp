namespace TabAmp.Engine.GuitarProFileFormat.Models;

public class Gp5RseMasterEffect
{
    public int Volume { get; set; }
    public Gp5RseEqualizer Equalizer { get; set; }
    public int Reverb { get; set; }


    #region Undetermined Data
    public int UNKN_A01 { get; set; }
    #endregion
}
