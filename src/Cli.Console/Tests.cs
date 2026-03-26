using TabAmp.Shared.Validation;

namespace TabAmp.Cli.Console;

internal static class Tests
{
    public static ValidationResult SomeValidationMethod(Scope scope = default)
    {
        return scope.ToResult();
    }
}
