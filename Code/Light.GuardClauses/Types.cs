using System;

namespace Light.GuardClauses
{
    /// <summary>
    /// This class caches <see cref="Type" /> instances to avoid use of the typeof operator.
    /// </summary>
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
    }
}