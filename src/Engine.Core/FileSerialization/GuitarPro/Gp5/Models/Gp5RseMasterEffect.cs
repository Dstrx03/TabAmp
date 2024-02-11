namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5RseMasterEffect
{
    public const int EqualizerBandsCount = 10;

    public int Volume { get; set; }
    public Gp5RseEqualizer Equalizer { get; set; }
    public int Reverb { get; set; }


    #region Unknown & Anonymous data
    public int _A01 { get; set; }
    #endregion
}
