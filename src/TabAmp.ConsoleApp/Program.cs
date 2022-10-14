using System.Text;
using TabAmp.Infrastructure;

namespace TabAmp.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var path = "../../../../../file.gp5";

            using var reader = new Reader(path);
            Memory<byte> buffer;

            buffer = await reader.ReadBytesAsync(1, default);
            var verTextSize = buffer.Span[0];

            buffer = await reader.ReadBytesAsync(verTextSize, default);
            var verText = Encoding.UTF8.GetString(buffer.Span);

            Console.WriteLine($"verTextSize: {verTextSize}, verText: {verText}");
        }
    }
}