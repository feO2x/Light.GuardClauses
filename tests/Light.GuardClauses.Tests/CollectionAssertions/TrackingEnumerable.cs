#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;

namespace Light.GuardClauses.Tests.CollectionAssertions;

internal sealed class TrackingEnumerable<T> : IEnumerable<T>
{
    private readonly IReadOnlyList<T> _items;
    private readonly bool _singleUse;

    public TrackingEnumerable(IReadOnlyList<T> items, bool singleUse = true)
    {
        _items = items;
        _singleUse = singleUse;
    }

    public int EnumeratorCount { get; private set; }

    public int MoveNextCount { get; private set; }

    public int DisposeCount { get; private set; }

    public IEnumerator<T> GetEnumerator()
    {
        ++EnumeratorCount;
        if (_singleUse && EnumeratorCount > 1)
        {
            throw new InvalidOperationException("This enumerable can only be observed once.");
        }

        return new TrackingEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private sealed class TrackingEnumerator : IEnumerator<T>
    {
        private readonly TrackingEnumerable<T> _owner;
        private int _position = -1;

        public TrackingEnumerator(TrackingEnumerable<T> owner) => _owner = owner;

        public T Current => _owner._items[_position];

        object IEnumerator.Current => Current!;

        public bool MoveNext()
        {
            ++_owner.MoveNextCount;
            ++_position;
            return _position < _owner._items.Count;
        }

        public void Reset() => throw new NotSupportedException();

        public void Dispose() => ++_owner.DisposeCount;
    }
}

internal sealed class IndexableOnlyList<T> : IList<T>, IList
{
    private readonly IReadOnlyList<T> _items;

    public IndexableOnlyList(IReadOnlyList<T> items) => _items = items;

    object? IList.this[int index]
    {
        get => _items[index];
        set => throw new NotSupportedException();
    }

    bool IList.IsFixedSize => true;

    bool IList.IsReadOnly => true;

    bool ICollection.IsSynchronized => false;

    object ICollection.SyncRoot => this;

    int IList.Add(object? value) => throw new NotSupportedException();

    bool IList.Contains(object? value) => throw new NotSupportedException();

    int IList.IndexOf(object? value) => throw new NotSupportedException();

    void IList.Insert(int index, object? value) => throw new NotSupportedException();

    void IList.Remove(object? value) => throw new NotSupportedException();

    void ICollection.CopyTo(Array array, int index) => throw new NotSupportedException();

    public T this[int index]
    {
        get => _items[index];
        set => throw new NotSupportedException();
    }

    public int Count => _items.Count;

    public bool IsReadOnly => true;

    public IEnumerator<T> GetEnumerator() =>
        throw new InvalidOperationException("The indexable fast path should not request an enumerator.");

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int IndexOf(T item) => throw new NotSupportedException();

    public void Insert(int index, T item) => throw new NotSupportedException();

    public void RemoveAt(int index) => throw new NotSupportedException();

    public void Add(T item) => throw new NotSupportedException();

    public void Clear() => throw new NotSupportedException();

    public bool Contains(T item) => throw new NotSupportedException();

    public void CopyTo(T[] array, int arrayIndex) => throw new NotSupportedException();

    public bool Remove(T item) => throw new NotSupportedException();
}

internal sealed class ReadOnlyIndexableOnlyList<T> : IReadOnlyList<T>
{
    private readonly IReadOnlyList<T> _items;

    public ReadOnlyIndexableOnlyList(IReadOnlyList<T> items) => _items = items;

    public T this[int index] => _items[index];

    public int Count => _items.Count;

    public IEnumerator<T> GetEnumerator() =>
        throw new InvalidOperationException("The indexable fast path should not request an enumerator.");

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
