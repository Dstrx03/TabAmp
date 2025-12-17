using System;

namespace TabAmp.Shared.Decorator.Core.Extensions;

internal static class TypeDisposableExtensions
{
    internal static bool IsDisposable(this Type type) => type.IsAssignableTo(typeof(IDisposable));
    internal static bool IsAsyncDisposable(this Type type) => type.IsAssignableTo(typeof(IAsyncDisposable));
    internal static bool RequiresDisposal(this Type type) => type.IsDisposable() || type.IsAsyncDisposable();
}
