using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.IO;

public class TabFileReadingProcedureFactory
{
    private const string FileExtensionGP5 = ".gp5";

    private readonly IServiceProvider _serviceProvider;
    private readonly ITabFileReaderContext _context;

    public TabFileReadingProcedureFactory(IServiceProvider serviceProvider, ITabFileReaderContext context) =>
        (_serviceProvider, _context) = (serviceProvider, context);

    public ITabFileReadingProcedure GetReadingProcedure()
    {
        return _context.PathInfo.FileExtension switch
        {
            FileExtensionGP5 => GetReadingProcedure<GP5ReadingProcedure>(),
            _ => throw new TabFileExtensionNotSupportedException(_context.PathInfo.FilePath),
        };
    }

    private ITabFileReadingProcedure GetReadingProcedure<T>()
        where T : ITabFileReadingProcedure => _serviceProvider.GetRequiredService<T>();
}
