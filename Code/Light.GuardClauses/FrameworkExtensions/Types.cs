using System;

namespace Light.GuardClauses.FrameworkExtensions
{
    /// <summary>
    ///     This class caches <see cref="Type" /> instances to avoid use of the typeof operator.
    /// </summary>
    public class Types
    {
        /// <summary>
        ///     Gets the <see cref="FlagsAttribute" /> type.
        /// </summary>
        public static readonly Type FlagsAttributeType = typeof(FlagsAttribute);

        /// <summary>
        /// Gets the <see cref="ulong"/> type.
        /// </summary>
        public static readonly Type UInt64Type = typeof(ulong);

        /// <summary>
        ///     This constructor is protected so that the <see cref="Types" /> class cannot be instantiated.
        /// </summary>
        protected Types() { }
    }
}