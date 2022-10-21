namespace TabAmp.IO
{
    public class TabFileTypesReader
    {
        private const int BYTE_SIZE = 1;

        private readonly Reader _reader;

        public TabFileTypesReader(Reader reader) =>
            _reader = reader;

        public async ValueTask<byte> ReadByteAsync()
        {
            var buffer = await _reader.ReadBytesSequenceAsync(BYTE_SIZE);
            return buffer.Span[0];
        }
    }
}
