using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class MustNotBeNullTests
    {
        [Fact]
        public static void ReferenceNull()
        {
            Action act = () => ((object) null).MustNotBeNull(MetasyntacticVariables.Foo);

            act.Should().Throw<ArgumentNullException>()
               .And.Message.Should().Contain($"{MetasyntacticVariables.Foo} must not be null.");
        }

        [Fact]
        public static void ReferenceNotNull() => string.Empty.MustNotBeNull().Should().BeSameAs(string.Empty);

        [Fact]
        public static void CustomException() =>
            CustomExceptions.TestCustomException(exceptionFactory => ((string) null).MustNotBeNull(exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            CustomMessages.TestCustomMessage<ArgumentNullException>(message => ((object) null).MustNotBeNull(message: message));
    }
}