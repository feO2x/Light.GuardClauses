using System;

namespace Light.GuardClauses.ExceptionFactory;

/// <summary>
/// Represents a delegate that receives a span and a value as parameters and that produces an exception.
/// </summary>
public delegate Exception SpanExceptionFactory<TItem, in T>(Span<TItem> span, T value);

/// <summary>
/// Represents a delegate that receives a read-only span and produces an exception.
/// </summary>
public delegate Exception ReadOnlySpanExceptionFactory<TItem>(ReadOnlySpan<TItem> span);

/// <summary>
/// Represents a delegate that receives a read-only span and a value as parameters and that produces an exception.
/// </summary>
public delegate Exception ReadOnlySpanExceptionFactory<TItem, in T>(ReadOnlySpan<TItem> span, T value);

/// <summary>
/// Represents a delegate that receives two spans and produces an exception.
/// </summary>
public delegate Exception ReadOnlySpansExceptionFactory<TItem>(ReadOnlySpan<TItem> span1, ReadOnlySpan<TItem> span2);

/// <summary>
/// Represents a delegate that receives two spans and a value as parameters and that produces an exception.
/// </summary>
public delegate Exception ReadOnlySpansExceptionFactory<TItem, in T>(
    ReadOnlySpan<TItem> span1,
    ReadOnlySpan<TItem> span2,
    T value
);
