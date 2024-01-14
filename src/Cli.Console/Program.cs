﻿using System.Text.Json;
using System.Threading.Tasks;
using TabAmp.Engine.GuitarProFileFormat;
using TabAmp.Engine.GuitarProFileFormat.FileReader;

namespace TabAmp.Cli.Console;

internal class Program
{
    static async Task Main(string[] args)
    {
        using var reader = new PocSerialAsynchronousFileReader("sample.gp5");
        var deserializer = new Gp5FileDeserializer(reader);

        var file = await deserializer.DeserializeAsync();
        var fileJson = JsonSerializer.Serialize(file, new JsonSerializerOptions { WriteIndented = true });

        System.Console.WriteLine(fileJson);
    }
}