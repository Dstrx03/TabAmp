using System;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.FileOpenFailed;

internal sealed class FileOpenFailedException : FileSerializationException
{
    private const string MessageTemplate = "Could not open the file: {0}.";

    public FileOpenFailedException(Reason reason, Exception inner)
        : base(ComposeMessage(reason), inner)
    {
        Reason = reason;
    }

    public Reason Reason { get; }

    private static string ComposeMessage(Reason reason) =>
        string.Format(MessageTemplate, GetMessageComponent(reason));

    private static string GetMessageComponent(Reason reason) => reason switch
    {
        Reason.IOError => "an unexpected I/O error occurred",
        Reason.FileNotFound => "the file does not exist",
        Reason.InvalidPath => "the specified path is invalid",
        Reason.DriveNotFound => "the specified drive is unavailable",
        Reason.PathTooLong => "the file path is too long",
        Reason.UnsupportedPathFormat => "the path format is not supported",
        Reason.AccessDenied => "access is denied",
        Reason.SecurityError => "the operation is not permitted due to security restrictions",
        _ => "TODO"
    };
}
