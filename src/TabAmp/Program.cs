using Microsoft.Extensions.Hosting;

namespace TabAmp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) => { })
                .Build();
            await host.RunAsync();
        }
    }
}