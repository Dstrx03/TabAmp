using TabAmp.Infrastructure;
using Decoder = TabAmp.Infrastructure.Decoder;

namespace TabAmp.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var path = "../../../../../file.gp5";

            using var reader = new Reader(path);
            var decoder = new Decoder(reader);

            var version = await decoder.ReadByteStringAsync(default);

            Console.WriteLine($"version: '{version}'");
        }
    }
}