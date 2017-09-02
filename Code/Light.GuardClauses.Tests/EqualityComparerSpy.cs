using System.Collections.Generic;

namespace Light.GuardClauses.Tests
{
    public sealed class EqualityComparerSpy<T> : IEqualityComparer<T>
    {
        private readonly bool _equalsReturnValue;
        private readonly int _hashCode;
        private int _equalsCallCount;
        private int _getHashCodeCallCount;

        public EqualityComparerSpy(bool equalsReturnValue = true, int hashCode = 0)
        {
            _hashCode = hashCode;
            _equalsReturnValue = equalsReturnValue;
        }

        public int EqualsCallCount => _equalsCallCount;
        public int GetHashCodeCallCount => _getHashCodeCallCount;

        public bool Equals(T x, T y)
        {
            _equalsCallCount++;
            return _equalsReturnValue;
        }

        public int GetHashCode(T obj)
        {
            _getHashCodeCallCount++;
            return _hashCode;
        }
    }
}