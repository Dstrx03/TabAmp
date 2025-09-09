namespace TabAmp.Shared.Decorator.Fluent;

public abstract record ServiceDecoratorDescriptorNode(ServiceDecoratorDescriptorNode? Next, object _placeholder)
{
    internal sealed record Instance<TDecorator>(ServiceDecoratorDescriptorNode? Next) :
        ServiceDecoratorDescriptorNode(Next, null);
}
