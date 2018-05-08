using System;
using FluentAssertions;
using Xunit.Sdk;

namespace Light.GuardClauses.Tests
{
    public static class CustomMessages
    {
        public static void TestCustomMessage<TException>(Action<string> executeAssertion) where TException : Exception
        {
            try
            {
                executeAssertion(MetasyntacticVariables.Foo);
                throw new XunitException("The assertion should have thrown a custom exception at this point.");
            }
            catch (TException exception)
            {
                exception.Message.Should().BeSameAs(MetasyntacticVariables.Foo);
            }
        }
    }
}