using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions;

public static class InvalidStateTests
{
    [Theory]
    [DefaultVariablesData]
    public static void ConditionTrue(string message)
    {
        var act = () => Check.InvalidState(true, message);

        act.Should().Throw<InvalidStateException>()
           .And.Message.Should().Be(message);
    }

    [Fact]
    public static void ConditionFalse() => Check.InvalidState(false);
}