#nullable enable
using System;
using Light.GuardClauses.ExceptionFactory;

namespace Light.GuardClauses.Tests;

public delegate void ExecuteReadOnlySpanAssertion<T>(
    ReadOnlySpan<T> span,
    ReadOnlySpanExceptionFactory<T> exceptionFactory
);

public delegate void ExecuteSpanAssertion<T>(
    Span<T> span,
    ReadOnlySpanExceptionFactory<T> exceptionFactory
);

public delegate void ExecuteReadOnlySpanAssertion<TItem, TValue>(
    ReadOnlySpan<TItem> span,
    TValue additionalValue,
    ReadOnlySpanExceptionFactory<TItem, TValue> exceptionFactory
);

public delegate void ExecuteSpanAssertion<TItem, TValue>(
    Span<TItem> span,
    TValue additionalValue,
    ReadOnlySpanExceptionFactory<TItem, TValue> exceptionFactory
);
