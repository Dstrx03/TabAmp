using TabAmp.Shared.Fuse;

namespace TabAmp.Shared.Decorator.Core.ConstructorDiscovery;

public static class ServiceDecoratorConstructorDiscoveryValidator
{
    public static FuseResult ValidateConstructor<TService, TDecorator>(FuseScope scope = default)
        where TDecorator : TService
    {
        return ServiceDecoratorConstructorDiscovery.DiscoverConstructor<TService, TDecorator>(scope);
    }
}
