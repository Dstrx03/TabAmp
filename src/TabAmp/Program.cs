using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TabAmp.Commands;
using TabAmp.IO;

namespace TabAmp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddMediatR(typeof(Program));
                    services.AddTransient<TabFileReader>();
                    services.AddTransient<TabFileTypesReader>();
                })
                .Build();

            Console.Write("Path: ");
            var path = Console.ReadLine();

            var mediator = host.Services.GetRequiredService<IMediator>();

            var song_1 = await mediator.Send(new ReadTabFileCommand(path));
            Console.WriteLine($"Song 1 Version: '{song_1.Version}'");

            var song_2 = await mediator.Send(new ReadTabFileCommand(path));
            Console.WriteLine($"Song 2 Version: '{song_2.Version}'");

            await host.RunAsync();
        }
    }
}