using System;
using FluentAssertions;

namespace Light.GuardClauses.Tests.CustomMessagesAndExceptions
{
    public sealed class CustomExceptionTest : IRunnableTest
    {
        public readonly Action<Func<Exception>> CallAssertionWithCustomException;

        public CustomExceptionTest(Action<Func<Exception>> callAssertionWithCustomException)
        {
            callAssertionWithCustomException.MustNotBeNull(nameof(callAssertionWithCustomException));

            CallAssertionWithCustomException = callAssertionWithCustomException;
        }

        void IRunnableTest.RunTest()
        {
            var exception = new Exception();
            Func<Exception> createExceptionDelegate = () => exception;

            Action act = () => CallAssertionWithCustomException(createExceptionDelegate);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }

        public override string ToString()
        {
            return "CustomExceptionTest";
        }
    }
}