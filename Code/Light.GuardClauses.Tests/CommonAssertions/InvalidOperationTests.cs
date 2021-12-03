using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class InvalidOperationTests
    {
        [Theory]
        [DefaultVariablesData]
        public static void ConditionTrue(string message)
        {
            var act = () => Check.InvalidOperation(true, message);

            act.Should().Throw<InvalidOperationException>()
               .And.Message.Should().Be(message);
        }

        [Fact]
        public static void ConditionFalse()
        {
            Check.InvalidOperation(false);
        }
    }
}