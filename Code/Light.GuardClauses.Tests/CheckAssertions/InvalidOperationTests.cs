using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CheckAssertions
{
    public static class InvalidOperationTests
    {
        [Theory(DisplayName = "InvalidOperation must throw an InvalidOperationException when the specified condition is true.")]
        [InlineData("Foo")]
        [InlineData("Bar")]
        public static void ConditionTrue(string message)
        {
            Action act = () => Check.InvalidOperation(true, message);

            act.Should().Throw<InvalidOperationException>()
               .And.Message.Should().Be(message);
        }

        [Fact(DisplayName = "InvalidOperation must not throw an exception when the specified condition is false.")]
        public static void ConditionFalse()
        {
            Check.InvalidOperation(false);
        }
    }
}