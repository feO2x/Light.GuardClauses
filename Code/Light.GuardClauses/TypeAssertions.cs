using System;
using System.Collections.Generic;
using System.Reflection;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;

namespace Light.GuardClauses
{
    /// <summary>
    ///     Provides assertion extension methods for the <see cref="Type" /> and <see cref="TypeInfo" /> types.
    /// </summary>
    public static class TypeAssertions
    {
        /// <summary>
        ///     Checks if the two specified types are equivalent. This is true when 1) both types are equal or
        ///     when 2) one type is a constructed generic type and the other type is the corresponding generic type definition.
        /// </summary>
        /// <param name="type">The first type to be checked.</param>
        /// <param name="other">The other type to be checked.</param>
        /// <returns>
        ///     True if both types are null, or if both are equal, or if one type
        ///     is a constructed generic type and the other one is the corresponding generic type definition, else false.
        /// </returns>
        public static bool IsEquivalentTypeTo(this Type type, Type other)
        {
            if (ReferenceEquals(type, other)) return true;
            if (type is null || other is null) return false;

            if (type == other) return true;

            if (type.IsConstructedGenericType == other.IsConstructedGenericType)
                return false;

            if (type.IsConstructedGenericType)
                return type.GetGenericTypeDefinition() == other;
            return other.GetGenericTypeDefinition() == type;
        }

        /// <summary>
        ///     Ensures that the parameter type is equivalent to the specified other type. This is true when 1) both types are equal or
        ///     when 2) one type is a constructed generic type and the other type is the corresponding generic type definition.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="other">The other type that <paramref name="parameter" /> should be equivalent to.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is not equivalent to <paramref name="other" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is not equivalent to <paramref name="other" />.</exception>
        public static Type MustBeEquivalentTo(this Type parameter, Type other, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsEquivalentTypeTo(other))
                return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must be equivalent to \"{other?.ToString() ?? "null"}\", but it is not.", parameterName);
        }

        /// <summary>
        ///     Ensures that the parameter type is not equivalent to the specified other type. This is true when 1) both types are not equal and
        ///     when 2) one type is a constructed generic type and the other type is not the corresponding generic type definition.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="other">The other type that <paramref name="parameter" /> should be equivalent to.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is equivalent to <paramref name="other" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is equivalent to <paramref name="other" />.</exception>
        public static Type MustNotBeEquivalentTo(this Type parameter, Type other, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsEquivalentTypeTo(other) == false)
                return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must not be equivalent to \"{other?.ToString() ?? "null"}\", but it is.", parameterName);
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Checks if the specified type is a class. This is true when <see cref="TypeInfo.IsClass" />
        ///     returns true and the type is no delegate (i.e. its <see cref="TypeInfo.BaseType" /> is no
        ///     <see cref="MulticastDelegate" />).
        /// </summary>
        /// /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
#else
        /// <summary>
        ///     Checks if the specified type is a class. This is true when <see cref="Type.IsClass" />
        ///     returns true and the type is no delegate (i.e. its <see cref="Type.BaseType" /> is no
        ///     <see cref="MulticastDelegate" />).
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
#endif
        public static bool IsClass(this Type type)
        {
#if NETSTANDARD1_0
            return type.GetTypeInfo().IsClass();
#else
            return type.MustNotBeNull(nameof(type)).IsClass && type.BaseType != Types.MulticastDelegateType;
#endif
        }
#if NETSTANDARD1_0
        /// <summary>
        ///     Checks if the specified type info describes a class. This is true when <see cref="TypeInfo.IsClass" />
        ///     returns true and the type is no delegate (i.e. its <see cref="TypeInfo.BaseType" /> is no
        ///     <see cref="MulticastDelegate" />).
        /// </summary>
        /// <param name="typeInfo">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeInfo" /> is null.</exception>
        public static bool IsClass(this TypeInfo typeInfo)
        {
            return typeInfo.MustNotBeNull(nameof(typeInfo)).IsClass && typeInfo.BaseType != Types.MulticastDelegateType;
        }
#endif

        /// <summary>
        ///     Ensures that the specified type is a class (no delegate, interface, struct or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no class (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is no class.</exception>
        public static Type MustBeClass(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsClass())
                return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must be a class, but it is not.", parameterName);
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Ensures that the specified type info describes a class (no delegate, interface, struct or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type info to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no class (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is no class.</exception>
        public static TypeInfo MustBeClass(this TypeInfo parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsClass())
                return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter.AsType()}\" must be a class, but it is not.", parameterName);
        }
