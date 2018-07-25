using System;
using JetBrains.Annotations;
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
using System.Runtime.CompilerServices;
#endif
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses
{
    public static partial class Check
    {
        /*
         * -------------------------------------
         * Must Not Be Less Than
         * Must Be Greater Than or Equal To
         * -------------------------------------
         */
        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is not less than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be less than or equal to <paramref name="parameter"/>.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is less than <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeLessThan<T>(this T parameter, T other, string parameterName = null, string message = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName, message).CompareTo(other) < 0)
                Throw.MustNotBeLessThan(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is not less than the given <paramref name="other" /> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be less than or equal to <paramref name="parameter"/>.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is less than <paramref name="other" />, or when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeLessThan<T>(this T parameter, T other, Func<T, T, Exception> exceptionFactory, string parameterName = null) where T : IComparable<T>
        {
            if (parameter == null)
                // ReSharper disable once ExpressionIsAlwaysNull
                Throw.CustomException(exceptionFactory, parameter, other);
            if (parameter.CompareTo(other) < 0)
                Throw.CustomException(exceptionFactory, parameter, other);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is not less than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be less than or equal to <paramref name="parameter"/>.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is less than <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustBeGreaterThanOrEqualTo<T>(this T parameter, T other, string parameterName = null, string message = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) < 0)
                Throw.MustBeGreaterThanOrEqualTo(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is not less than the given <paramref name="other" /> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be less than or equal to <paramref name="parameter"/>.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is less than <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustBeGreaterThanOrEqualTo<T>(this T parameter, T other, Func<T, T, Exception> exceptionFactory, string parameterName = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) < 0)
                Throw.CustomException(exceptionFactory, parameter, other);
            return parameter;
        }

        /*
         * -------------------------------------
         * Must Be Less Than
         * Must Not Be Greater Than or Equal To
         * -------------------------------------
         */
        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is less than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be greater than <paramref name="parameter"/>.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is not less than <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustBeLessThan<T>(this T parameter, T other, string parameterName = null, string message = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) >= 0)
                Throw.MustBeLessThan(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is less than the given <paramref name="other" /> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be greater than <paramref name="parameter"/>.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is not less than <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustBeLessThan<T>(this T parameter, T other, Func<T, T, Exception> exceptionFactory, string parameterName = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) >= 0)
                Throw.CustomException(exceptionFactory, parameter, other);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is less than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be greater than <paramref name="parameter"/>.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is not less than <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeGreaterThanOrEqualTo<T>(this T parameter, T other, string parameterName = null, string message = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) >= 0)
                Throw.MustNotBeGreaterThanOrEqualTo(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is less than the given <paramref name="other" /> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be greater than <paramref name="parameter"/>.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is not less than <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeGreaterThanOrEqualTo<T>(this T parameter, T other, Func<T, T, Exception> exceptionFactory, string parameterName = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) >= 0)
                Throw.CustomException(exceptionFactory, parameter, other);
            return parameter;
        }

        /*
         * -------------------------------------
         * Must Be Greater Than
         * Must Not Be Less Than or Equal To
         * -------------------------------------
         */
        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is greater than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be less than <paramref name="parameter"/>.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is less than or equal to <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustBeGreaterThan<T>(this T parameter, T other, string parameterName = null, string message = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) <= 0)
                Throw.MustBeGreaterThan(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is greater than the given <paramref name="other" /> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be less than <paramref name="parameter"/>.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is less than or equal to <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustBeGreaterThan<T>(this T parameter, T other, Func<T, T, Exception> exceptionFactory, string parameterName = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) <= 0)
                Throw.CustomException(exceptionFactory, parameter, other);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is greater than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be less than <paramref name="parameter"/>.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is less than or equal to <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeLessThanOrEqualTo<T>(this T parameter, T other, string parameterName = null, string message = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) <= 0)
                Throw.MustNotBeLessThanOrEqualTo(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is greater than the given <paramref name="other" /> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be less than <paramref name="parameter"/>.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is less than or equal to <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeLessThanOrEqualTo<T>(this T parameter, T other, Func<T, T, Exception> exceptionFactory, string parameterName = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) <= 0)
                Throw.CustomException(exceptionFactory, parameter, other);
            return parameter;
        }

        /*
         * -------------------------------------
         * Must Not Be Greater Than
         * Must Be Less Than or Equal To
         * -------------------------------------
         */
        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is not greater than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be greater than or equal to <paramref name="parameter"/>.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is greater than <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeGreaterThan<T>(this T parameter, T other, string parameterName = null, string message = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) > 0)
                Throw.MustNotBeGreaterThan(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is not greater than the given <paramref name="other" /> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be greater than or equal to <paramref name="parameter"/>.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is greater than <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeGreaterThan<T>(this T parameter, T other, Func<T, T, Exception> exceptionFactory, string parameterName = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) > 0)
                Throw.CustomException(exceptionFactory, parameter, other);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is not greater than the given <paramref name="other" /> value, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be greater than or equal to <paramref name="parameter"/>.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the specified <paramref name="parameter" /> is greater than <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustBeLessThanOrEqualTo<T>(this T parameter, T other, string parameterName = null, string message = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) > 0)
                Throw.MustBeLessThanOrEqualTo(parameter, other, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that the specified <paramref name="parameter" /> is not greater than the given <paramref name="other" /> value, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="other">The boundary value that must be greater than or equal to <paramref name="parameter"/>.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="other"/> are passed to this delegate.</param>
        /// <param name="parameterName">The name of the parameter (optional). This is used for the <see cref="ArgumentNullException"/>.</param>
        /// <exception cref="Exception">Your custom exception thrown when the specified <paramref name="parameter" /> is greater than <paramref name="other" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustBeLessThanOrEqualTo<T>(this T parameter, T other, Func<T, T, Exception> exceptionFactory, string parameterName = null) where T : IComparable<T>
        {
            if (parameter.MustNotBeNullReference(parameterName).CompareTo(other) > 0)
                Throw.CustomException(exceptionFactory, parameter, other);
            return parameter;
        }

        /*
         * -------------------------------------
         * Ranges
         * -------------------------------------
         */
        /// <summary>
        /// Checks if the value is within the specified range.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="range">The range where <paramref name="parameter" /> must be in-between.</param>
        /// <returns>True if the parameter is within the specified range, else false.</returns>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsIn<T>(this T parameter, Range<T> range) where T : IComparable<T> => range.IsValueWithinRange(parameter);

        /// <summary>
        /// Checks if the value is not within the specified range.
        /// </summary>
        /// <param name="parameter">The comparable to be checked.</param>
        /// <param name="range">The range where <paramref name="parameter" /> must not be in-between.</param>
        /// <returns>True if the parameter is not within the specified range, else false.</returns>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool IsNotIn<T>(this T parameter, Range<T> range) where T : IComparable<T> => !range.IsValueWithinRange(parameter);

        /// <summary>
        /// Ensures that <paramref name="parameter" /> is within the specified range, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="range">The range where <paramref name="parameter" /> must be in-between.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="parameter" /> is not within <paramref name="range" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustBeIn<T>(this T parameter, Range<T> range, string parameterName = null, string message = null) where T : IComparable<T>
        {
            if (!range.IsValueWithinRange(parameter))
                Throw.MustBeInRange(parameter, range, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter" /> is within the specified range, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="range">The range where <paramref name="parameter" /> must be in-between.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="range"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is not within <paramref name="range" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustBeIn<T>(this T parameter, Range<T> range, Func<T, Range<T>, Exception> exceptionFactory) where T : IComparable<T>
        {
            if (!range.IsValueWithinRange(parameter))
                Throw.CustomException(exceptionFactory, parameter, range);
            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter" /> is not within the specified range, or otherwise throws an <see cref="ArgumentOutOfRangeException" />.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to be checked.</typeparam>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="range">The range where <paramref name="parameter" /> must not be in-between.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be passed to the <see cref="ArgumentOutOfRangeException" /> (optional).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="parameter" /> is within <paramref name="range" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeIn<T>(this T parameter, Range<T> range, string parameterName = null, string message = null) where T : IComparable<T>
        {
            if (range.IsValueWithinRange(parameter))
                Throw.MustNotBeInRange(parameter, range, parameterName, message);
            return parameter;
        }

        /// <summary>
        /// Ensures that <paramref name="parameter" /> is not within the specified range, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The parameter to be checked.</param>
        /// <param name="range">The range where <paramref name="parameter" /> must not be in-between.</param>
        /// <param name="exceptionFactory">The delegate that creates your custom exception. <paramref name="parameter"/> and <paramref name="range"/> are passed to this delegate.</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter" /> is within <paramref name="range" />.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
#if (NETSTANDARD2_0 || NETSTANDARD1_0 || NET45 || SILVERLIGHT)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull; exceptionFactory:null => halt")]
        public static T MustNotBeIn<T>(this T parameter, Range<T> range, Func<T, Range<T>, Exception> exceptionFactory) where T : IComparable<T>
        {
            if (range.IsValueWithinRange(parameter))
                Throw.CustomException(exceptionFactory, parameter, range);
            return parameter;
        }
    }
}
