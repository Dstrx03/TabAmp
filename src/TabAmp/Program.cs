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
                .ConfigureServices((_, services) => { services.AddMediatR(typeof(Program)); })
                .Build();

            var mediator = host.Services.GetRequiredService<IMediator>();

            var command = new ReadTabFileCommand("somepath/somefile.ext");
            var result = await mediator.Send(command);

            Console.WriteLine($"Path: '{result.Path}'");

            await host.RunAsync();
        }
    }
}