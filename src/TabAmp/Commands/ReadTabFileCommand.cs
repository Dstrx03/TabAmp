using MediatR;
using TabAmp.IO;
using TabAmp.Models;

namespace TabAmp.Commands
{
    public record ReadTabFileCommand(string Path) : IRequest<Song>;

    public class ReadTabFileCommandHandler : IRequestHandler<ReadTabFileCommand, Song>
    {
        private readonly TabFileReader _tabFileReader;

        public ReadTabFileCommandHandler(TabFileReader tabFileReader) =>
            _tabFileReader = tabFileReader;

        public Task<Song> Handle(ReadTabFileCommand request, CancellationToken cancellationToken) =>
            _tabFileReader.ReadAsync();
    }
}
