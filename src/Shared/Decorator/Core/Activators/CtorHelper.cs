using System;
using System.Linq;
using TabAmp.Shared.Decorator.Core.DescriptorChain.Validation;

namespace TabAmp.Shared.Decorator.Core.Activators;

/*internal*/
public static class CtorHelper
{
    public static ServiceDecoratorDescriptorChainValidationResult A(bool stopOnFirstError = false)
    {
        var input = new Input();
        var bOutput = B(input);

        throw new NotImplementedException();
    }

    public static Output B(Input input)
    {
        if (true)
        {
            var error = new InvalidOperationException("B error...");
            if (input.ShouldStopOn(error, out input))
                return input;
        }

        throw new NotImplementedException();
    }

    public static void C()
    {
        throw new NotImplementedException();
    }

    public static int D()
    {
        throw new NotImplementedException();
        return 12345;
    }

    public readonly ref struct Input
    {
        public bool ShouldStopOn(Exception error, out Input input)
        {
            input = new();
            return false;
        }

        public static implicit operator Output(Input input) => new();
    }

    public readonly ref struct Output
    {
    }
}

public static class Extensions
{
}
