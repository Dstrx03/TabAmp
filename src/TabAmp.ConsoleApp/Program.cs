using TabAmp.Infrastructure;

namespace TabAmp.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var path = "../../../../../file.gp5";

            using var reader = new Reader(path);

            var readOnlyMemory_1 = await reader.ReadBytesAsync(4, default);
            var readOnlyMemory_2 = await reader.ReadBytesAsync(4, default);
        }
    }
}