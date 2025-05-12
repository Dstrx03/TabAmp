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
        Reason.DriveNotFound => "the specified drive is unavailable",
        Reason.InvalidPath => "the specified path is invalid",
        Reason.PathTooLong => "the file path is too long",
        Reason.AccessDenied => "access is denied",
        _ => "TODO"
    };
}
