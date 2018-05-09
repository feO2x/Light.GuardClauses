using System;
using FluentAssertions;
using Xunit.Sdk;

namespace Light.GuardClauses.Tests
{
    public static class CustomExceptions
    {
        private static readonly ExceptionDummy Exception = new ExceptionDummy();
        private static readonly Func<Exception> ExceptionFactory = () => Exception;

        public static void TestCustomException(Action<Func<Exception>> executeAssertion)
        {
            try
            {
                executeAssertion(ExceptionFactory);
                throw new XunitException("The assertion should have thrown a custom exception at this point.");
            }
            catch (ExceptionDummy exception)
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
            catch (ExceptionDummy exception)
            {
                exception.Should().BeSameAs(Exception);
                capturedParameter.Should().Be(invalidValue);
            }
        }

        public static void TestCustomException<T1, T2>(T1 first, T2 second, Action<T1, T2, Func<T1, T2, Exception>> executeAssertion)
        {
            T1 capturedFirst = default;
            T2 capturedSecond = default;

            Exception ExceptionFactory(T1 x, T2 y)
            {
                capturedFirst = x;
                capturedSecond = y;
                return Exception;
            }

            try
            {
                executeAssertion(first, second, ExceptionFactory);
                throw new XunitException("The assertion should have thrown a custom exception at this point.");
            }
            catch (ExceptionDummy exception)
            {
                exception.Should().BeSameAs(Exception);
                capturedFirst.Should().Be(first);
                capturedSecond.Should().Be(second);
            }
        }

        private sealed class ExceptionDummy : Exception { };
    }
}