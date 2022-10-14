using TabAmp.Infrastructure;

namespace TabAmp.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var reader = new Reader();

            var bytes = await reader.ReadBytesAsync();

            Console.WriteLine(bytes.Length);
        }
    }
}