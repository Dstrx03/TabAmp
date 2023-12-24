using System.Text;
using System.Threading.Tasks;
using TabAmp.Engine.GuitarProFileFormat.FileReader;

namespace TabAmp.Engine.GuitarProFileFormat
{
    public class Gp5ComplexTypesSerialDecoder
    {
        private readonly ISerialAsynchronousFileReader _fileReader;
        private readonly Gp5PrimitivesSerialDecoder _primitivesDecoder;

        public Gp5ComplexTypesSerialDecoder(ISerialAsynchronousFileReader fileReader, Gp5PrimitivesSerialDecoder primitivesDecoder)
        {
            _fileReader = fileReader;
            _primitivesDecoder = primitivesDecoder;
        }

        public async ValueTask<string> ReadByteSizeStringAsync(int readSize = 0)
        {
            var size = await _primitivesDecoder.ReadByteAsync();
            return await ReadStringAsync(size, readSize);
        }

        public async ValueTask<string> ReadIntSizeStringAsync(int readSize = 0)
        {
            var size = await _primitivesDecoder.ReadIntAsync();
            return await ReadStringAsync(size, readSize);
        }

        public async ValueTask<string> ReadIntByteSizeStringAsync()
        {
            var intValue = await _primitivesDecoder.ReadIntAsync();
            var byteValue = await _primitivesDecoder.ReadByteAsync();
            var readSize = intValue - 1;
            var stringValue = await ReadStringAsync(byteValue, readSize);
            return stringValue;
        }

        private async ValueTask<string> ReadStringAsync(int size, int readSize)
        {
            var buffer = await _fileReader.ReadBytesAsync(size);
            var skipBytes = readSize - size;
            if (skipBytes > 0)
                _fileReader.SkipBytes(skipBytes);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