#endif

        /// <summary>
        ///     Ensures that the specified type is no class (but a delegate, interface, struct or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is a class (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is a class.</exception>
        public static Type MustNotBeClass(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsClass() == false)
                return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must not be a class, but it is.", parameterName);
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Ensures that the specified type info does not describe a class (but a delegate, interface, struct or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type info to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is a class (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is a class.</exception>
        public static TypeInfo MustNotBeClass(this TypeInfo parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsClass() == false)
                return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter.AsType()}\" must not be a class, but it is.", parameterName);
        }

        /// <summary>
        ///     Checks if the specified type is an interface. This is true when <see cref="TypeInfo.IsInterface" />
        ///     returns true.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsInterface(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }
#endif

        /// <summary>
        ///     Ensures that the specified type is an interface (no class, delegate, struct or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no interface (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is no interface.</exception>
        public static Type MustBeInterface(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
#if NETSTANDARD1_0
            if (parameter.IsInterface())
#else
            if (parameter.IsInterface)
#endif
                return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must be an interface, but it is not.", parameterName);
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Ensures that the specified type info describes an interface (no class, delegate, struct or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type info to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no interface (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is no interface.</exception>
        public static TypeInfo MustBeInterface(this TypeInfo parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsInterface) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter.AsType()}\" must be an interface, but it is not.", parameterName);
        }
#endif

        /// <summary>
        ///     Ensures that the specified type is no interface (but a class, delegate, struct or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is an interface (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is an interface.</exception>
        public static Type MustNotBeInterface(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

#if NETSTANDARD1_0
            if (parameter.IsInterface() == false)
#else
            if (parameter.IsInterface == false)
#endif
                return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must not be an interface, but it is.", parameterName);
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Ensures that the specified type info does not describe an interface (but a class, delegate, struct or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type info to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is an interface (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is an interface.</exception>
        public static TypeInfo MustNotBeInterface(this TypeInfo parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsInterface == false) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter.AsType()}\" must not be an interface, but it is.", parameterName);
        }

        /// <summary>
        ///     Checks if the specified type is a delegate. This is true when <see cref="TypeInfo.IsClass" />
        ///     returns true and <see cref="TypeInfo.BaseType" /> is the <see cref="MulticastDelegate" />) type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
#else
        /// <summary>
        ///     Checks if the specified type is a delegate. This is true when <see cref="Type.IsClass" />
        ///     returns true and <see cref="Type.BaseType" /> is the <see cref="MulticastDelegate" /> type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
