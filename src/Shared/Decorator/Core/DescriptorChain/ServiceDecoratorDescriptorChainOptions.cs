using System;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain;

[Flags]
public enum ServiceDecoratorDescriptorChainOptions : byte
{
    UseDefaultImplementationServiceKey = 0x01,
    IsDisposableContainerAllowed = 0x02,
    SkipPreRegistrationValidation = 0x04
}
