using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class MustBeEmailAddressTests
{
    [Theory]
    [ClassData(typeof(ValidEmailAddresses))]
    public static void ValidEmailAddress(string emailAddress) =>
        emailAddress.MustBeEmailAddress().Should().BeSameAs(emailAddress);

    [Theory]
    [ClassData(typeof(InvalidEmailAddresses))]
    public static void InvalidEmailAddress(string emailAddress)
    {
        Action act = () => emailAddress.MustBeEmailAddress();

        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should().Contain(
                $"emailAddress must be a valid email address, but it actually is \"{emailAddress}\"."
            );
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddresses))]
    public static void InvalidEmailAddressArgumentName(string emailAddress)
    {
        Action act = () => emailAddress.MustBeEmailAddress(nameof(emailAddress));

        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should().Contain(
                $"emailAddress must be a valid email address, but it actually is \"{emailAddress}\"."
            );
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddresses))]
    public static void InvalidEmailAddressCustomMessage(string emailAddress)
    {
        const string customMessage = "This email address is not valid";

        Action act = () => emailAddress.MustBeEmailAddress(nameof(emailAddress), customMessage);

        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should().Contain(customMessage);
    }

    private static readonly Regex CustomRegex = new (@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    [Fact]
    public static void InvalidEmailCustomRegex()
    {
        const string email = "email@domain@domain.com";

        Action act = () => email.MustBeEmailAddress(CustomRegex);

        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should().Contain($"email must be a valid email address, but it actually is \"{email}\".");
    }

    [Fact]
    public static void EmailNull()
    {
        Action act = () => default(string).MustBeEmailAddress();

        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [MemberData(nameof(NullData))]
    public static void NullCustomRegex(string email, Regex regex)
    {
        Action act = () => email.MustBeEmailAddress(regex);

        act.Should().Throw<ArgumentNullException>();
    }

    public static readonly TheoryData<string, Regex> NullData =
        new ()
        {
            { null, CustomRegex },
            { "invalidEmailAddress", null },
        };

    [Fact]
    public static void CustomException() =>
        Test.CustomException(
            "email.domain.com",
            (input, exceptionFactory) => input.MustBeEmailAddress(exceptionFactory)
        );

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<InvalidEmailAddressException>(
            message => "#@%^%#$@#$@#.com".MustBeEmailAddress(message: message)
        );

    [Fact]
    public static void CustomExceptionCustomRegex() =>
        Test.CustomException(
            "invalidEmailAddress",
            CustomRegex,
            (i, r, exceptionFactory) => i.MustBeEmailAddress(r, exceptionFactory)
        );

    [Fact]
    public static void CustomMessageCustomRegex() =>
        Test.CustomMessage<InvalidEmailAddressException>(
            message => "invalidEmailAddress".MustBeEmailAddress(CustomRegex, message: message)
        );

    [Fact]
    public static void CallerArgumentExpression()
    {
        const string email = "This is not an email address";

        var act = () => email.MustBeEmailAddress();

        act.Should().Throw<InvalidEmailAddressException>()
           .WithParameterName(nameof(email));
    }

#if NET8_0
    [Theory]
    [ClassData(typeof(ValidEmailAddresses))]
    public static void ValidEmailAddress_ReadOnlySpan(string email)
    {
        var result = email.AsSpan().MustBeEmailAddress();
        result.ToString().Should().Be(email);
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddresses))]
    public static void InvalidEmailAddress_ReadOnlySpan(string email)
    {
        var act = () =>
        {
            var readOnlySpan = email.AsSpan();
            readOnlySpan.MustBeEmailAddress();
        };
        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should()
           .Contain($"readOnlySpan must be a valid email address, but it actually is \"{email}\".");
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddresses))]
    public static void InvalidEmailAddressArgumentName_ReadOnlySpan(string email)
    {
        Action act = () => email.AsSpan().MustBeEmailAddress(nameof(email));
        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should().Contain(nameof(email));
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddresses))]
    public static void InvalidEmailAddressCustomMessage_ReadOnlySpan(string email)
    {
        const string customMessage = "This email address is not valid";
        Action act = () => email.AsSpan().MustBeEmailAddress(nameof(email), customMessage);
        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should().Contain(customMessage);
    }

    [Fact]
    public static void CallerArgumentExpression_ReadOnlySpan()
    {
        var act = () =>
        {
            var email = "This is not an email address".AsSpan();
            email.MustBeEmailAddress();
        };
        act.Should().Throw<InvalidEmailAddressException>()
           .WithParameterName("email");
    }

    [Fact]
    public static void CustomException_ReadOnlySpan() =>
        Test.CustomSpanException(
            "email.domain.com".AsSpan(),
            (input, exceptionFactory) => input.MustBeEmailAddress(exceptionFactory)
        );

    [Fact]
    public static void CustomExceptionCustomRegex_ReadOnlySpan() =>
        Test.CustomSpanException(
            "invalidEmailAddress".AsSpan(),
            CustomRegex,
            (i, r, exceptionFactory) => i.MustBeEmailAddress(r, exceptionFactory)
        );

    [Fact]
    public static void CustomMessageCustomRegex_ReadOnlySpan() =>
        Test.CustomMessage<InvalidEmailAddressException>(
            message => "invalidEmailAddress".AsSpan().MustBeEmailAddress(CustomRegex, message: message)
        );

    // Tests for Span<char>
    [Theory]
    [ClassData(typeof(ValidEmailAddresses))]
    public static void ValidEmailAddress_Span(string email)
    {
        var emailChars = email.ToCharArray();
        var span = new Span<char>(emailChars);
        var result = span.MustBeEmailAddress();
        new string(result).Should().Be(email);
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddresses))]
    public static void InvalidEmailAddress_Span(string email)
    {
        var emailChars = email.ToCharArray();
        var act = () =>
        {
            var span = new Span<char>(emailChars);
            span.MustBeEmailAddress();
        };
        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should().Contain($"span must be a valid email address, but it actually is \"{email}\".");
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddresses))]
    public static void InvalidEmailAddressArgumentName_Span(string email)
    {
        var emailChars = email.ToCharArray();
        var act = () =>
        {
            var span = new Span<char>(emailChars);
            span.MustBeEmailAddress(nameof(span));
        };
        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should().Contain("span");
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddresses))]
    public static void InvalidEmailAddressCustomMessage_Span(string email)
    {
        const string customMessage = "This email address is not valid";
        var emailChars = email.ToCharArray();
        var act = () =>
        {
            var span = new Span<char>(emailChars);
            span.MustBeEmailAddress(nameof(span), customMessage);
        };
        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should().Contain(customMessage);
    }

    [Fact]
    public static void CallerArgumentExpression_Span()
    {
        var act = () =>
        {
            var emailChars = "This is not an email address".ToCharArray();
            var span = new Span<char>(emailChars);
            span.MustBeEmailAddress();
        };
        act.Should().Throw<InvalidEmailAddressException>()
           .WithParameterName("span");
    }

    [Fact]
    public static void CustomException_Span()
    {
        var emailChars = "email.domain.com".ToCharArray();
        var span = new Span<char>(emailChars);
        Test.CustomSpanException(
            span,
            (input, exceptionFactory) => input.MustBeEmailAddress(exceptionFactory)
        );
    }

    [Fact]
    public static void CustomExceptionCustomRegex_Span()
    {
        var emailChars = "invalidEmailAddress".ToCharArray();
        var span = new Span<char>(emailChars);
        Test.CustomSpanException(
            span,
            CustomRegex,
            (i, r, exceptionFactory) => i.MustBeEmailAddress(r, exceptionFactory)
        );
    }

    [Fact]
    public static void CustomMessageCustomRegex_Span() =>
        Test.CustomMessage<InvalidEmailAddressException>(
            message =>
            {
                var span = new Span<char>("invalidEmailAddress".ToCharArray());
                span.MustBeEmailAddress(CustomRegex, message: message);
            }
        );

    // Tests for Memory<char>
    [Theory]
    [ClassData(typeof(ValidEmailAddresses))]
    public static void ValidEmailAddress_Memory(string email)
    {
        var memory = email.ToCharArray().AsMemory();
        var result = memory.MustBeEmailAddress();
        result.ToString().Should().Be(email);
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddresses))]
    public static void InvalidEmailAddress_Memory(string email)
    {
        var memory = email.ToCharArray().AsMemory();
        Action act = () => memory.MustBeEmailAddress();
        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should().Contain($"memory must be a valid email address, but it actually is \"{email}\".");
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddresses))]
    public static void InvalidEmailAddressArgumentName_Memory(string email)
    {
        var memory = email.ToCharArray().AsMemory();
        Action act = () => memory.MustBeEmailAddress(nameof(memory));
        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should().Contain(nameof(memory));
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddresses))]
    public static void InvalidEmailAddressCustomMessage_Memory(string email)
    {
        const string customMessage = "This email address is not valid";
        var memory = email.ToCharArray().AsMemory();
        Action act = () => memory.MustBeEmailAddress(nameof(memory), customMessage);
        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should().Contain(customMessage);
    }

    [Fact]
    public static void CallerArgumentExpression_Memory()
    {
        var act = () =>
        {
            var memory = "This is not an email address".ToCharArray().AsMemory();
            memory.MustBeEmailAddress();
        };
        act.Should().Throw<InvalidEmailAddressException>()
           .WithParameterName("memory");
    }

    [Fact]
    public static void CustomException_Memory()
    {
        var memory = "email.domain.com".ToCharArray().AsMemory();
        Test.CustomMemoryException(
            memory,
            (input, exceptionFactory) => input.MustBeEmailAddress(exceptionFactory)
        );
    }

    [Fact]
    public static void CustomExceptionCustomRegex_Memory()
    {
        var memory = "invalidEmailAddress".ToCharArray().AsMemory();
        Test.CustomMemoryException(
            memory,
            CustomRegex,
            (i, r, exceptionFactory) => i.MustBeEmailAddress(r, exceptionFactory)
        );
    }

    [Fact]
    public static void CustomMessageCustomRegex_Memory()
    {
        var memory = "invalidEmailAddress".AsMemory();
        Test.CustomMessage<InvalidEmailAddressException>(
            message => memory.MustBeEmailAddress(CustomRegex, message: message)
        );
    }

    // Tests for ReadOnlyMemory<char>
    [Theory]
    [ClassData(typeof(ValidEmailAddresses))]
    public static void ValidEmailAddress_ReadOnlyMemory(string email)
    {
        var memory = email.AsMemory();
        var result = memory.MustBeEmailAddress();
        result.ToString().Should().Be(email);
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddresses))]
    public static void InvalidEmailAddress_ReadOnlyMemory(string email)
    {
        var memory = email.AsMemory();
        Action act = () => memory.MustBeEmailAddress();
        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should().StartWith($"memory must be a valid email address, but it actually is \"{email}\".");
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddresses))]
    public static void InvalidEmailAddressArgumentName_ReadOnlyMemory(string email)
    {
        var readOnlyMemory = email.AsMemory();
        Action act = () => readOnlyMemory.MustBeEmailAddress(nameof(readOnlyMemory));
        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should().Contain(nameof(readOnlyMemory));
    }

    [Theory]
    [ClassData(typeof(InvalidEmailAddresses))]
    public static void InvalidEmailAddressCustomMessage_ReadOnlyMemory(string email)
    {
        const string customMessage = "This email address is not valid";
        var readOnlyMemory = email.AsMemory();
        Action act = () => readOnlyMemory.MustBeEmailAddress(nameof(readOnlyMemory), customMessage);
        act.Should().Throw<InvalidEmailAddressException>()
           .And.Message.Should().Contain(customMessage);
    }

    [Fact]
    public static void CallerArgumentExpression_ReadOnlyMemory()
    {
        var act = () =>
        {
            var readOnlyMemory = "This is not an email address".AsMemory();
            readOnlyMemory.MustBeEmailAddress();
        };
        act.Should().Throw<InvalidEmailAddressException>()
           .WithParameterName("readOnlyMemory");
    }

    [Fact]
    public static void CustomException_ReadOnlyMemory()
    {
        var readOnlyMemory = "email.domain.com".AsMemory();
        Test.CustomMemoryException(
            readOnlyMemory,
            (input, exceptionFactory) => input.MustBeEmailAddress(exceptionFactory)
        );
    }

    [Fact]
    public static void CustomExceptionCustomRegex_ReadOnlyMemory()
    {
        var readOnlyMemory = "invalidEmailAddress".AsMemory();
        Test.CustomMemoryException(
            readOnlyMemory,
            CustomRegex,
            (i, r, exceptionFactory) => i.MustBeEmailAddress(r, exceptionFactory)
        );
    }

    [Fact]
    public static void CustomMessageCustomRegex_ReadOnlyMemory()
    {
        var readOnlyMemory = "invalidEmailAddress".AsMemory();
        Test.CustomMessage<InvalidEmailAddressException>(
            message => readOnlyMemory.MustBeEmailAddress(CustomRegex, message: message)
        );
    }
#endif
}
