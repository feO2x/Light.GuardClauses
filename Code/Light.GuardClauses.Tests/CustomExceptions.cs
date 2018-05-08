using System;
using FluentAssertions;
using Xunit.Sdk;

namespace Light.GuardClauses.Tests
{
    public static class CustomExceptions
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

        public static void TestCustomException<T>(T invalidValue, Action<T, Func<T, Exception>> executeAssertion)
        {
            T capturedParameter = default;

            Exception ExceptionFactory(T parameter)
            {
                capturedParameter = parameter;
                return Exception;
            }

            try
            {
                executeAssertion(invalidValue, ExceptionFactory);
                throw new XunitException("The assertion should have thrown a custom exception at this point.");
            }
            catch (Exception exception)
            {
                exception.Should().BeSameAs(Exception);
                capturedParameter.Should().Be(invalidValue);
            }
        }
    }
}