using System;
using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Light.GuardClauses.ExceptionFactory;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using NotNullAttribute = System.Diagnostics.CodeAnalysis.NotNullAttribute;

namespace Light.GuardClauses;

public static partial class Check
{
    /// <summary>
    /// Ensures that the collection count is within the specified range, or otherwise throws an <see cref="InvalidCollectionCountException" />.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="range">The range in which the collection count must lie.</param>
    /// <param name="parameterName">The name of the parameter (optional).</param>
    /// <param name="message">The message that will be passed to the resulting exception (optional).</param>
    /// <returns>The original collection.</returns>
    /// <exception cref="InvalidCollectionCountException">Thrown when the collection count is not within <paramref name="range" />.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
    public static TCollection MustHaveCountIn<TCollection>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        Range<int> range,
        [CallerArgumentExpression("parameter")] string? parameterName = null,
        string? message = null
    ) where TCollection : class, IEnumerable
    {
        var actualCount = parameter.Count(parameterName, message);
        if (!range.IsValueWithinRange(actualCount))
        {
            Throw.CollectionCountNotInRange(parameter, actualCount, range, parameterName, message);
        }

        return parameter;
    }

    /// <summary>
    /// Ensures that the collection count is within the specified range, or otherwise throws your custom exception.
    /// </summary>
    /// <param name="parameter">The collection to be checked.</param>
    /// <param name="range">The range in which the collection count must lie.</param>
    /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter" /> and <paramref name="range" /> are passed to this delegate.</param>
    /// <returns>The original collection.</returns>
    /// <exception cref="Exception">Your custom exception thrown when the collection is null or its count is not within <paramref name="range" />.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
    public static TCollection MustHaveCountIn<TCollection>(
        [NotNull] [ValidatedNotNull] this TCollection? parameter,
        Range<int> range,
        Func<TCollection?, Range<int>, Exception> exceptionFactory
    ) where TCollection : class, IEnumerable
    {
        if (parameter is null || !range.IsValueWithinRange(parameter.Count()))
        {
            Throw.CustomException(exceptionFactory, parameter, range);
        }

        return parameter;
    }
}
