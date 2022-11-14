using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TabAmp.Commands;

namespace TabAmp;

internal class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                services.AddMediatR(typeof(Program));
                services.AddIOServices();
            })
            .Build();

        Console.Write("Path: ");
        var path = Console.ReadLine();

        var mediator = host.Services.GetRequiredService<IMediator>();

        var resultFirst = await mediator.Send(new ReadTabFileCommand(path));
        PrintResult(resultFirst, nameof(resultFirst));

        var resultSecond = await mediator.Send(new ReadTabFileCommand(path));
        PrintResult(resultSecond, nameof(resultSecond));

        await host.RunAsync();
    }

    private static void PrintResult(ReadTabFileResult result, string resultName)
    {
        if (result.Success)
            Console.WriteLine($"Result ({resultName}) Version: [{result.TabFile.Version}]");
        else
            Console.WriteLine($"Result ({resultName}) Error: {result.Exception.Message}");
    }
}