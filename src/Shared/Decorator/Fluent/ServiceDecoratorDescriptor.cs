namespace TabAmp.Shared.Decorator.Fluent;

public abstract record ServiceDecoratorDescriptor
{
    public abstract ServiceDecoratorDescriptorNode ToNode(ServiceDecoratorDescriptorNode? next);

    internal static ServiceDecoratorDescriptor Create<TDecorator>() => new Instance<TDecorator>();

    private record Instance<TDecorator> : ServiceDecoratorDescriptor
    {
        public override ServiceDecoratorDescriptorNode ToNode(ServiceDecoratorDescriptorNode? next) =>
            new ServiceDecoratorDescriptorNode.Instance<TDecorator>(next);
    }
}
