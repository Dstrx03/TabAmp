﻿using System.Threading.Tasks;
using TabAmp.Engine.GuitarProFileFormat.FileReader;
using TabAmp.Engine.GuitarProFileFormat.Models;

namespace TabAmp.Engine.GuitarProFileFormat;

public class Gp5FileDeserializer
{
    private readonly ISerialAsynchronousFileReader _fileReader;
    private readonly Gp5PrimitivesSerialDecoder _primitivesDecoder;
    private readonly Gp5CompositeTypesSerialDecoder _compositeTypesDecoder;

    private readonly Gp5File _file;

    public Gp5FileDeserializer(ISerialAsynchronousFileReader fileReader)
    {
        _fileReader = fileReader;
        _primitivesDecoder = new Gp5PrimitivesSerialDecoder(fileReader);
        _compositeTypesDecoder = new Gp5CompositeTypesSerialDecoder(fileReader, _primitivesDecoder);

        _file = new Gp5File();
    }

    public async Task<Gp5File> DeserializeAsync()
    {
        await ReadVersionAsync();
        await ReadScoreInformationAsync();
        return _file;
    }

    // TODO:
    // "version" data is stored in size of 30 bytes, the actual version string is 24 characters long
    // remaining 6 bytes seems to have some arbitrary data - it may be not just trailing string bytes
    // does that 30 bytes is actually a "header" of guitar pro file?
    private async ValueTask ReadVersionAsync()
    {
        const int versionStringSize = 30;
        var versionString = await _compositeTypesDecoder.ReadStringOfByteLengthAsync(versionStringSize);

        _file.Version = versionString;
    }

    private async ValueTask ReadScoreInformationAsync()
    {
        var scoreInformation = new Gp5ScoreInformation
        {
            Title = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Subtitle = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Artist = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Album = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Words = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Music = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Copyright = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Tab = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Instructions = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync(),
            Notice = new string[await _primitivesDecoder.ReadIntAsync()]
        };

        for (var i = 0; i < scoreInformation.Notice.Length; i++)
        {
            var noticeLine = await _compositeTypesDecoder.ReadStringOfByteLengthIntSizeAsync();
            scoreInformation.Notice[i] = noticeLine;
        }

        _file.ScoreInformation = scoreInformation;
    }
}
