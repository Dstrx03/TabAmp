namespace TabAmp.Commands;

public record ReadTabFileRequest(string Path, CancellationToken CancellationToken);
