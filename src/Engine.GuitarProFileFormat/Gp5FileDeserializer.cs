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

        // TODO:
        // "version" data is stored in size of 30 bytes, the actual version string is 24 characters long
        // remaining 6 bytes seems to have some arbitrary data - it may be not just trailing string bytes
        // does that 30 bytes is actually a "header" of guitar pro file?
    }

    private async ValueTask ReadScoreInformationAsync()
    {
        var scoreInformation = new Gp5ScoreInformation
        {
            Title = await _complexTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Subtitle = await _complexTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Artist = await _complexTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Album = await _complexTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Words = await _complexTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Music = await _complexTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Copyright = await _complexTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Tab = await _complexTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Instructions = await _complexTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Notice = new string[await _primitivesDecoder.ReadIntAsync()]
        };

        for (var i = 0; i < scoreInformation.Notice.Length; i++)
        {
            var noticeLine = await _complexTypesDecoder.ReadStringOfByteLengthIntSizeAsync();
            scoreInformation.Notice[i] = noticeLine;
        }

        _file.ScoreInformation = scoreInformation;
    }
}
