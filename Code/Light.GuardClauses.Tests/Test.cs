using System;
using FluentAssertions;
using Xunit.Sdk;

namespace Light.GuardClauses.Tests
{
    public static class Test
    {
        private static readonly ExceptionDummy Exception = new ExceptionDummy();
        private static readonly Func<Exception> ExceptionFactory = () => Exception;

        public static void CustomException(Action<Func<Exception>> executeAssertion)
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

        public static void CustomException<T>(T invalidValue, Action<T, Func<T, Exception>> executeAssertion)
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

        public static void CustomException<T1, T2>(T1 first, T2 second, Action<T1, T2, Func<T1, T2, Exception>> executeAssertion)
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

        public static void CustomException<T1, T2, T3>(T1 first, T2 second, T3 third, Action<T1, T2, T3, Func<T1, T2, T3, Exception>> executeAssertion)
        {
            T1 capturedFirst = default;
            T2 capturedSecond = default;
            T3 capturedThird = default;

            Exception ExceptionFactory(T1 x, T2 y, T3 z)
            {
                capturedFirst = x;
                capturedSecond = y;
                capturedThird = z;
                return Exception;
            }

            try
            {
                executeAssertion(first, second, third, ExceptionFactory);
                throw new XunitException("The assertion should have thrown a custom exception at this point.");
            }
            catch (ExceptionDummy exception)
            {
                exception.Should().BeSameAs(Exception);
                capturedFirst.Should().Be(first);
                capturedSecond.Should().Be(second);
                capturedThird.Should().Be(third);
            }
        }

        public static void CustomMessage<TException>(Action<string> executeAssertion) where TException : Exception
        {
            try
            {
                executeAssertion(Metasyntactic.Foo);
                throw new XunitException("The assertion should have thrown a custom exception at this point.");
            }
            catch (TException exception)
            {
                exception.Message.Should().BeSameAs(Metasyntactic.Foo);
            }
        }

        private sealed class ExceptionDummy : Exception { };
    }
}