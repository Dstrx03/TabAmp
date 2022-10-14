using System.Buffers;
using TabAmp.Infrastructure;

namespace TabAmp.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var path = "../../../../../file.gp5";

            using var reader = new Reader(path);
            using var owner = MemoryPool<byte>.Shared.Rent();

            var buffer = owner.Memory[..4];

            await reader.ReadBytesAsync(buffer, default);
            await reader.ReadBytesAsync(buffer, default);

            Console.WriteLine("Exit");
        }
    }
}