#endif
        public static bool IsDelegate(this Type type)
        {
#if NETSTANDARD1_0
            return type.GetTypeInfo().IsDelegate();
#else
            return type.MustNotBeNull(nameof(type)).IsClass && type.BaseType == Types.MulticastDelegateType;
#endif
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Checks if the specified type info describes a delegate. This is true when <see cref="TypeInfo.IsClass" />
        ///     returns true and <see cref="TypeInfo.BaseType" /> is the <see cref="MulticastDelegate" />) type.
        /// </summary>
        /// <param name="typeInfo">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeInfo" /> is null.</exception>
        public static bool IsDelegate(this TypeInfo typeInfo)
        {
            return typeInfo.MustNotBeNull(nameof(typeInfo)).IsClass && typeInfo.BaseType == Types.MulticastDelegateType;
        }
#endif

        /// <summary>
        ///     Ensures that the specified type is a delegate (no class, interface, struct or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no delegate (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is no delegate.</exception>
        public static Type MustBeDelegate(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsDelegate()) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must be a delegate, but it is not.", parameterName);
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Ensures that the specified type info describes a delegate (no class, interface, struct or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type info to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no delegate (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is no delegate.</exception>
        public static TypeInfo MustBeDelegate(this TypeInfo parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsDelegate()) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must be a delegate, but it is not.", parameterName);
        }
#endif

        /// <summary>
        ///     Ensures that the specified type is not a delegate (but a class, interface, struct or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is a delegate (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is a delegate.</exception>
        public static Type MustNotBeDelegate(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsDelegate() == false) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must not be a delegate, but it is.", parameterName);
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Ensures that the specified type info does not describe a delegate (but a class, interface, struct or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type info to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is a delegate (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is a delegate.</exception>
        public static TypeInfo MustNotBeDelegate(this TypeInfo parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsDelegate() == false) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must not be a delegate, but it is.", parameterName);
        }

        /// <summary>
        ///     Checks if the specified type is a struct. This is true when <see cref="TypeInfo.IsValueType" />
        ///     returns true and <see cref="TypeInfo.IsEnum" /> returns false.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
#else
        /// <summary>
        ///     Checks if the specified type is a struct. This is true when <see cref="Type.IsValueType" />
        ///     returns true and <see cref="Type.IsEnum" /> returns false.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
#endif
        public static bool IsStruct(this Type type)
        {
#if NETSTANDARD1_0
            return type.GetTypeInfo().IsStruct();
#else
            return type.MustNotBeNull(nameof(type)).IsValueType && type.IsEnum == false;
#endif
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Checks if the specified type info describes a struct. This is true when <see cref="TypeInfo.IsValueType" />
        ///     returns true and <see cref="TypeInfo.IsEnum" /> returns false.
        /// </summary>
        /// <param name="typeInfo">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeInfo" /> is null.</exception>
        public static bool IsStruct(this TypeInfo typeInfo)
        {
            return typeInfo.MustNotBeNull(nameof(typeInfo)).IsValueType && typeInfo.IsEnum == false;
        }
#endif

        /// <summary>
        ///     Ensures that the specified type is a struct (no class, interface, delegate, or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no struct (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is no struct.</exception>
        public static Type MustBeStruct(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsStruct()) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must be a struct, but it is not.", parameterName);
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Ensures that the specified type info describes a struct (no class, interface, delegate, or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type info to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no struct (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is no struct.</exception>
        public static TypeInfo MustBeStruct(this TypeInfo parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsStruct()) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter.AsType()}\" must be a struct, but it is not.", parameterName);
        }
#endif

        /// <summary>
        ///     Ensures that the specified type is no struct (but a class, interface, delegate, or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is a struct (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is a struct.</exception>
        public static Type MustNotBeStruct(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsStruct() == false) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must not be a struct, but it is.", parameterName);
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Ensures that the specified type info does not describe a struct (but a class, interface, delegate, or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type info to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is a struct (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is a struct.</exception>
        public static TypeInfo MustNotBeStruct(this TypeInfo parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsStruct() == false) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter.AsType()}\" must not be a struct, but it is.", parameterName);
        }

        /// <summary>
        ///     Checks if the specified type is an enum. This is true when <see cref="TypeInfo.IsEnum" />
        ///     returns true.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }
#endif

        /// <summary>
        ///     Ensures that the specified type is an enum (no class, interface, delegate, or struct), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no enum (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is no enum.</exception>
        public static Type MustBeEnum(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

#if NETSTANDARD1_0
            if (parameter.IsEnum())
#else
            if (parameter.IsEnum)
#endif
                return parameter;

            throw exception?.Invoke() ?? throw new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must be an enum, but it is not.", parameterName);
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Ensures that the specified type info describes an enum (no class, interface, delegate, or struct), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type info to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no enum (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is no enum.</exception>
        public static TypeInfo MustBeEnum(this TypeInfo parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsEnum) return parameter;

            throw exception?.Invoke() ?? throw new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter.AsType()}\" must be an enum, but it is not.", parameterName);
        }
#endif

        /// <summary>
        ///     Ensures that the specified type is no enum (but a class, interface, delegate, or struct), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is an enum (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is an enum.</exception>
        public static Type MustNotBeEnum(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

#if NETSTANDARD1_0
            if (parameter.IsEnum() == false)
#else
            if (parameter.IsEnum == false)
#endif
                return parameter;

            throw exception?.Invoke() ?? throw new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must not be an enum, but it is.", parameterName);
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Ensures that the specified type info does not describe an enum (but a class, interface, delegate, or struct), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type info to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is an enum (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is an enum.</exception>
        public static TypeInfo MustNotBeEnum(this TypeInfo parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsEnum == false) return parameter;

            throw exception?.Invoke() ?? throw new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter.AsType()}\" must not be an enum, but it is.", parameterName);
        }

        /// <summary>
        ///     Checks if the specified type is a reference type. This is true when <see cref="TypeInfo.IsValueType" />
        ///     returns false.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        /// <returns>True if the specified type is a class, delegate, or interface, else false.</returns>
#else
        /// <summary>
        ///     Checks if the specified type is a reference type. This is true when <see cref="Type.IsValueType" />
        ///     returns false.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        /// <returns>True if the specified type is a class, delegate, or interface, else false.</returns>
