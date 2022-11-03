using MediatR;
using TabAmp.IO;

namespace TabAmp.Commands;

public record ReadTabFileCommand(string Path) : IRequest<ReadTabFileResult>;

public class ReadTabFileCommandHandler : IRequestHandler<ReadTabFileCommand, ReadTabFileResult>
{
    private readonly ITabFileReader _tabFileReader;

    public ReadTabFileCommandHandler(ITabFileReader tabFileReader) =>
        _tabFileReader = tabFileReader;

    public Task<ReadTabFileResult> Handle(ReadTabFileCommand request, CancellationToken cancellationToken) =>
        _tabFileReader.ReadAsync(new ReadTabFileRequest(request.Path, cancellationToken));
}
