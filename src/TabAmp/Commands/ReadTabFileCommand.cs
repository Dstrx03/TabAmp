using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TabAmp.IO;
using TabAmp.Models;

namespace TabAmp.Commands
{
    public record ReadTabFileCommand(string Path) : IRequest<Song>;

    public class ReadTabFileCommandHandler : IRequestHandler<ReadTabFileCommand, Song>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ReadTabFileCommandHandler(IServiceScopeFactory serviceScopeFactory) =>
            _serviceScopeFactory = serviceScopeFactory;

        public async Task<Song> Handle(ReadTabFileCommand request, CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            var tabFileReader = scope.ServiceProvider.GetRequiredService<TabFileReader>();
            var song = await tabFileReader.ReadAsync(request.Path);
            return song;
        }
    }
}
