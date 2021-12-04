using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertions;

public static class MustMatchTests
{
    [Fact]
    public static void StringDoesNotMatch()
    {
        var pattern = new Regex(@"\d{5}");
        const string @string = "12c45";

        Action act = () => @string.MustMatch(pattern, nameof(@string));

        var assertion = act.Should().Throw<StringDoesNotMatchException>().Which;
        assertion.Message.Should().Contain($"{nameof(@string)} must match the regular expression \"{pattern}\", but it actually is \"{@string}\".");
        assertion.ParamName.Should().BeSameAs(nameof(@string));
    }

    [Fact]
    public static void StringMatches()
    {
        var pattern = new Regex(@"\w{5}");
        // ReSharper disable once StringLiteralTypo
        const string @string = "abcde";

        var result = @string.MustMatch(pattern, nameof(@string));

        result.Should().BeSameAs(@string);
    }

    [Fact]
    public static void StringNull()
    {
        Action act = () => ((string) null).MustMatch(new Regex("Foo"));

        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [MemberData(nameof(CustomExceptionData))]
    public static void CustomException(string input, Regex regularExpression) =>
        Test.CustomException(input,
                             regularExpression,
                             (@string, regex, exceptionFactory) => @string.MustMatch(regex, exceptionFactory));

    public static readonly TheoryData<string, Regex> CustomExceptionData =
        new()
        {
            { "ab", new Regex(@"\w{3}") },
            { null, new Regex(@"\W{6}") },
            { "Foo", null }
        };

    [Fact]
    public static void NoCustomExceptionThrown() => 
        "Foo".MustMatch(new Regex("\\w{3}"), (_, _) => new Exception()).Should().BeSameAs("Foo");


    [Fact]
    public static void CustomMessage() =>
        // ReSharper disable once StringLiteralTypo
        Test.CustomMessage<StringDoesNotMatchException>(message => "abcde".MustMatch(new Regex("Foo"), message:message));

    [Fact]
    public static void CustomMessageParameterNull() => 
        Test.CustomMessage<ArgumentNullException>(message => ((string) null).MustMatch(new Regex("Foo"), message: message));

    [Fact]
    public static void CustomMessageRegexNull() =>
        Test.CustomMessage<ArgumentNullException>(message => "Foo".MustMatch(null!, message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        const string email = "This is not really an email";

        var act = () => email.MustMatch(RegularExpressions.EmailRegex);

        act.Should().Throw<StringDoesNotMatchException>()
           .And.ParamName.Should().Be(nameof(email));
    }
}