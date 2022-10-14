using System.Text;

namespace TabAmp.Infrastructure
{
    public class Decoder
    {
        private const int BYTE_SIZE = 1;

        private readonly Reader _reader;

        public Decoder(Reader reader)
        {
            _reader = reader;
        }

        public async Task<byte> ReadByteAsync(CancellationToken cancellationToken)
        {
            var buffer = await _reader.ReadBytesAsync(BYTE_SIZE, cancellationToken);
            return buffer.Span[0];
        }

        public async Task<string> ReadStringAsync(int length, CancellationToken cancellationToken)
        {
            var buffer = await _reader.ReadBytesAsync(length, cancellationToken);
            return Encoding.UTF8.GetString(buffer.Span);
        }

        public async Task<string> ReadByteStringAsync(CancellationToken cancellationToken)
        {
            var length = await ReadByteAsync(cancellationToken);
            return await ReadStringAsync(length, cancellationToken);
        }
    }
}
