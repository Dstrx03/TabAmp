namespace TabAmp.IO
{
    public class TabFileTypesReader : Reader
    {
        private const int BYTE_SIZE = 1;

        public async ValueTask<byte> ReadByteAsync()
        {
            var buffer = await ReadBytesSequenceAsync(BYTE_SIZE);
            return buffer.Span[0];
        }
    }
}
