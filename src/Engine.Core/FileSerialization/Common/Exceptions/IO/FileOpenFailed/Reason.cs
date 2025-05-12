namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.FileOpenFailed;

internal enum Reason
{
    IOError,
    FileNotFound,
    DriveNotFound,
    InvalidPath,
    PathTooLong,
    AccessDenied
}
