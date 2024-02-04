using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TabAmp.Engine.Core.FileSerialization;

namespace TabAmp.Cli.Console;

internal class Program
{
    static async Task Main(string[] args)
    {
        using var serviceProvider = new ServiceCollection()
            .AddEngineCore()
            .BuildServiceProvider();

        var fileSerializationService = serviceProvider.GetRequiredService<IFileSerializationService>();
        var data = await fileSerializationService.ReadFileAsync<object>("sample.gp5");
    }
}
