using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CheckAssertions
{
    public static class InvalidStateTests
    {
        [Theory(DisplayName = "InvalidState must throw an InvalidStateException when the specified condition is true.")]
        [InlineData("Baz")]
        [InlineData("Qux")]
        public static void ConditionTrue(string message)
        {
            Action act = () => Check.InvalidState(true, message);

            act.Should().Throw<InvalidStateException>()
               .And.Message.Should().Be(message);
        }

        [Fact(DisplayName = "InvalidState must not throw an exception when the specified condition is false.")]
        public static void ConditionFalse() => Check.InvalidState(false);
    }
}