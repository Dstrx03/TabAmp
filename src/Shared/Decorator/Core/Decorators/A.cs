using System;
using TabAmp.Shared.Decorator.Core.DescriptorChain;

namespace TabAmp.Shared.Decorator.Core.Decorators;

internal class A<TService>
    where TService : notnull
{
    internal void TODO1(TService service, ServiceDecoratorDescriptorChain<TService> descriptor)
    {
        throw new NotImplementedException();
    }

    internal TService TODO2(TService service)
    {
        throw new NotImplementedException();
    }
}
