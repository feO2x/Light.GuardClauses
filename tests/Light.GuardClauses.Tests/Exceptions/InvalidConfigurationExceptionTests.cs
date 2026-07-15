using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.Exceptions;

public static class InvalidConfigurationExceptionTests
{
    [Fact]
    public static void MessageAndInnerExceptionArePassedToBaseClass()
    {
        var innerException = new ArgumentException("inner");

        var exception = new InvalidConfigurationException("outer", innerException);

        exception.Message.Should().Be("outer");
        exception.InnerException.Should().BeSameAs(innerException);
    }
}
