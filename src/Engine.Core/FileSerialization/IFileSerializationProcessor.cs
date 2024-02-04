using System;
using System.Threading.Tasks;

namespace TabAmp.Engine.Core.FileSerialization;

internal interface IFileSerializationProcessor
{
    protected FileSerializationContext Context { get; }

    protected async Task ProcessAsync()
    {
        Context.ProcessingStarted();
        await ProcessTodoAsync();
        Context.ProcessingCompleted();
    }

    protected Task ProcessTodoAsync();
}

internal interface IFileDeserializer<TFileData> : IFileSerializationProcessor
{
    async Task DeserializeAsync()
    {
        await ProcessAsync();
        Context.CommitSuccess(GetFileData);
    }

    TFileData GetFileData();
}



internal abstract class AFileSerializationProcessor : IFileSerializationProcessor
{
    protected readonly FileSerializationContext _context;

    protected AFileSerializationProcessor(FileSerializationContext context)
    {
        _context = context;
    }

    FileSerializationContext IFileSerializationProcessor.Context => _context;

    public async Task ProcessTodoAsync()
    {
        Console.WriteLine($"Start {nameof(AFileDeserializer)}");
        await Task.Delay(5, _context.CancellationToken.Value);
        Console.WriteLine($"End   {nameof(AFileDeserializer)}");
        await Task.Delay(5, _context.CancellationToken.Value);
        Console.WriteLine($"FilePath:{_context.FilePath}");
    }
}

internal class AFileDeserializer : AFileSerializationProcessor, IFileDeserializer<object>
{
    private object _data;

    public AFileDeserializer(FileSerializationContext context) : base(context)
    {
        _data = new();
    }

    public object GetFileData() => _data;
}