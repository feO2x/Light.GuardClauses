using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Light.GuardClauses.ExceptionFactory;

public static partial class Throw
{
    /// <summary>
    /// Throws the exception that is returned by <paramref name="exceptionFactory" />.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CustomException(Func<Exception> exceptionFactory) =>
        throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))();

    /// <summary>
    /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="parameter" /> is
    /// passed to <paramref name="exceptionFactory" />.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CustomException<T>(Func<T, Exception> exceptionFactory, T parameter) =>
        throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(parameter);

    /// <summary>
    /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="first" /> and
    /// <paramref name="second" /> are passed to <paramref name="exceptionFactory" />.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CustomException<T1, T2>(Func<T1, T2, Exception> exceptionFactory, T1 first, T2 second) =>
        throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(first, second);

    /// <summary>
    /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="first" />,
    /// <paramref name="second" />, and <paramref name="third" /> are passed to <paramref name="exceptionFactory" />.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CustomException<T1, T2, T3>(
        Func<T1, T2, T3, Exception> exceptionFactory,
        T1 first,
        T2 second,
        T3 third
    ) =>
        throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(first, second, third);

    /// <summary>
    /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="span" /> and
    /// <paramref name="value" /> are passed to <paramref name="exceptionFactory" />.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CustomSpanException<TItem, T>(
        SpanExceptionFactory<TItem, T> exceptionFactory,
        Span<TItem> span,
        T value
    ) =>
        throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory)).Invoke(span, value);

    /// <summary>
    /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="span" /> is
    /// passed to <paramref name="exceptionFactory" />.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CustomSpanException<TItem>(
        ReadOnlySpanExceptionFactory<TItem> exceptionFactory,
        ReadOnlySpan<TItem> span
    ) =>
        throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(span);

    /// <summary>
    /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="span" /> and
    /// <paramref name="value" /> are passed to <paramref name="exceptionFactory" />.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CustomSpanException<TItem, T>(
        ReadOnlySpanExceptionFactory<TItem, T> exceptionFactory,
        ReadOnlySpan<TItem> span,
        T value
    ) =>
        throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(span, value);

    /// <summary>
    /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="first" /> and
    /// <paramref name="second" /> are passed to <paramref name="exceptionFactory" />.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CustomSpanException<TItem>(
        ReadOnlySpansExceptionFactory<TItem> exceptionFactory,
        ReadOnlySpan<TItem> first,
        ReadOnlySpan<TItem> second
    ) =>
        throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(first, second);

    /// <summary>
    /// Throws the exception that is returned by <paramref name="exceptionFactory" />. <paramref name="first" />,
    /// <paramref name="second" />, and <paramref name="third" /> are passed to <paramref name="exceptionFactory" />.
    /// </summary>
    [ContractAnnotation("=> halt")]
    [DoesNotReturn]
    public static void CustomSpanException<TItem, T>(
        ReadOnlySpansExceptionFactory<TItem, T> exceptionFactory,
        ReadOnlySpan<TItem> first,
        ReadOnlySpan<TItem> second,
        T third
    ) =>
        throw exceptionFactory.MustNotBeNull(nameof(exceptionFactory))(first, second, third);
}
