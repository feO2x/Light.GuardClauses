using System;

namespace Light.GuardClauses
{
    /// <summary>
    ///     Encapsulates an object reference and ensures that it is not null.
    /// </summary>
    /// <typeparam name="T">The type this NotNull encapsulates. This must be a reference type.</typeparam>
    public struct NotNull<T> : IEquatable<NotNull<T>>, IEquatable<T> where T : class
    {
        /// <summary>
        ///     Gets the reference to the actual object.
        /// </summary>
        public readonly T Object;

        /// <summary>
        ///     Creates a new <see cref="NotNull{T}" />.
        /// </summary>
        /// <param name="object">The object reference that must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="object"/> is null.</exception>
        public NotNull(T @object)
        {
            @object.MustNotBeNull(nameof(@object), $"You initialized a NotNull with null for type \"{typeof (T).FullName}\".");

            Object = @object;
        }

        /// <summary>
        ///     Checks if the specified NotNull object equals the object that is referenced by this NotNull instance. The Equals call is forwarded to the other instance.
        /// </summary>
        /// <param name="other">The other NotNull instance whose object is compared to this one.</param>
        /// <returns>True if the forwared Equals call returned true, else false.</returns>
        public bool Equals(NotNull<T> other)
        {
            return other.Object.Equals(Object);
        }

        /// <summary>
        ///     Checks if the specified object is equal to the object that is referenced by this NotNull instance. The Equals call is forwared to the other instance.
        /// </summary>
        /// <param name="other">The object that is compared to the object referenced by this NotNull instance.</param>
        /// <returns>True if the forwared Equals call returned true, else false.</returns>
        public bool Equals(T other)
        {
            return other != null && other.Equals(Object);
        }

        /// <summary>
        ///     Returns the hash code of the object referenced by this NotNull instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Object.GetHashCode();
        }

        /// <summary>
        ///     Checks if the specified object is equal to the object that is referenced by this NotNull instance.
        /// </summary>
        /// <param name="obj">The object to be compared.</param>
        /// <returns>True if <paramref name="obj" /> can be casted to <typeparamref name="T" /> or <see cref="NotNull{T}" /> and the corresponding Equals implementation returns true, else false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var @object = obj as T;
            if (@object != null)
                return Equals(@object);

            try
            {
                return Equals((NotNull<T>) obj);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        /// <summary>
        ///     Returns the string representation of the object referenced by this NotNull instance.
        /// </summary>
        /// <returns>The string representation of the object referenced by this NotNull instance.</returns>
        public override string ToString()
        {
            return Object.ToString();
        }

        /// <summary>
        ///     Implicitly converts a <see cref="NotNull{T}" /> instance to <typeparamref name="T" />.
        /// </summary>
        public static implicit operator T(NotNull<T> notNull)
        {
            return notNull.Object;
        }

        /// <summary>
        ///     Implicitly converts a <typeparamref name="T" /> reference to an instance of <see cref="NotNull{T}" />.
        /// </summary>
        public static implicit operator NotNull<T>(T @object)
        {
            return new NotNull<T>(@object);
        }

        /// <summary>
        /// Checks if the two NotNull instances are equal.
        /// </summary>
        public static bool operator ==(NotNull<T> first, NotNull<T> second)
        {
            return first.GetHashCode() == second.GetHashCode() && first.Equals(second);
        }

        /// <summary>
        /// Checks if the two NotNull instances are not equal.
        /// </summary>
        public static bool operator !=(NotNull<T> first, NotNull<T> second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Checks if the NotNull instance and the object reference are equal.
        /// </summary>
        public static bool operator ==(NotNull<T> first, T second)
        {
            if (second != null)
                return first.GetHashCode() == second.GetHashCode() && first.Equals(second);

            return false;
        }

        /// <summary>
        /// Checks if the NotNull instance and the object reference are not equal.
        /// </summary>
        public static bool operator !=(NotNull<T> first, T second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Checks if the object reference and the NotNull instance are equal.
        /// </summary>
        public static bool operator ==(T first, NotNull<T> second)
        {
            if (first != null)
                return first.GetHashCode() == second.GetHashCode() && second.Equals(first);

            return false;
        }

        /// <summary>
        /// Checks if the object reference and the NotNull instance are not equal.
        /// </summary>
        public static bool operator !=(T first, NotNull<T> second)
        {
            return !(first == second);
        }
    }
}