using System;
using FluentAssertions;

namespace Light.GuardClauses.Tests.CustomMessagesAndExceptions
{
    public sealed class CustomExceptionTest : IRunnableTest
    {
        public readonly Action<Exception> CallAssertionWithCustomException;

        public CustomExceptionTest(Action<Exception> callAssertionWithCustomException)
        {
            callAssertionWithCustomException.MustNotBeNull(nameof(callAssertionWithCustomException));

            CallAssertionWithCustomException = callAssertionWithCustomException;
        }

        void IRunnableTest.RunTest()
        {
            var exception = new Exception();

            Action act = () => CallAssertionWithCustomException(exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }

        public override string ToString()
        {
            return "CustomExceptionTest";
        }
    }
}