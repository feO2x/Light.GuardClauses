using System;
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

#if NET8_0
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
#endif
}
