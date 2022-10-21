namespace TabAmp.IO
{
    public abstract class Reader : IDisposable
    {
        private byte _fakeData = 65;

        public Reader()
        {
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        protected ValueTask<ReadOnlyMemory<byte>> ReadBytesSequenceAsync(int count) =>
            ValueTask.FromResult(new ReadOnlyMemory<byte>(new[] { _fakeData++ }));
    }
}
