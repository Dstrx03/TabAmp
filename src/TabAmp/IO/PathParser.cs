using TabAmp.Models;

namespace TabAmp.IO;

public class PathParser : IPathParser
{
    public PathInfo Parse(string path)
    {
        var fileInfo = new FileInfo(path);
        return new PathInfo(fileInfo.Name, fileInfo.FullName, fileInfo.Extension);
    }
}
