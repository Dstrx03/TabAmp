using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TabAmp.Shared.Validation;

[DebuggerDisplay("Count = {Count}")]
public readonly ref struct Errors
{
    private readonly object? _storage;
    private readonly int _start;
    private readonly int _length;

    private Errors(object? storage, int start, int length)
    {
        _storage = storage;
        _start = start;
        _length = length;
    }

    public int Count => _length - _start;
    public bool IsEmpty => Count == 0;

    private bool IsNull => _length == 0;
    private bool IsSingle => _length == 1;
    private bool IsMany => _length > 1;

    private Exception AsSingle => (_storage as Exception)!;
    private List<Exception> AsMany => (_storage as List<Exception>)!;

    internal Errors Add(Exception error)
    {
        ArgumentNullException.ThrowIfNull(error);

        if (_length == int.MaxValue)
            throw ExceededSupportedLimitException();

        object storage = this switch
        {
            { IsNull: true } => error,
            { IsSingle: true } => AddAsSingle(error),
            { IsMany: true } => AddAsMany(error),
            _ => throw new UnreachableException()
        };

        return new(storage, start: _start, length: _length + 1);
    }

    private List<Exception> AddAsSingle(Exception error) => [AsSingle, error];

    private List<Exception> AddAsMany(Exception error)
    {
        var storage = AsMany;

        if (storage.Count != _length)
            throw StorageAsManyInconsistentStateException(storage, _length);

        storage.Add(error);

        return storage;
    }

    internal Errors ToInner() => new(_storage, start: _length, length: _length);

    internal Errors FromOuter(Errors outer)
    {
        if (outer._start > _start)
            throw OuterScopeCannotReferenceLaterErrorRangeThanCurrentScopeException(outer, _start);

        if (outer._length > _length)
            throw OuterScopeCannotContainMoreErrorsThanCurrentScopeException(outer, _length);

        var mustReferenceSameStorage = (outer.IsSingle && IsSingle) || outer.IsMany;

        if (mustReferenceSameStorage && outer._storage != _storage)
            throw OuterAndCurrentScopesMustReferenceSameStorageException();

        return new(_storage, start: outer._start, length: _length);
    }

    public List<Exception> ToList() => this switch
    {
        { IsEmpty: true } => [],
        { IsSingle: true } => [AsSingle],
        { IsMany: true } => AsMany.Slice(_start, Count),
        _ => throw new UnreachableException()
    };

    public Enumerator GetEnumerator() => new(this);

    public ref struct Enumerator
    {
        private readonly Errors _errors;

        private int _index;
        private Exception? _current;

        internal Enumerator(Errors errors)
        {
            _errors = errors;
            _index = errors._start;
        }

        public readonly Exception Current => _current!;

        public bool MoveNext()
        {
            if ((uint)_index < (uint)_errors._length)
            {
                _current = _errors switch
                {
                    { IsSingle: true } => _errors.AsSingle,
                    { IsMany: true } => _errors.AsMany[_index],
                    _ => throw new UnreachableException()
                };
                _index++;

                return true;
            }

            _current = null;
            _index = -1;

            return false;
        }
    }

    private static InvalidOperationException ExceededSupportedLimitException() =>
        new("The number of captured errors exceeded the supported limit.");

    private static InvalidOperationException StorageAsManyInconsistentStateException(List<Exception> storage, int length) =>
        new("The captured error collection is in an inconsistent state. " +
            $"Storage count: {storage.Count}. Expected length: {length}.");

    private static InvalidOperationException OuterScopeCannotReferenceLaterErrorRangeThanCurrentScopeException(Errors outer, int start) =>
        new("The outer scope cannot reference a later error range than the current scope. " +
            $"Outer start: {outer._start}. Current start: {start}.");

    private static InvalidOperationException OuterScopeCannotContainMoreErrorsThanCurrentScopeException(Errors outer, int length) =>
        new("The outer scope cannot contain more errors than the current scope. " +
            $"Outer length: {outer._length}. Current length: {length}.");

    private static InvalidOperationException OuterAndCurrentScopesMustReferenceSameStorageException() =>
        new("The outer and current scopes must reference the same error storage.");
}
