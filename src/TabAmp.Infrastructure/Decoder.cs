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

        public async ValueTask<byte> ReadByteAsync(CancellationToken cancellationToken)
        {
            var buffer = await _reader.ReadBytesAsync(BYTE_SIZE, cancellationToken);
            return buffer.Span[0];
        }

        public async ValueTask<string> ReadStringAsync(int length, CancellationToken cancellationToken)
        {
            var buffer = await _reader.ReadBytesAsync(length, cancellationToken);
            return Encoding.UTF8.GetString(buffer.Span);
        }

        public async ValueTask<string> ReadByteStringAsync(CancellationToken cancellationToken)
        {
            var length = await ReadByteAsync(cancellationToken);
            return await ReadStringAsync(length, cancellationToken);
        }

        [Obsolete]
        public async Task<byte> ReadByteAsync_Task(CancellationToken cancellationToken)
        {
            var buffer = await _reader.ReadBytesAsync_Task(BYTE_SIZE, cancellationToken);
            return buffer.Span[0];
        }

        [Obsolete]
        public async Task<string> ReadStringAsync_Task(int length, CancellationToken cancellationToken)
        {
            var buffer = await _reader.ReadBytesAsync_Task(length, cancellationToken);
            return Encoding.UTF8.GetString(buffer.Span);
        }

        [Obsolete]
        public async Task<string> ReadByteStringAsync_Task(CancellationToken cancellationToken)
        {
            var length = await ReadByteAsync_Task(cancellationToken);
            return await ReadStringAsync_Task(length, cancellationToken);
        }
    }
}
