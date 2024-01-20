namespace TabAmp.Engine.GuitarProFileFormat.Models;

public class Gp5HeaderKeySignature
{
    public sbyte Key { get; set; }
    public sbyte Octave { get; set; }


    #region Undetermined Data
    public sbyte UNKN_A01 { get; set; }
    public sbyte UNKN_A02 { get; set; }
    public sbyte UNKN_A03 { get; set; }
    #endregion
}