#endif
        public static bool IsReferenceType(this Type type)
        {
#if NETSTANDARD1_0
            return type.GetTypeInfo().IsValueType == false;
#else
            return type.MustNotBeNull(nameof(type)).IsValueType == false;
#endif
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Checks if the specified type info describes a reference type. This is true when <see cref="TypeInfo.IsValueType" />
        ///     return false.
        /// </summary>
        /// <param name="typeInfo">The type to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeInfo" /> is null.</exception>
        /// <returns>True if the specified type is a class, delegate, or interface, else false.</returns>
        public static bool IsReferenceType(this TypeInfo typeInfo)
        {
            return typeInfo.MustNotBeNull(nameof(typeInfo)).IsValueType == false;
        }
#endif

        /// <summary>
        ///     Ensures that the specified type is a reference type (i.e. a class, interface, or delegate), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no reference type (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is no reference type.</exception>
        public static Type MustBeReferenceType(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsReferenceType()) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must be a reference type, but it is a value type.", parameterName);
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Ensures that the specified type info describes a reference type (i.e. a class, interface, or delegate), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type info to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no reference type (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is no reference type.</exception>
        public static TypeInfo MustBeReferenceType(this TypeInfo parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsReferenceType()) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter.AsType()}\" must be a reference type, but it is a value type.", parameterName);
        }

        /// <summary>
        ///     Checks if the specified type is a value type. This is true when the <see cref="TypeInfo.IsValueType" />
        ///     property returns true.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }
#endif


        /// <summary>
        ///     Ensures that the specified type is a value type (i.e. a struct or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no value type (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is no value type.</exception>
        public static Type MustBeValueType(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

#if NETSTANDARD1_0
            if (parameter.IsValueType())
#else
            if (parameter.IsValueType)
#endif
                return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must be a value type, but it is a reference type.", parameterName);
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Ensures that the specified type info describes a value type (i.e. a struct or enum), or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type info to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is no value type (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is no value type.</exception>
        public static TypeInfo MustBeValueType(this TypeInfo parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsValueType) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter.AsType()}\" must be a value type, but it is a reference type.", parameterName);
        }
