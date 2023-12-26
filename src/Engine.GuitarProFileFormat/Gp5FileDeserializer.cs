using System;
using System.Threading.Tasks;
using TabAmp.Engine.GuitarProFileFormat.FileReader;

namespace TabAmp.Engine.GuitarProFileFormat
{
    public class Gp5FileDeserializer
    {
        private readonly Gp5PrimitivesSerialDecoder _primitivesDecoder;
        private readonly Gp5ComplexTypesSerialDecoder _complexTypesDecoder;

        public Gp5FileDeserializer(ISerialAsynchronousFileReader fileReader)
        {
            _primitivesDecoder = new Gp5PrimitivesSerialDecoder(fileReader);
            _complexTypesDecoder = new Gp5ComplexTypesSerialDecoder(fileReader, _primitivesDecoder);
        }

        public async Task<object> DeserializeAsync()
        {
            var verString = await _complexTypesDecoder.ReadStringOfByteSizeAsync();
            Console.WriteLine($"Ver: [{verString}]");
            throw new NotImplementedException();
        }
    }
}
