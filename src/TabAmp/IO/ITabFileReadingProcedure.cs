using TabAmp.Models;

namespace TabAmp.IO;

public interface ITabFileReadingProcedure
{
    public Task<TabFile> ReadAsync();
}
