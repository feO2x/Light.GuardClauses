
// PLEASE NOTICE: this code is auto-generated. Any changes you make to this file are going to be overwritten when this file is recreated by the code generator.
// Check "UriAssertions.MustHaveOneSchemeOf.csx" for details.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Light.GuardClauses.Exceptions;

// ReSharper disable PossibleMultipleEnumeration

namespace Light.GuardClauses
{
    public static partial class UriAssertions
    {
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws an <see cref="InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="InvalidUriSchemeException"/> (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, string[] schemes, string parameterName = null, string message = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Length; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, string[] schemes, Func<Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Length; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, string[] schemes, Func<Uri, Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Length; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory, parameter);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws an <see cref="InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="InvalidUriSchemeException"/> (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, List<string> schemes, string parameterName = null, string message = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, List<string> schemes, Func<Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, List<string> schemes, Func<Uri, Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory, parameter);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws an <see cref="InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="InvalidUriSchemeException"/> (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, IReadOnlyList<string> schemes, string parameterName = null, string message = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, IReadOnlyList<string> schemes, Func<Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, IReadOnlyList<string> schemes, Func<Uri, Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory, parameter);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws an <see cref="InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="InvalidUriSchemeException"/> (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, IList<string> schemes, string parameterName = null, string message = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, IList<string> schemes, Func<Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, IList<string> schemes, Func<Uri, Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory, parameter);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws an <see cref="InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="InvalidUriSchemeException"/> (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, ObservableCollection<string> schemes, string parameterName = null, string message = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, ObservableCollection<string> schemes, Func<Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, ObservableCollection<string> schemes, Func<Uri, Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory, parameter);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws an <see cref="InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="InvalidUriSchemeException"/> (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, Collection<string> schemes, string parameterName = null, string message = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, Collection<string> schemes, Func<Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, Collection<string> schemes, Func<Uri, Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory, parameter);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws an <see cref="InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="InvalidUriSchemeException"/> (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, ReadOnlyCollection<string> schemes, string parameterName = null, string message = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, ReadOnlyCollection<string> schemes, Func<Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, ReadOnlyCollection<string> schemes, Func<Uri, Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            for (var i = 0; i < schemes.Count; ++i)
            {
                var scheme = schemes[i];
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory, parameter);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws an <see cref="InvalidUriSchemeException"/>.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="InvalidUriSchemeException"/> (optional).</param>
        /// <exception cref="InvalidUriSchemeException">Thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, IEnumerable<string> schemes, string parameterName = null, string message = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            foreach (var scheme in schemes)
            {
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.UriMustHaveOneSchemeOf(parameter, schemes, parameterName, message);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, IEnumerable<string> schemes, Func<Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            foreach (var scheme in schemes)
            {
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory);
            return null;
        }
        
        /// <summary>
        /// Ensures that the parameter has one of the specified schemes, or otherwise throws your custom exception.
        /// </summary>
        /// <param name="parameter">The URI to be checked.</param>
        /// <param name="schemes">One of these schemes should apply to the URI.</param>
        /// <param name="exceptionFactory">The delegate that creates the exception to be thrown.</param>
        /// <param name="parameterName">The name of the parameter used for the <see cref="ArgumentNullException"/> and <see cref="RelativeUriException"/> (optional).</param>
        /// <exception cref="Exception">Your custom exception thrown when <paramref name="parameter"/> uses none of the specified schemes.</exception>
        /// <exception cref="RelativeUriException">Thrown when <paramref name="parameter"/> is relative and thus has no scheme.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri MustHaveOneSchemeOf(this Uri parameter, IEnumerable<string> schemes, Func<Uri, Exception> exceptionFactory, string parameterName = null)
        {
            parameter.MustBeAbsoluteUri(parameterName);
            foreach (var scheme in schemes)
            {
                if (parameter.Scheme.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                    return parameter;
            }
            
            Throw.CustomException(exceptionFactory, parameter);
            return null;
        }
        
    }
}
