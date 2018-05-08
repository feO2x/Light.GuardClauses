using System;
using FluentAssertions;
using Xunit.Sdk;

namespace Light.GuardClauses.Tests
{
    public static class CustomExceptionOverload
    {
        private static readonly Exception Exception = new Exception();
        private static readonly Func<Exception> ExceptionFactory = () => Exception;

        public static void TestCustomException(Action<Func<Exception>> executeAssertion)
        {
            try
            {
                executeAssertion(ExceptionFactory);
                throw new XunitException("The assertion should have thrown a custom exception at this point.");
            }
            catch (Exception exception)
            {
                exception.Should().BeSameAs(Exception);
            }
        }
    }
}