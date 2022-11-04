using Microsoft.Extensions.DependencyInjection;

namespace TabAmp.IO;

public class TabFileReadingProcedureFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITabFileReaderContext _context;

    public TabFileReadingProcedureFactory(IServiceProvider serviceProvider, ITabFileReaderContext context) =>
        (_serviceProvider, _context) = (serviceProvider, context);

    public ITabFileReadingProcedure CreateReadingProcedureForScope()
    {
        return _context.FileExtension switch
        {
            TabFileExtension.GP5 => GetReadingProcedure<GP5ReadingProcedure>(),
            _ => throw new TabFileExtensionNotSupportedException(_context.FilePath),
        };
    }

    private ITabFileReadingProcedure GetReadingProcedure<T>()
        where T : ITabFileReadingProcedure => _serviceProvider.GetRequiredService<T>();
}
