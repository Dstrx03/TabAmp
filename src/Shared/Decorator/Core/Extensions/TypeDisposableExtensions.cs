using System;

namespace TabAmp.Shared.Decorator.Core.Extensions;

internal static class TypeDisposableExtensions
{
    internal static bool IsDisposable(this Type type) =>
        type.IsAssignableTo(typeof(IDisposable)) || type.IsAssignableTo(typeof(IAsyncDisposable));
}
