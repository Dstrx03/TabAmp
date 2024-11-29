namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5RseInstrument
{
    public int Instrument { get; set; }
    public int SoundBank { get; set; }


    #region Unknown & Anonymous data
    public int _A01 { get; set; }
    public int _B01 { get; set; }
    #endregion
}
