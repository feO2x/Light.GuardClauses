using System;

namespace Light.GuardClauses
{
    /// <summary>
    ///     The <see cref="Check" /> class is the entry point to using assertions in production code: it defines the two
    ///     static methods <see cref="That" /> and <see cref="Against" /> that form the most general entry point to the library.
    /// </summary>
    public static class Check
    {
        /// <summary>
        ///     Ensures that the specified <paramref name="assertionResult" /> is true, or otherwise throws the specified exception.
        /// </summary>
        /// <param name="assertionResult">The result of an assertion to be checked.</param>
        /// <param name="createException">The delegate that creates the exception to be thrown when the <paramref name="assertionResult" /> is false.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="createException" /> is null.</exception>
        public static void That(bool assertionResult, Func<Exception> createException)
        {
            createException.MustNotBeNull(nameof(createException));

            if (assertionResult == false)
                throw createException();
        }

        /// <summary>
        ///     Ensures that the specified <paramref name="assertionResult" /> is false, or otherwise throws the specified exception.
        /// </summary>
        /// <param name="assertionResult">The result of an assertion to be checked.</param>
        /// <param name="createException">The delegate that creates the exception to be thrown when the <paramref name="assertionResult" /> is true.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="createException" /> is null.</exception>
        public static void Against(bool assertionResult, Func<Exception> createException)
        {
            createException.MustNotBeNull(nameof(createException));

            if (assertionResult)
                throw createException();
        }
    }
}