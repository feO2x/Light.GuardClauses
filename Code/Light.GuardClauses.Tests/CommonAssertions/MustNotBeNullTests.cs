using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class MustNotBeNullTests
    {
        [Fact]
        public static void ReferenceIsNull()
        {
            Action act = () => ((object) null).MustNotBeNull(Metasyntactic.Foo);

            act.Should().Throw<ArgumentNullException>()
               .And.Message.Should().Contain($"{Metasyntactic.Foo} must not be null.");
        }

        [Fact]
        public static void ReferenceIsNotNull() => string.Empty.MustNotBeNull().Should().BeSameAs(string.Empty);

        [Fact]
        public static void CustomException() =>
            Test.CustomException(exceptionFactory => ((string) null).MustNotBeNull(exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ArgumentNullException>(message => ((object) null).MustNotBeNull(message: message));
    }
}