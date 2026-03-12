using System;
using TabAmp.Shared.Decorator.Core.DescriptorChain.Validation;
using static TabAmp.Shared.Decorator.Core.Activators.CtorHelper;

namespace TabAmp.Shared.Decorator.Core.Activators;

/*internal*/
public static class CtorHelper
{
    public static ServiceDecoratorDescriptorChainValidationResult A(bool stopOnFirstError = false)
    {
        var input = new Input();

        if (B(input).FooBar(ref input))
            return input;

        if (C(input).FooBar(ref input))
            return input;

        return input;
    }

    private static Output B(Input input)
    {
        if (true)
        {
            var error = new InvalidOperationException("B error...");
            if (error.FooBar(ref input))
                return input;
        }

        return input;
    }

    private static Output C(Input input)
    {
        if (true)
        {
            var error = new InvalidOperationException("C error...");
            if (error.FooBar(ref input))
                return input;
        }

        if (D(input).FooBar(ref input))
            return input;

        return input;
    }

    public static int GetD()
    {
        var output = D(new());
        throw new NotImplementedException();
    }

    private static Output<int> D(Input input)
    {
        if (true)
        {
            var error = new InvalidOperationException("D error...");
            if (error.FooBar(ref input))
                return input.ToOutput<int>();
        }

        return input.ToOutput(12345);
    }

    public readonly ref struct Input
    {
        public Output<T> ToOutput<T>() => new();
        public Output<T> ToOutput<T>(T value) => new();
        public static implicit operator Output(Input input) => new();
        public static implicit operator ServiceDecoratorDescriptorChainValidationResult(Input input) => new();
    }

    public readonly ref struct Output
    {
        public bool FooBar(ref Input input) => false;
    }

    public readonly ref struct Output<T>
    {
        public bool FooBar(ref Input input) => false;
    }
}

public static class Extensions
{
    public static bool FooBar(this Exception error, ref Input input) => false;
}
