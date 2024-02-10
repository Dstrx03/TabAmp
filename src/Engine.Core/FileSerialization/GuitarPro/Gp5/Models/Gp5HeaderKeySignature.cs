namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

internal class Gp5HeaderKeySignature
{
    public sbyte Key { get; set; }
    public sbyte Octave { get; set; }


    #region Unknown & Anonymous data
    public sbyte _A01 { get; set; }
    public sbyte _A02 { get; set; }
    public sbyte _A03 { get; set; }
    #endregion
}
