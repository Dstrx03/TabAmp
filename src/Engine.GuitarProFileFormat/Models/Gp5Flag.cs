using System.Collections.Generic;

namespace TabAmp.Engine.GuitarProFileFormat.Models;

public class Gp5Flag // TODO: naming, concept, implementation
{
    private Dictionary<string, byte> _dictionary;

    public Gp5Flag(short val)
    {
        _dictionary = new Dictionary<string, byte>
        {
            { "Title", 0x001 }
        };
        Val = val;
    }

    public short Val { get; }

    public bool Evaluate(string flagName)
    {
        if (!_dictionary.ContainsKey(flagName))
            return false;

        var flag = _dictionary[flagName];
        var expr = (Val & flag) > 0;
        return expr;
    }
}
