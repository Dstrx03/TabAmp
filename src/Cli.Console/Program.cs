using Microsoft.Extensions.DependencyInjection;
using System.Threading;
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

        var cts = new CancellationTokenSource();
        var fileSerializationService = serviceProvider.GetRequiredService<IFileSerializationService>();
        var task = fileSerializationService.ReadFileAsync<object>("sample.gp5", cts.Token);

        await Task.Delay(1000);
        cts.Cancel();

        await task;
    }
}
