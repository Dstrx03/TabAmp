using MediatR;

namespace TabAmp.Commands
{
    public record ReadTabFileCommand(string Path) : IRequest<ReadTabFileResult>;

    public class ReadTabFileCommandHandler : IRequestHandler<ReadTabFileCommand, ReadTabFileResult>
    {
        public Task<ReadTabFileResult> Handle(ReadTabFileCommand request, CancellationToken cancellationToken)
        {
            var result = new ReadTabFileResult { Path = request.Path };
            return Task.FromResult(result);
        }
    }
}
