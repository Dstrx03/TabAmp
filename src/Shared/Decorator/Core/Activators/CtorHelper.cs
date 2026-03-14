using System;
using System.Diagnostics;
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

    private static Output B(InputGuard guard)
    {
        var input = guard.ToInput();

        if (true)
        {
            var error = new InvalidOperationException("B error...");//1
            if (error.FooBar(ref input))
                return input;
        }

        return input;
    }

    private static Output C(InputGuard guard)
    {
        var input = guard.ToInput();

        if (true)
        {
            var error = new InvalidOperationException("C error...");//2
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

    private static Output<int> D(InputGuard guard)
    {
        var input = guard.ToInput();

        if (E(input).FooBar(ref input))
            return input.ToOutput<int>();

        if (true)
        {
            var error = new InvalidOperationException("D1 error...");//3
            if (error.FooBar(ref input))
                return input.ToOutput<int>();
        }

        if (true)
        {
            var error = new InvalidOperationException("D2 error...");//4
            if (error.FooBar(ref input))
                return input.ToOutput<int>();
        }

        return input.ToOutput(12345);
    }

    private static Output<int> E(InputGuard guard)
    {
        var input = guard.ToInput();

        if (true)
        {
            var error = new InvalidOperationException("E1 error...");//5
            if (error.FooBar(ref input))
                return input.ToOutput<int>();
        }

        if (true)
        {
            var error = new InvalidOperationException("E2 error...");//6
            if (error.FooBar(ref input))
                return input.ToOutput<int>();
        }

        return input.ToOutput(12345);
    }

    public readonly ref struct InputGuard
    {
        private readonly Input _input;
        internal InputGuard(Input input) => _input = input;
        public Input ToInput() => new(_input._totalErrors, 0);
    }

    [DebuggerDisplay("Errors: {_errors}/{_totalErrors}")]
    public readonly ref struct Input
    {
        internal readonly int _totalErrors;
        internal readonly int _errors;
        internal Input(int totalErrors, int errors)
        {
            _totalErrors = totalErrors;
            _errors = errors;
        }
        public Output<T> ToOutput<T>() => new(this);
        public Output<T> ToOutput<T>(T value) => new(this);
        public static implicit operator Output(Input input) => new(input);
        public static implicit operator InputGuard(Input input) => new(input);
        public static implicit operator ServiceDecoratorDescriptorChainValidationResult(Input input) => new();
    }

    public readonly ref struct Output
    {
        private readonly Input _input;
        internal Output(Input input) => _input = input;
        public bool FooBar(ref Input input)
        {
            var totalErrors = _input._totalErrors;
            var errors = _input._errors > 0 ? input._errors + 1 : input._errors;
            input = new Input(totalErrors, errors);

            return false;
        }
    }

    public readonly ref struct Output<T>
    {
        private readonly Input _input;
        internal Output(Input input) => _input = input;
        public bool FooBar(ref Input input)
        {
            var totalErrors = _input._totalErrors;
            var errors = _input._errors > 0 ? input._errors + 1 : input._errors;
            input = new Input(totalErrors, errors);

            return false;
        }
    }
}

public static class Extensions
{
    public static bool FooBar(this Exception error, ref Input input)
    {
        var totalErrors = input._totalErrors + 1;
        var errors = input._errors + 1;
        input = new Input(totalErrors, errors);

        return false;
    }
}
