using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertions;

public static class IsOpenConstructedGenericTypeTests
{
    [Fact]
    public static void NonGenericType() => 
        GenericTypeKinds.NonGenericType.IsOpenConstructedGenericType().Should().BeFalse();

    [Fact]
    public static void ClosedConstructedType() => 
        GenericTypeKinds.ClosedConstructedGenericType.IsOpenConstructedGenericType().Should().BeFalse();

    [Fact]
    public static void GenericTypeDefinition() => 
        GenericTypeKinds.GenericTypeDefinition.IsOpenConstructedGenericType().Should().BeFalse();

    [Fact]
    public static void OpenConstructedGenericType() => 
        GenericTypeKinds.OpenConstructedGenericType.IsOpenConstructedGenericType().Should().BeTrue();

    [Fact]
    public static void GenericTypeParameter() => 
        GenericTypeKinds.GenericTypeParameter.IsOpenConstructedGenericType().Should().BeFalse();

    [Fact]
    public static void ArgumentNull()
    {
        Action act = () => ((Type) null).IsOpenConstructedGenericType();

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("type");
    }
}