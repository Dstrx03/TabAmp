using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TabAmp.Engine.Core.FileSerialization;
using TabAmp.Engine.Core.FileSerialization.DependencyInjection;
using TabAmp.Engine.Core.Score;

namespace TabAmp.Cli.Console;

internal class Program
{
    static async Task Main(string[] args)
    {
        using var serviceProvider = new ServiceCollection()
            .AddFileSerialization()
            .BuildServiceProvider(validateScopes: true);

        var fileSerializationService = serviceProvider.GetRequiredService<IFileSerializationService>();
        var file = await fileSerializationService.ReadFileAsync<Gp5Score>("sample.gp5");
    }
}
