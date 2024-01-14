namespace TabAmp.Engine.GuitarProFileFormat.Models;

public class Gp5RseMasterEffect
{
    public int Volume { get; set; }
    public Gp5RseEqualizer Equalizer { get; set; }


    #region Unknown Properties
    public int _A01 { get; set; }
    #endregion
}
