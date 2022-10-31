using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TabAmp.Commands;

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
                    services.AddIOServices();
                })
                .Build();

            Console.Write("Path: ");
            var path = Console.ReadLine();

            var mediator = host.Services.GetRequiredService<IMediator>();

            var songFirst = await mediator.Send(new ReadTabFileCommand(path));
            Console.WriteLine($"First Song Version: '{songFirst.Version}'");

            var songSecond = await mediator.Send(new ReadTabFileCommand(path));
            Console.WriteLine($"Second Song Version: '{songSecond.Version}'");

            await host.RunAsync();
        }
    }
}