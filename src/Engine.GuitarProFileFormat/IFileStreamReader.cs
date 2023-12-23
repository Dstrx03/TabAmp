using System.Threading.Tasks;

namespace TabAmp.Engine.GuitarProFileFormat
{
    public interface IFileStreamReader
    {
        long Length { get; }
        long Position { get; }

        ValueTask<byte[]> ReadNextAsync(int bytesCount);
        void SkipNext(int bytesCount);
    }
}
