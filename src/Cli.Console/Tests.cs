using TabAmp.Shared.Validation;

namespace TabAmp.Cli.Console;

internal static class Tests
{
    public static void SomeValidationMethod(bool stopOnFirstError = false)
    {
        var s = Scope.Init_TODONAME(stopOnFirstError);
    }
}
