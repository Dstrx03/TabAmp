namespace TabAmp.Models;

public class TabFile
{
    public TabFile(PathInfo pathInfo, GP5Song song) =>
        (PathInfo, Song) = (pathInfo, song);

    public PathInfo PathInfo { get; }
    public GP5Song Song { get; }
}