#endif

        /// <summary>
        ///     Checks if the specified type derives from the other type. Internally, this method uses <see cref="IsEquivalentTypeTo" />
        ///     by default so that constructed generic types and their corresponding generic type definitions are regarded as equal.
        ///     If you don't want this default behavior, then provide a fitting instance as <paramref name="typeComparer" />.
        /// </summary>
        /// <param name="type">The type info to be checked.</param>
        /// <param name="baseClass">The base class that <paramref name="type" /> should derive from.</param>
        /// <param name="typeComparer">The equality comparer used to compare the base types (optional). When no value is specified, an instance of <see cref="EqualivalentTypeComparer" /> is used.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="baseClass" /> is null.</exception>
        /// <returns>True if <paramref name="type" /> derives directly or indirectly from <paramref name="baseClass" />, else false.</returns>
        public static bool IsDerivingFrom(this Type type, Type baseClass, IEqualityComparer<Type> typeComparer = null)
        {
            baseClass.MustNotBeNull(nameof(baseClass));
            if (baseClass.IsClass() == false) return false;

            typeComparer = typeComparer ?? EqualivalentTypeComparer.Instance;

#if NETSTANDARD1_0
            var currentBaseType = type.GetTypeInfo().BaseType;
#else
            var currentBaseType = type.MustNotBeNull(nameof(type)).BaseType;
#endif
            while (currentBaseType != null)
            {
                if (typeComparer.Equals(currentBaseType, baseClass))
                    return true;

#if NETSTANDARD1_0
                currentBaseType = currentBaseType.GetTypeInfo().BaseType;
#else
                currentBaseType = currentBaseType.BaseType;
#endif
            }
            return false;
        }

        /// <summary>
        ///     Ensures that the type derives from the specified <paramref name="baseClass" />, or otherwise throws a <see cref="TypeException" />.
        ///     By default, this method uses <see cref="IsEquivalentTypeTo" /> internally so that constructed generic types and their corrsponding
        ///     generic type definitions are regarded as equal. If you do not want this default behavior, then please provide a fitting
        ///     <paramref name="typeComparer" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="baseClass">The type describing the base class that <paramref name="parameter" /> should derive from.</param>
        /// <param name="typeComparer">The equality comparer that is used to check if two types are equal (optional). By default, an instance of <see cref="EqualivalentTypeComparer" /> is used.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not derive from <paramref name="baseClass" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="baseClass" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> does not derive from <paramref name="baseClass" />.</exception>
        public static Type MustDeriveFrom(this Type parameter, Type baseClass, IEqualityComparer<Type> typeComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            baseClass.MustNotBeNull(nameof(baseClass));

            if (parameter.IsDerivingFrom(baseClass, typeComparer)) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must derive from \"{baseClass}\", but it does not.", parameterName);
        }

        /// <summary>
        ///     Ensures that the type does not derive from the specified <paramref name="other" /> type, or otherwise throws a <see cref="TypeException" />.
        ///     By default, this method uses <see cref="IsEquivalentTypeTo" /> internally so that constructed generic types and their corrsponding
        ///     generic type definitions are regarded as equal. If you do not want this default behavior, then please provide a fitting
        ///     <paramref name="typeComparer" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="other">The type that <paramref name="parameter" /> must not derive from.</param>
        /// <param name="typeComparer">The equality comparer that is used to check if two types are equal (optional). By default, an instance of <see cref="EqualivalentTypeComparer" /> is used.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does derive from <paramref name="other" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="other" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> does derive from <paramref name="other" />.</exception>
        public static Type MustNotDeriveFrom(this Type parameter, Type other, IEqualityComparer<Type> typeComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            other.MustNotBeNull(nameof(other));

            if (parameter.IsDerivingFrom(other, typeComparer) == false) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must not derive from \"{other}\", but it does.", parameterName);
        }

        /// <summary>
        ///     Checks if the specified type implements the given interface type. Internally, this method uses <see cref="IsEquivalentTypeTo" />
        ///     so that constructed generic types and their corresponding generic type defintions are regarded as equal.
        ///     If you don't want this default behavior, then provide a fitting instance as <paramref name="typeComparer" />.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <param name="interfaceType">The interface type that <paramref name="type" /> should implement.</param>
        /// <param name="typeComparer">The equality comparer used to compare the interface types (optional). When no value is specified, an instance of <see cref="EqualivalentTypeComparer" /> is used.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="interfaceType" /> is null.</exception>
        /// <returns>True if <paramref name="type" /> implements <paramref name="interfaceType" /> directly or indirectly, else false.</returns>
        public static bool IsImplementing(this Type type, Type interfaceType, IEqualityComparer<Type> typeComparer = null)
        {
            interfaceType.MustNotBeNull(nameof(interfaceType));
#if NETSTANDARD1_0
            if (interfaceType.IsInterface() == false)
#else
            if (interfaceType.IsInterface == false)
#endif
                return false;

            typeComparer = typeComparer ?? EqualivalentTypeComparer.Instance;
#if NETSTANDARD1_0
            var implementedInterfaces = type.GetTypeInfo().ImplementedInterfaces.AsArray();
#else
            var implementedInterfaces = type.MustNotBeNull(nameof(type)).GetInterfaces();
#endif
            for (var i = 0; i < implementedInterfaces.Length; i++)
            {
                if (typeComparer.Equals(implementedInterfaces[i], interfaceType))
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     Ensures that the type implements the specified <paramref name="interfaceType" />, or otherwise throws a <see cref="TypeException" />.
        ///     By default, this method uses <see cref="IsEquivalentTypeTo" /> internally so that constructed generic types and their corrsponding
        ///     generic type definitions are regarded as equal. If you do not want this default behavior, then please provide a fitting
        ///     <paramref name="typeComparer" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="interfaceType">The type describing the interface that <paramref name="parameter" /> should implement.</param>
        /// <param name="typeComparer">The equality comparer that is used to check if two types are equal (optional). By default, an instance of <see cref="EqualivalentTypeComparer" /> is used.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not implement <paramref name="interfaceType" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="interfaceType" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> does not implement <paramref name="interfaceType" />.</exception>
        public static Type MustImplement(this Type parameter, Type interfaceType, IEqualityComparer<Type> typeComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            interfaceType.MustNotBeNull(nameof(interfaceType));

            if (parameter.IsImplementing(interfaceType, typeComparer)) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must implement \"{interfaceType}\", but it does not.", parameterName);
        }

        /// <summary>
        ///     Ensures that the type does not implement the specified <paramref name="other" /> type, or otherwise throws a <see cref="TypeException" />.
        ///     By default, this method uses <see cref="IsEquivalentTypeTo" /> internally so that constructed generic types and their corrsponding
        ///     generic type definitions are regarded as equal. If you do not want this default behavior, then please provide a fitting
        ///     <paramref name="typeComparer" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="other">The type that <paramref name="parameter" /> must not implement.</param>
        /// <param name="typeComparer">The equality comparer that is used to check if two types are equal (optional). By default, an instance of <see cref="EqualivalentTypeComparer" /> is used.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does implement <paramref name="other" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="other" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> does implement <paramref name="other" />.</exception>
        public static Type MustNotImplement(this Type parameter, Type other, IEqualityComparer<Type> typeComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            other.MustNotBeNull(nameof(other));

            if (parameter.IsImplementing(other, typeComparer) == false) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must not implement \"{other}\", but it does.", parameterName);
        }

        /// <summary>
        ///     Checks if the given type derives from the specified base class or interface type. Internally, this method uses <see cref="IsEquivalentTypeTo" />
        ///     so that constructed generic types and their corresponding generic type defintions are regarded as equal.
        ///     If you don't want this default behavior, then provide a fitting instance as <paramref name="typeComparer" />.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <param name="baseClassOrInterfaceType">The type describing an interface or base class that <paramref name="type" /> should derive from or implement.</param>
        /// <param name="typeComparer">The equality comparer used to compare the interface types (optional). When no value is specified, an instance of <see cref="EqualivalentTypeComparer" /> is used.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="baseClassOrInterfaceType" /> is null.</exception>
        public static bool IsDerivingFromOrImplementing(this Type type, Type baseClassOrInterfaceType, IEqualityComparer<Type> typeComparer = null)
        {
            return baseClassOrInterfaceType.MustNotBeNull(nameof(baseClassOrInterfaceType))
#if NETSTANDARD1_0
                                           .IsInterface()
#else
                                           .IsInterface
#endif
                       ? type.IsImplementing(baseClassOrInterfaceType, typeComparer)
                       : type.IsDerivingFrom(baseClassOrInterfaceType, typeComparer);
        }

        /// <summary>
        ///     Ensures that the type derives from or implements the specified <paramref name="baseClassOrInterfaceType" />, or otherwise throws a <see cref="TypeException" />.
        ///     By default, this method uses <see cref="IsEquivalentTypeTo" /> internally so that constructed generic types and their corrsponding
        ///     generic type definitions are regarded as equal. If you do not want this default behavior, then please provide a fitting
        ///     <paramref name="typeComparer" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="baseClassOrInterfaceType">The type that <paramref name="parameter" /> should derive from or implement.</param>
        /// <param name="typeComparer">The equality comparer that is used to check if two types are equal (optional). By default, an instance of <see cref="EqualivalentTypeComparer" /> is used.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> does not derive from or implement <paramref name="baseClassOrInterfaceType" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="baseClassOrInterfaceType" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> does not derive from or implement <paramref name="baseClassOrInterfaceType" />.</exception>
        public static Type MustDeriveFromOrImplement(this Type parameter, Type baseClassOrInterfaceType, IEqualityComparer<Type> typeComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            baseClassOrInterfaceType.MustNotBeNull(nameof(baseClassOrInterfaceType));

            if (parameter.IsDerivingFromOrImplementing(baseClassOrInterfaceType, typeComparer)) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must derive from or implement \"{baseClassOrInterfaceType}\", but it does not.", parameterName);
        }

        /// <summary>
        ///     Ensures that the type does not derive from or implement the specified <paramref name="other" /> type, or otherwise throws a <see cref="TypeException" />.
        ///     By default, this method uses <see cref="IsEquivalentTypeTo" /> internally so that constructed generic types and their corrsponding
        ///     generic type definitions are regarded as equal. If you do not want this default behavior, then please provide a fitting
        ///     <paramref name="typeComparer" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="other">The type that <paramref name="parameter" /> should not derive from or implement.</param>
        /// <param name="typeComparer">The equality comparer that is used to check if two types are equal (optional). By default, an instance of <see cref="EqualivalentTypeComparer" /> is used.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> derives from or implements <paramref name="other" /> (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or <paramref name="other" /> is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> derives from or implements <paramref name="other" />.</exception>
        public static Type MustNotDeriveFromOrImplement(this Type parameter, Type other, IEqualityComparer<Type> typeComparer = null, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);
            other.MustNotBeNull(nameof(other));

            if (parameter.IsDerivingFromOrImplementing(other, typeComparer) == false) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must not derive from or implement \"{other}\", but it does.", parameterName);
        }

        /// <summary>
        ///     Checks if the given <paramref name="type" /> is equal to the specified <paramref name="otherType" /> or if it implements it. Internally, this
        ///     method uses <see cref="IsEquivalentTypeTo" /> so that constructed generic types and their corresponding generic type defintions are regarded as equal.
        ///     If you don't want this default behavior, then provide a fitting instance as <paramref name="typeComparer" />.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <param name="otherType">The type that is equivalent to <paramref name="type" /> or the interface type that <paramref name="type" /> implements.</param>
        /// <param name="typeComparer">The equality comparer used to compare the types (optional). When no value is specified, an instance of <see cref="EqualivalentTypeComparer" /> is used.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="otherType" /> is null.</exception>
        public static bool IsOrImplements(this Type type, Type otherType, IEqualityComparer<Type> typeComparer = null)
        {
            type.MustNotBeNull(nameof(type));
            otherType.MustNotBeNull(nameof(otherType));
            typeComparer = typeComparer ?? EqualivalentTypeComparer.Instance;

            return typeComparer.Equals(type, otherType) || type.IsImplementing(otherType, typeComparer);
        }

        /// <summary>
        ///     Checks if the given <paramref name="type" /> is equal to the specified <paramref name="otherType" /> or if it derives from it. Internally, this
        ///     method uses <see cref="IsEquivalentTypeTo" /> so that constructed generic types and their corresponding generic type defintions are regarded as equal.
        ///     If you don't want this default behavior, then provide a fitting instance as <paramref name="typeComparer" />.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <param name="otherType">The type that is equivalent to <paramref name="type" /> or the base class type where <paramref name="type" /> derives from.</param>
        /// <param name="typeComparer">The equality comparer used to compare the types (optional). When no value is specified, an instance of <see cref="EqualivalentTypeComparer" /> is used.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="otherType" /> is null.</exception>
        public static bool IsOrDerivesFrom(this Type type, Type otherType, IEqualityComparer<Type> typeComparer = null)
        {
            type.MustNotBeNull(nameof(type));
            otherType.MustNotBeNull(nameof(otherType));
            typeComparer = typeComparer ?? EqualivalentTypeComparer.Instance;

            return typeComparer.Equals(type, otherType) || type.IsDerivingFrom(otherType, typeComparer);
        }

        /// <summary>
        ///     Checks if the given <paramref name="type" /> is equal to the specified <paramref name="otherType" /> or if it derives from it or implements it.
        ///     Internally, this method uses <see cref="IsEquivalentTypeTo" /> so that constructed generic types and their corresponding generic type defintions
        ///     are regarded as equal. If you don't want this default behavior, then provide a fitting instance as <paramref name="typeComparer" />.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <param name="otherType">The type that is equivalent to <paramref name="type" /> or the base class type where <paramref name="type" /> derives from.</param>
        /// <param name="typeComparer">The equality comparer used to compare the types (optional). When no value is specified, an instance of <see cref="EqualivalentTypeComparer" /> is used.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> or <paramref name="otherType" /> is null.</exception>
        public static bool IsInInheritanceHierarchyOf(this Type type, Type otherType, IEqualityComparer<Type> typeComparer = null)
        {
            type.MustNotBeNull(nameof(type));
            otherType.MustNotBeNull(nameof(otherType));
            typeComparer = typeComparer ?? EqualivalentTypeComparer.Instance;

            return typeComparer.Equals(type, otherType) || type.IsDerivingFromOrImplementing(otherType, typeComparer);
        }

        /// <summary>
        ///     Checks if the given <paramref name="type" /> is a generic type that has no open generic parameters (e.g. typeof(List&lt;string&gt;)).
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsClosedConstructedGenericType(this Type type)
        {
#if NETSTANDARD1_0
            return type.GetTypeInfo().IsClosedConstructedGenericType();
#else
            return type.MustNotBeNull(nameof(type)).IsGenericType && type.ContainsGenericParameters == false;
#endif
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Checks if the given <paramref name="typeInfo" /> describes a generic type that has no open generic parameters (e.g. typeof(List&lt;string&gt;)).
        /// </summary>
        /// <param name="typeInfo">The type info to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeInfo" /> is null.</exception>
        public static bool IsClosedConstructedGenericType(this TypeInfo typeInfo)
        {
            return typeInfo.MustNotBeNull(nameof(typeInfo)).IsGenericType && typeInfo.ContainsGenericParameters == false;
        }
#endif

        /// <summary>
        ///     Checks if the given <paramref name="type" /> is a generic type definition (e.g. typeof(List&lt;&gt;)).
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsGenericTypeDefinition(this Type type)
        {
            return type.GetTypeInfo().IsGenericTypeDefinition;
        }

        /// <summary>
        ///     Checks if the given <paramref name="type" /> is a generic type that has open generic parameters,
        ///     but is no generic type definition (e.g. if you derive from Dictionary&lt;string, T&gt;).
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsOpenConstructedGenericType(this Type type)
        {
#if NETSTANDARD1_0
            return type.GetTypeInfo().IsOpenConstructedGenericType();
#else
            return type.MustNotBeNull(nameof(type)).IsGenericType &&
                   type.ContainsGenericParameters &&
                   type.IsGenericTypeDefinition == false;

#endif
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Checks if the given <paramref name="typeInfo" /> describes a generic type that has open generic parameters,
        ///     but is no generic type definition (e.g. if you derive from Dictionary&lt;string, T&gt;).
        /// </summary>
        /// <param name="typeInfo">The type info to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeInfo" /> is null.</exception>
        public static bool IsOpenConstructedGenericType(this TypeInfo typeInfo)
        {
            return typeInfo.MustNotBeNull(nameof(typeInfo)).IsGenericType &&
                   typeInfo.ContainsGenericParameters &&
                   typeInfo.IsGenericTypeDefinition == false;
        }
#endif

        /// <summary>
        ///     Checks if the given <paramref name="type" /> is a generic type parameter (e.g the T in List&lt;T&gt;)
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsGenericTypeParameter(this Type type)
        {
            return type.GetTypeInfo().IsGenericParameter;
        }

        /// <summary>
        ///     Checks if the specified type is an abstract base class or interface.
        /// </summary>
        /// <param name="type">The type to be checked.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsAbstraction(this Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return typeInfo.IsInterface || typeInfo.IsClass && typeInfo.IsAbstract && typeInfo.IsSealed == false;
        }

        /// <summary>
        ///     Ensures that the specified type is an abstract base class or interface, or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">
        ///     The message that will be injected into the <see cref="TypeException" /> (optional).
        /// </param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is not an abstract base class or interface (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> or is null.</exception>
        /// <exception cref="TypeException">Thrown when <paramref name="parameter" /> is not an abstract base class or interface.</exception>
        public static Type MustBeAbstraction(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            if (parameter.MustNotBeNull(parameterName).IsAbstraction()) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must be an abstract base class or interface, but it is not.", parameterName);
        }

        /// <summary>
        ///     Checks if the given type is a static class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type" /> is null.</exception>
        public static bool IsStaticClass(this Type type)
        {
#if NETSTANDARD1_0
            return type.GetTypeInfo().IsStaticClass();
#else
            return type.MustNotBeNull(nameof(type)).IsClass && type.IsAbstract && type.IsSealed;
#endif
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Checks if the given type info describes a static class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="typeInfo" /> is null.</exception>
        public static bool IsStaticClass(this TypeInfo typeInfo)
        {
            typeInfo.MustNotBeNull(nameof(typeInfo));

            return typeInfo.IsClass && typeInfo.IsAbstract && typeInfo.IsSealed;
        }
#endif

        /// <summary>
        ///     Ensures that the specified type is a static class, or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="TypeException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is not a static class (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        public static Type MustBeStaticClass(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsStaticClass()) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must be a static class, but it is not.", parameterName);
        }

#if NETSTANDARD1_0
        /// <summary>
        ///     Ensures that the specified type info describes a static class, or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type info to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="TypeException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is not a static class (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        public static TypeInfo MustBeStaticClass(this TypeInfo parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsStaticClass()) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter.AsType()}\" must be a static class, but it is not.", parameterName);
        }
