using System;
using System.Threading;

namespace TabAmp.Engine.Core.FileSerialization;

internal sealed class FileSerializationContext
{
    public FileSerializationContext()
    {
        ValidateThenSetStatus(CycleStatus.Created);
    }

    public CycleStatus Status { get; private set; }

    public string? FilePath { get; private set; }
    public CancellationToken? CancellationToken { get; private set; }

    public void Initialize(string filePath, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(filePath) || string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Value cannot be null or empty.", nameof(filePath));

        var action = () =>
        {
            FilePath = filePath;
            CancellationToken = cancellationToken;
        };

        ValidateThenSetStatus(action, CycleStatus.Initialized);
    }

    public void ProcessingStarted() =>
        ValidateThenSetStatus(CycleStatus.ProcessingStarted);

    public void ProcessingCompleted() =>
        ValidateThenSetStatus(CycleStatus.ProcessingCompleted);

    public void CommitSuccess<TFileData>(Func<TFileData> fileDataFunc)
    {
        var action = () =>
        {
            var fileData = fileDataFunc();
            if (fileData is null)
                throw new ArgumentNullException(nameof(fileData));
        };

        ValidateThenSetStatus(action, CycleStatus.SuccessCommitted);
    }

    private void ValidateThenSetStatus(CycleStatus value) =>
        ValidateThenSetStatus(null, value);

    private void ValidateThenSetStatus(Action? action, CycleStatus value)
    {
        if (value != Status + 1)
            throw new InvalidOperationException($"{Status} => {value}.");

        if (action is not null)
            action();

        Status = value;
    }

    public enum CycleStatus
    {
        None,
        Created,
        Initialized,
        ProcessingStarted,
        ProcessingCompleted,
        SuccessCommitted
    }
}
