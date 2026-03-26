using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TabAmp.Shared.Validation;

[DebuggerDisplay("Count = {Count}")]
public readonly ref struct Errors
{
    private readonly object? _storage;
    private readonly int _length;

    private Errors(object? storage, int length) =>
        (_storage, _length) = (storage, length);

    public int Count => _length;
    public bool IsEmpty => _length == 0;

    private bool IsSingle => _length == 1;
    private bool IsMany => _length > 1;

    private Exception AsSingle => (_storage as Exception)!;
    private List<Exception> AsMany => (_storage as List<Exception>)!;

    internal Errors Add(Exception error)
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

    public List<Exception> ToList() => this switch
    {
        { IsEmpty: true } => [],
        { IsSingle: true } => [AsSingle],
        { IsMany: true } => [.. AsMany.Take(_length)],
        _ => throw new UnreachableException()
    };

    public Enumerator GetEnumerator() => new(this);

    public ref struct Enumerator
    {
        private readonly Errors _errors;
        private int _index;

        internal Enumerator(Errors errors)
        {
            _errors = errors;
            _index = -1;
        }

        public readonly Exception Current => _errors switch
        {
            { IsEmpty: true } => null!,
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
    }
}
