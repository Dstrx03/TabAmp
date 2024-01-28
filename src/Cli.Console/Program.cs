using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace TabAmp.Cli.Console;

internal class Program
{
    static async Task Main(string[] args)
    {
        using var serviceProvider = new ServiceCollection()
            .AddGuitarProFileFormat()
            .BuildServiceProvider();

        System.Console.WriteLine("sample.gp5");
    }
}
