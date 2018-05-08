using System;

namespace Light.GuardClauses
{
    public abstract class Types
    {
        /// <summary>
        /// Gets the <see cref="FlagsAttribute" /> type.
        /// </summary>
        public static readonly Type FlagsAttributeType = typeof(FlagsAttribute);

        /// <summary>
        /// Gets the <see cref="ulong" /> type.
        /// </summary>
        public static readonly Type UInt64Type = typeof(ulong);

        /// <summary>
        /// Gets the <see cref="MulticastDelegate" /> type.
        /// </summary>
        public static readonly Type MulticastDelegateType = typeof(MulticastDelegate);
    }
}