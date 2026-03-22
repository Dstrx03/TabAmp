using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TabAmp.Shared.Validation;

internal readonly ref struct Errors
{
    private readonly object? _storage;
    private readonly int _length;

    public Errors() => throw ThrowHelper.TODO(typeof(Errors));

    private Errors(object? storage, int length) =>
        (_storage, _length) = (storage, length);

    public int Count => _length;

    public bool Any => _length > 0;
    public bool IsEmpty => _length == 0;

    private bool IsSingle => _length == 1;
    private bool IsMany => _length > 1;

    private Exception AsSingle => (_storage as Exception)!;
    private List<Exception> AsMany => (_storage as List<Exception>)!;

    private Errors Add(Exception error)
    {
        ArgumentNullException.ThrowIfNull(error);

        object errors = this switch
        {
            { IsEmpty: true } => error,
            { IsSingle: true } => AddAsSingle(error),
            { IsMany: true } => AddAsMany(error),
            _ => throw new UnreachableException()
        };

        return new(errors, _length + 1);
    }

    private List<Exception> AddAsSingle(Exception error) => [AsSingle, error];

    private List<Exception> AddAsMany(Exception error)
    {
        var storage = AsMany;
        storage.Add(error);

        return storage;
    }

    public Enumerator GetEnumerator() => new();

    private static Errors Empty => new();

    public ref struct Enumerator
    {
        private readonly Errors _errors;
        private int _index = -1;

        public Enumerator() => throw ThrowHelper.TODO(typeof(Enumerator));

        private Enumerator(Errors errors) => _errors = errors;

        public readonly Exception Current => _errors switch
        {
            { IsSingle: true } => _errors.AsSingle,
            { IsMany: true } => _errors.AsMany[_index],
            _ => throw new UnreachableException()
        };

        public bool MoveNext()
        {
            if (_errors.IsEmpty)
                return false;

            return ++_index < _errors._length;
        }

        private static Enumerator FromErrors(Errors errors) => new(errors);
    }
}
