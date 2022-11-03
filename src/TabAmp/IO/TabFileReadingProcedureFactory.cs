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
            TabFileExtension.GP5 => GetRequiredService<GP5ReadingProcedure>(),
            _ => throw new Exception($"{_context.FilePath} filename extension is not supproted."),
        };
    }

    private T GetRequiredService<T>() =>
        _serviceProvider.GetRequiredService<T>();
}
