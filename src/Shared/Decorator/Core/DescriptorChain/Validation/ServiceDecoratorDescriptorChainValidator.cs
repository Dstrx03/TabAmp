using System;
using System.Collections.Generic;

namespace TabAmp.Shared.Decorator.Core.DescriptorChain.Validation;

internal static class ServiceDecoratorDescriptorChainValidator
{
    internal static void ValidateAndThrow<TService>(
        ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : class => Validate(descriptorChain).ThrowFirstErrorIfAny();

    internal static ServiceDecoratorDescriptorChainValidationResult Validate<TService>(
        ServiceDecoratorDescriptorChain<TService> descriptorChain)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(descriptorChain);
        List<Exception>? errors = null;
        errors ??= [];
        errors.Add(new NotImplementedException("TODO... #1"));
        errors.Add(new NotImplementedException("TODO... #2"));
        return new(errors);
    }
}
