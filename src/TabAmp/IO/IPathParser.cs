using TabAmp.Models;

namespace TabAmp.IO;

public interface IPathParser
{
    public PathInfo Parse(string path);
}