#endif

        /// <summary>
        ///     Ensures that the specified type is not a static class, or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="TypeException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is a static class (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        public static Type MustNotBeStaticClass(this Type parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsStaticClass() == false) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter}\" must not be a static class, but it is.", parameterName);
        }

#if (NETSTANDARD1_0)
        /// <summary>
        ///     Ensures that the specified type info does not describe a static class, or otherwise throws a <see cref="TypeException" />.
        /// </summary>
        /// <param name="parameter">The type info to be checked.</param>
        /// <param name="parameterName">The name of the parameter (optional).</param>
        /// <param name="message">The message that will be injected into the <see cref="TypeException" /> (optional).</param>
        /// <param name="exception">
        ///     The exception that is thrown when <paramref name="parameter" /> is a static class (optional).
        ///     Please note that <paramref name="message" /> and <paramref name="parameterName" /> are both ignored when you specify exception.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="parameter" /> is null.</exception>
        public static TypeInfo MustNotBeStaticClass(this TypeInfo parameter, string parameterName = null, string message = null, Func<Exception> exception = null)
        {
            parameter.MustNotBeNull(parameterName);

            if (parameter.IsStaticClass() == false) return parameter;

            throw exception?.Invoke() ?? new TypeException(message ?? $"{parameterName ?? "The type"} \"{parameter.AsType()}\" must not be a static class, but it is.", parameterName);
        }
#endif
    }
}