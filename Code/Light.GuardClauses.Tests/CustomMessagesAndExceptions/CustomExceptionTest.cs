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
            Exception CreateExceptionDelegate() => exception;

            Action act = () => CallAssertionWithCustomException(CreateExceptionDelegate);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }

        public override string ToString()
        {
            return "CustomExceptionTest";
        }
    }
}