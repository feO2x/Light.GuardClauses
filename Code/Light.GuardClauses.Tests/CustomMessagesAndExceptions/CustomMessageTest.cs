using System;
using FluentAssertions;

namespace Light.GuardClauses.Tests.CustomMessagesAndExceptions
{
    public sealed class CustomMessageTest<TException> : IRunnableTest where TException : Exception
    {
        public readonly Action<string> CallAssertionWithCustomMessage;
        public readonly Type ExpectedExceptionType;

        public CustomMessageTest(Action<string> callAssertionWithCustomMessage)
        {
            callAssertionWithCustomMessage.MustNotBeNull(nameof(callAssertionWithCustomMessage));

            CallAssertionWithCustomMessage = callAssertionWithCustomMessage;
            ExpectedExceptionType = typeof (TException);
        }

        void IRunnableTest.RunTest()
        {
            const string message = "This is a custom message injected into an assertion.";

            Action act = () => CallAssertionWithCustomMessage(message);

            act.ShouldThrow<Exception>().Where(ex => ex.GetType() == ExpectedExceptionType).And.Message.Should().Contain(message);
        }

        public override string ToString()
        {
            return "Custom Message Test";
        }
    }
}