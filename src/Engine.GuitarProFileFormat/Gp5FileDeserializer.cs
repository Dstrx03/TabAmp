using System.Threading.Tasks;
using TabAmp.Engine.GuitarProFileFormat.FileReader;

namespace TabAmp.Engine.GuitarProFileFormat;

public class Gp5FileDeserializer
{
    private readonly ISerialAsynchronousFileReader _fileReader;
    private readonly Gp5PrimitivesSerialDecoder _primitivesDecoder;
    private readonly Gp5ComplexTypesSerialDecoder _complexTypesDecoder;

    private readonly Gp5File _file;

    public Gp5FileDeserializer(ISerialAsynchronousFileReader fileReader)
    {
        _fileReader = fileReader;
        _primitivesDecoder = new Gp5PrimitivesSerialDecoder(fileReader);
        _complexTypesDecoder = new Gp5ComplexTypesSerialDecoder(fileReader, _primitivesDecoder);

        _file = new Gp5File();
    }

    public async Task<Gp5File> DeserializeAsync()
    {
        await ReadVersionAsync();
        await ReadScoreInformationAsync();
        return _file;
    }

    private async ValueTask ReadVersionAsync()
    {
        const int versionStringSize = 30;
        var versionString = await _complexTypesDecoder.ReadStringOfByteLengthAsync(versionStringSize);
        _file.Version = versionString;
    }

    private async ValueTask ReadScoreInformationAsync()
    {
        var title = await _complexTypesDecoder.ReadStringOfByteLengthIntSizeAsync();
    }
}
