using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public sealed class IsEmailAddressTests
{
    [Theory]
    [ClassData(typeof(InvalidEmailAddressesWithNull))]
    public void IsNotValidEmailAddress(string email)
    {
        var isValid = email.IsEmailAddress();

        isValid.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(ValidEmailAddresses))]
    public void IsValidEmailAddress(string email)
    {
        var isValid = email.IsEmailAddress();

        isValid.Should().BeTrue();
    }

#if NET8_0_OR_GREATER
    [Theory]
    [ClassData(typeof(InvalidEmailAddressesWithNull))]
    public void IsNotValidEmailAddress_ReadOnlySpan(string email)
    {
        var span = new ReadOnlySpan<char>(email?.ToCharArray() ?? []);
        var isValid = span.IsEmailAddress();

        isValid.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(ValidEmailAddresses))]
    public void IsValidEmailAddress_ReadOnlySpan(string email)
    {
        var span = email.AsSpan();
        var isValid = span.IsEmailAddress();

        isValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddressesWithNull))]
    public void IsNotValidEmailAddress_Span(string email)
    {
        var span = new Span<char>(email?.ToCharArray() ?? []);
        var isValid = span.IsEmailAddress();

        isValid.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(ValidEmailAddresses))]
    public void IsValidEmailAddress_Span(string email)
    {
        var span = new Span<char>(email.ToCharArray());
        var isValid = span.IsEmailAddress();

        isValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddressesWithNull))]
    public void IsNotValidEmailAddress_Memory(string email)
    {
        var memory = email?.ToCharArray().AsMemory() ?? Memory<char>.Empty;
        var isValid = memory.IsEmailAddress();

        isValid.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(ValidEmailAddresses))]
    public void IsValidEmailAddress_Memory(string email)
    {
        var memory = email.ToCharArray().AsMemory();
        var isValid = memory.IsEmailAddress();

        isValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddressesWithNull))]
    public void IsNotValidEmailAddress_ReadOnlyMemory(string email)
    {
        var memory = new ReadOnlyMemory<char>(email?.ToCharArray() ?? []);
        var isValid = memory.IsEmailAddress();

        isValid.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(ValidEmailAddresses))]
    public void IsValidEmailAddress_ReadOnlyMemory(string email)
    {
        var memory = new ReadOnlyMemory<char>(email.ToCharArray());
        var isValid = memory.IsEmailAddress();

        isValid.Should().BeTrue();
    }

    private static readonly Regex CustomPattern = new (@"\A[a-z]+@[a-z]+\.com\z");

    [Theory]
    [InlineData("kenny@feo.com", true)]
    [InlineData("Kenny@Feo.com", false)]
    public void CustomPattern_Span(string email, bool expected) =>
        new Span<char>(email.ToCharArray()).IsEmailAddress(CustomPattern).Should().Be(expected);

    [Theory]
    [InlineData("kenny@feo.com", true)]
    [InlineData("Kenny@Feo.com", false)]
    public void CustomPattern_Memory(string email, bool expected) =>
        email.ToCharArray().AsMemory().IsEmailAddress(CustomPattern).Should().Be(expected);

    [Theory]
    [InlineData("kenny@feo.com", true)]
    [InlineData("Kenny@Feo.com", false)]
    public void CustomPattern_ReadOnlyMemory(string email, bool expected) =>
        new ReadOnlyMemory<char>(email.ToCharArray()).IsEmailAddress(CustomPattern).Should().Be(expected);

    [Fact]
    public void CustomPatternNull_Span()
    {
        var act = () => new Span<char>("foo@bar.com".ToCharArray()).IsEmailAddress(null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("emailAddressPattern");
    }

    [Fact]
    public void CustomPatternNull_Memory()
    {
        var act = () => "foo@bar.com".ToCharArray().AsMemory().IsEmailAddress(null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("emailAddressPattern");
    }

    [Fact]
    public void CustomPatternNull_ReadOnlyMemory()
    {
        var act = () => new ReadOnlyMemory<char>("foo@bar.com".ToCharArray()).IsEmailAddress(null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("emailAddressPattern");
    }
#endif
}
