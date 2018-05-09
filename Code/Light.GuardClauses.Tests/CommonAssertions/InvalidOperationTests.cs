using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class InvalidOperationTests
    {
        [Theory]
        [MetasyntacticVariablesData]
        public static void ConditionTrue(string message)
        {
            Action act = () => Guard.InvalidOperation(true, message);

            act.Should().Throw<InvalidOperationException>()
               .And.Message.Should().Be(message);
        }

        [Fact]
        public static void ConditionFalse()
        {
            Guard.InvalidOperation(false);
        }
    }
}