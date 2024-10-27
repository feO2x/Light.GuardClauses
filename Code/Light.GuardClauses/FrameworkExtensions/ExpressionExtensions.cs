using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Light.GuardClauses.FrameworkExtensions;

/// <summary>
/// Provides extension methods for <see cref="Expression" /> instances.
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Extracts the <see cref="PropertyInfo" /> from an expression of the shape "object => object.Property".
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="expression">The expression where the property info will be extracted from.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="expression" /> is null.</exception>
    /// <exception cref="ArgumentException">
    /// Throw when the <paramref name="expression" /> is not of the shape "object => object.Property".
    /// </exception>
    // ReSharper disable once RedundantNullableFlowAttribute - NotNull is not redundant, see Issue72NotNullAttributeTests
    public static PropertyInfo ExtractProperty<T, TProperty>([NotNull, ValidatedNotNull] this Expression<Func<T, TProperty>> expression)
    {
        expression.MustNotBeNull(nameof(expression));

        var memberExpression = expression.Body as MemberExpression;
        if (!(memberExpression?.Member is PropertyInfo propertyInfo))
            throw new ArgumentException("The specified expression is not valid. Please use an expression like the following one: o => o.Property", nameof(expression));

        return propertyInfo;
    }

    /// <summary>
    /// Extracts the <see cref="FieldInfo" /> from an expression of the shape "object => object.Field".
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <typeparam name="TField">The type of the field.</typeparam>
    /// <param name="expression">The expression where the field info will be extracted from.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="expression" /> is null.</exception>
    /// <exception cref="ArgumentException">
    /// Throw when the <paramref name="expression" /> is not of the shape "object => object.Field".
    /// </exception>
    // ReSharper disable once RedundantNullableFlowAttribute - NotNull is not redundant, see Issue72NotNullAttributeTests
    public static FieldInfo ExtractField<T, TField>([NotNull, ValidatedNotNull] this Expression<Func<T, TField>> expression)
    {
        expression.MustNotBeNull(nameof(expression));

        var memberExpression = expression.Body as MemberExpression;
        if (!(memberExpression?.Member is FieldInfo fieldInfo))
            throw new ArgumentException("The specified expression is not valid. Please use an expression like the following one: o => o.Field", nameof(expression));

        return fieldInfo;
    }
}