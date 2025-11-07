namespace TabAmp.Shared.Decorator.DescriptorChain;

public abstract class ServiceDecoratorDescriptor<TService>
    where TService : notnull
{
    internal ServiceDecoratorDescriptor<TService>? Next { get; private set; }
    internal int? Position { get; private set; }

    private ServiceDecoratorDescriptor()
    {
    }

    internal ServiceDecoratorDescriptor<TService> AppendTo(ServiceDecoratorDescriptor<TService>? descriptor)
    {
        Next = descriptor;
        Position = descriptor?.Position + 1 ?? 1;

        return this;
    }

    internal abstract ServiceDecoratorDescriptorChain<TService> ToChain(
        ServiceDecoratorDescriptorChain<TService> descriptorChain);

    public class For<TDecorator> : ServiceDecoratorDescriptor<TService>
        where TDecorator : notnull, TService
    {
        internal sealed override ServiceDecoratorDescriptorChain<TService> ToChain(
            ServiceDecoratorDescriptorChain<TService> descriptorChain)
        {
            return new ServiceDecoratorDescriptorChain<TService>.For<TDecorator>(next: descriptorChain);
        }
    }
}
