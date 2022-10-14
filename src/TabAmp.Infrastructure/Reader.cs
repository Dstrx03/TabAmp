using System.Buffers;

namespace TabAmp.Infrastructure
{
    public class Reader : IDisposable
    {
        private readonly FileStream _stream;
        private readonly IMemoryOwner<byte> _owner;

        public Reader(string path)
        {
            _stream = File.Open(path, new FileStreamOptions { Options = FileOptions.Asynchronous });
            _owner = MemoryPool<byte>.Shared.Rent();
        }

        public async Task<Memory<byte>> ReadBytesAsync(int count, CancellationToken cancellationToken)
        {
            var buffer = _owner.Memory[..count];
            await _stream.ReadAsync(buffer, cancellationToken);
            return buffer;
        }

        public void Dispose()
        {
            _stream?.Dispose();
            _owner?.Dispose();
        }

        [Obsolete]
        public async Task<byte[]> ReadBytesAsync_Obsolete(int count, CancellationToken cancellationToken)
        {
            var buffer = new byte[count];
            await _stream.ReadAsync(buffer);
            return buffer;
        }
    }
}
