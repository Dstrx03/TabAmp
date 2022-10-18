using TabAmp.Infrastructure;

namespace TabAmp.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var path = "../../../../../file.gp5";

            using var reader = new Reader(path);
        }
    }
}