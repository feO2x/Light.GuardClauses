using System.Collections.Generic;

namespace Light.GuardClauses.Tests;

public sealed class EqualityComparerStub<T> : IEqualityComparer<T>
{
    private readonly bool _equalsReturnValue;

    public EqualityComparerStub(bool equalsReturnValue) => _equalsReturnValue = equalsReturnValue;

    public bool Equals(T x, T y) => _equalsReturnValue;

    public int GetHashCode(T obj) => 0;
}