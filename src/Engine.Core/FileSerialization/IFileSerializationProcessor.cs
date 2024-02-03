using System;
using System.Threading.Tasks;

namespace TabAmp.Engine.Core.FileSerialization;

internal interface IFileSerializationProcessor
{
    protected FileSerializationContext Context { get; }

    public async Task ProcessAsync()
    {
        Context.ProcessingStarted();
        await ProcessTodoAsync();
        Context.ProcessingCompleted();
    }

    protected Task ProcessTodoAsync();
}

internal interface IFileDeserializer : IFileSerializationProcessor
{
}



internal class AFileDeserializer : IFileDeserializer
{
    private readonly FileSerializationContext _context;

    public AFileDeserializer(FileSerializationContext context)
    {
        _context = context;
    }

    FileSerializationContext IFileSerializationProcessor.Context => _context;

    async Task IFileSerializationProcessor.ProcessTodoAsync()
    {
        Console.WriteLine($"Start {nameof(AFileDeserializer)}");
        await Task.Delay(5000, _context.CancellationToken.Value);
        Console.WriteLine($"End   {nameof(AFileDeserializer)}");
        await Task.Delay(5000, _context.CancellationToken.Value);
        Console.WriteLine($"FilePath:{_context.FilePath}");
    }
}