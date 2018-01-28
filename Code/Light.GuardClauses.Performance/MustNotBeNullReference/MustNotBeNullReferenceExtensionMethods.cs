using System.Runtime.CompilerServices;
using Light.GuardClauses.Exceptions;

namespace Light.GuardClauses.Performance.MustNotBeNullReference
{
    public static class MustNotBeNullReferenceExtensionMethods
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullReferenceV1<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (default(T) == null)
            {
                if (parameter != null)
                    return parameter;

                Throw.ArgumentNull(parameterName, message);
                return default(T);
            }

            return parameter;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MustNotBeNullReferenceV2<T>(this T parameter, string parameterName = null, string message = null)
        {
            if (default(T) != null) return parameter;

            if (parameter != null)
                return parameter;

            Throw.ArgumentNull(parameterName, message);
            return default(T);
        }
    }
}