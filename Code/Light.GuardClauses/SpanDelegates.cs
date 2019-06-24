using System;

namespace Light.GuardClauses
{
#if (NETSTANDARD2_0 || NET45)
    /// <summary>
    /// Represents a delegate that receives a span and a value as parameters and that produces an exception.
    /// </summary>
    public delegate Exception SpanExceptionFactory<TItem, in T>(Span<TItem> span, T value);
#endif
}
