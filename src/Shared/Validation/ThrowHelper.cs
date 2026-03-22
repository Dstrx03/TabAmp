using System;

namespace TabAmp.Shared.Validation;

internal static class ThrowHelper
{
    internal static InvalidOperationException TODO(Type type) => new("Public ctor is not allowed TODO... - " + type.Name);
}
