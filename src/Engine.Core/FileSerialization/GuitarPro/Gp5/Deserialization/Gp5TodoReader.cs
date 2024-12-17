using System;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization.Common.Exceptions;
using TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Models;

namespace TabAmp.Engine.Core.FileSerialization.GuitarPro.Gp5.Deserialization;

[Obsolete]
internal class Gp5TodoReader : IGp5TodoReader //    
{
    private readonly IGp5BinaryPrimitivesReader _primitivesReader;
    private readonly IGp5TextReader _textReader;
    private readonly IGp5RseEqualizerReader _rseEqualizerReader;

    public Gp5TodoReader(IGp5BinaryPrimitivesReader primitivesReader, IGp5TextReader textReader,
        IGp5RseEqualizerReader rseEqualizerReader)
    {
        _primitivesReader = primitivesReader;
        _textReader = textReader;
        _rseEqualizerReader = rseEqualizerReader;
    }

    public async ValueTask<Gp5Tempo> ReadHeaderTempoAsync()//TODO: move to deserializer
    {
        var tempo = await ReadTempoAsync();
        tempo.HideBeatsPerMinute = await _primitivesReader.ReadBoolAsync();// TODO: named method via Demetra principle

        return tempo;
    }

    

    

    

    

    
}
