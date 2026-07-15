using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions;

public static class IsEmptyGuidTests
{
    [Fact]
    public static void EmptyGuid() => Guid.Empty.IsEmpty().Should().BeTrue();

    [Fact]
    public static void NonEmptyGuid() => Guid.NewGuid().IsEmpty().Should().BeFalse();
}
