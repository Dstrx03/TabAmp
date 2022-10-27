namespace TabAmp.IO
{
    public class TabFileTypesReader
    {
        private const int BYTE_SIZE = 1;

        private readonly IReader _reader;

        public TabFileTypesReader(IReader reader) =>
            _reader = reader;

        public async ValueTask<byte> ReadByteAsync()
        {
            var buffer = await _reader.ReadBytesSequenceAsync(BYTE_SIZE);
            return buffer.Span[0];
        }

        public void SkipBytesSequence(int count) =>
            _reader.SkipBytesSequence(count);
    }
}
