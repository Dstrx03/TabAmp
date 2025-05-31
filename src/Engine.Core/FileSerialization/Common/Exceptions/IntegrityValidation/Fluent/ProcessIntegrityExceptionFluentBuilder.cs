using System.Runtime.CompilerServices;

namespace TabAmp.Engine.Core.FileSerialization.Common.Exceptions.IntegrityValidation.Fluent;

internal class ProcessIntegrityExceptionFluentBuilder
{
    private void FooBar(int a, [CallerArgumentExpression(nameof(a))] string? aName = default) { }
}
