using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions;

public static class MustNotBeNullTests
{
    [Fact]
    public static void ReferenceIsNull()
    {
        const string parameterName = "Foo";
        Action act = () => ((object) null).MustNotBeNull(parameterName);

        var exceptionAssertion = act.Should().Throw<ArgumentNullException>().And;
        exceptionAssertion.Message.Should().Contain($"{parameterName} must not be null.");
        exceptionAssertion.ParamName.Should().BeSameAs(parameterName);
    }

    [Fact]
    public static void ReferenceIsNotNull() => string.Empty.MustNotBeNull().Should().BeSameAs(string.Empty);

    [Fact]
    public static void CustomException() =>
        Test.CustomException(exceptionFactory => ((string) null).MustNotBeNull(exceptionFactory));

    [Fact]
    public static void CustomExceptionParameterNotNull() => 
        "Foo".MustNotBeNull(() => null).Should().BeSameAs("Foo");

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentNullException>(message => ((object) null).MustNotBeNull(message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        var someParameter = (string) null;

        // ReSharper disable once ExpressionIsAlwaysNull -- I want someParameter to be the implicit parameterName
        Action act = () => someParameter.MustNotBeNull();

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be(nameof(someParameter));
    }
}