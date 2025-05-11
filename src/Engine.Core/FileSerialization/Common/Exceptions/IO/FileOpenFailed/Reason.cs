namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IO.FileOpenFailed;

internal enum Reason
{
    IOError = 1,
    FileNotFound = 2,
    InvalidPath = 3,
    DriveNotFound = 4,
    PathTooLong = 5,
    UnsupportedPathFormat = 6,
    AccessDenied = 7,
    SecurityError = 8
}
