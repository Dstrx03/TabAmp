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

        object storage = this switch
        {
            { IsNull: true } => error,
            { IsSingle: true } => AddAsSingle(error),
            { IsMany: true } => AddAsMany(error),
            _ => throw new UnreachableException()
        };

        return new(storage, _start, _length + 1);
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
        { IsMany: true } => AsMany.Slice(_start, _length),
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
}
