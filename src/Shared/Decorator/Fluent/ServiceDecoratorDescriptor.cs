namespace TabAmp.Shared.Decorator.Fluent;

public abstract record ServiceDecoratorDescriptor
{
    internal static ServiceDecoratorDescriptor Create<TDecorator>() => new Instance<TDecorator>();

    private record Instance<TDecorator> : ServiceDecoratorDescriptor;
}
