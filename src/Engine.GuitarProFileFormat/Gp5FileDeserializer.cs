using System;
using System.Threading.Tasks;
using TabAmp.Engine.GuitarProFileFormat.FileReader;

namespace TabAmp.Engine.GuitarProFileFormat;

public class Gp5FileDeserializer
{
    private readonly Gp5PrimitivesSerialDecoder _primitivesDecoder;
    private readonly Gp5ComplexTypesSerialDecoder _complexTypesDecoder;

    private readonly Gp5File _file;

    public Gp5FileDeserializer(ISerialAsynchronousFileReader fileReader)
    {
        _primitivesDecoder = new Gp5PrimitivesSerialDecoder(fileReader);
        _complexTypesDecoder = new Gp5ComplexTypesSerialDecoder(fileReader, _primitivesDecoder);
        _file = new Gp5File();
    }

    public async Task<Gp5File> DeserializeAsync()
    {
        await ReadVersionAsync();
        return _file;
    }

    private async ValueTask ReadVersionAsync()
    {
        var versionString = await _complexTypesDecoder.ReadStringOfByteSizeAsync();
        _file.Version = versionString;
    }
}
