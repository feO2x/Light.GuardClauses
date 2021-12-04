using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions;

public static class MustNotBeSameAsTests
{
    [Theory]
    [InlineData("Foo")]
    [InlineData("Bar")]
    public static void ReferencesEqual(string reference)
    {
        Action act = () => reference.MustNotBeSameAs(reference, nameof(reference));

        act.Should().Throw<SameObjectReferenceException>()
           .And.Message.Should().Contain($"{nameof(reference)} must not point to object \"{reference}\", but it actually does.");
    }

    [Theory]
    [InlineData("Hello", "World")]
    [InlineData("1", "2")]
    public static void ReferencesDifferent(string first, string second)
    {
        var result = first.MustNotBeSameAs(second);

        result.Should().Be(first);
    }

    [Fact]
    public static void CustomException() => 
        Test.CustomException("Baz",
                             (reference, exceptionFactory) => reference.MustNotBeSameAs(reference, exceptionFactory));

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<SameObjectReferenceException>(message => "Qux".MustNotBeSameAs("Qux", message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        var object1 = new object();

        Action act = () => object1.MustNotBeSameAs(object1);

        act.Should().Throw<SameObjectReferenceException>()
           .And.ParamName.Should().Be(nameof(object1));
    }
}