namespace TabAmp.Shared.Decorator.DescriptorChain;

public abstract class ServiceDecoratorDescriptor<TService>
    where TService : notnull
{
    /*
    internal int? Position { get; private set; }
    

    internal ServiceDecoratorDescriptor<TService> AppendTo(ServiceDecoratorDescriptor<TService>? descriptor)
    {
        if (Position is not null)//TODO: check descriptor.Position is not null, check Position value overflow
            throw TODO_NAME();

        Position = descriptor?.Position + 1 ?? 1;//TODO: zero based is more idiomatic?
        Next = descriptor;

        return this;
    }

    internal ServiceDecoratorDescriptor<TService>? ChainTo(ServiceDecoratorDescriptor<TService>? descriptorChain)
    {
        var next = Next;
        Next = descriptorChain;

        return next;
    }

    

    

    private static InvalidOperationException TODO_NAME() => 
        new($"");

    private static InvalidOperationException ContextAlreadyExistsException(FileSerializationContext context) =>
        new($"Cannot create the context: {nameof(FileSerializationContext)} already exists in the current scope " +
            $"with {nameof(FileSerializationContext.FilePath)}: '{context.FilePath}' and cannot be initialized again.");

    //TODO: since base class is more sophisticated, extract to separate file?
    [DebuggerDisplay("TDecorator = {typeof(TDecorator).Name}")]
    
    */
}
