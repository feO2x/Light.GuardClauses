using System;
using System.Diagnostics;

namespace Light.GuardClauses
{
    /// <summary>
    ///     The Check class is the entry point to using assertions in production code: it defines the CompileAssertionsSymbol
    ///     so that assertions can be included or excluded according to the build definitions of a project, and the two
    ///     static methods <see cref="That" /> and <see cref="Against" /> that form the most general entry point to the library.
    /// </summary>
    public static class Check
    {
        /// <summary>
        ///     The pre-compiler symbol that must be added to the Build settings of a project so that calls to the assertion
        ///     methods of this library are included in the target assembly. This value is "COMPILE_ASSERTIONS" (without quotation marks).
        /// </summary>
        public const string CompileAssertionsSymbol = "COMPILE_ASSERTIONS";

        /// <summary>
        ///     Checks that the specified <paramref name="assertionResult" /> is true, or otherwise throws the specified exception.
        /// </summary>
        /// <param name="assertionResult">The result of an assertions to be checked.</param>
        /// <param name="otherwiseCreateException">The delegate that creates the exception to be thrown.</param>
        [Conditional(CompileAssertionsSymbol)]
        public static void That(bool assertionResult, Func<Exception> otherwiseCreateException)
        {
            if (assertionResult == false)
                throw otherwiseCreateException();
        }

        /// <summary>
        ///     Checks that the specified <paramref name="assertionResult" /> is false, or otherwise throws the specified exception.
        /// </summary>
        /// <param name="assertionResult">The result of an assertion to be checked.</param>
        /// <param name="createException">The delegate that creates the exception to be thrown.</param>
        [Conditional(CompileAssertionsSymbol)]
        public static void Against(bool assertionResult, Func<Exception> createException)
        {
            if (assertionResult)
                throw createException();
        }
    }
}