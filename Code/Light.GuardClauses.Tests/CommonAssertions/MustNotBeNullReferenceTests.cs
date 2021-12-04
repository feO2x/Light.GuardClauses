using System;
using System.Collections;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions;

public static class MustNotBeNullReferenceTests
{
    [Fact]
    public static void StringIsNull() => CheckArgumentNullExceptionIsThrown(default(string));

    [Fact]
    public static void DelegateIsNull() => CheckArgumentNullExceptionIsThrown(default(Action));

    [Fact]
    public static void PolymorphicReferenceIsNull() => CheckArgumentNullExceptionIsThrown(default(IEnumerable));

    private static void CheckArgumentNullExceptionIsThrown<T>(T nullReference) where T : class
    {
        Action act = () => nullReference.MustNotBeNullReference(nameof(nullReference));

        act.Should().Throw<ArgumentNullException>()
           .And.Message.Should().Contain($"{nameof(nullReference)} must not be null.");
    }

    [Theory]
    [InlineData(42)]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(ConsoleColor.Cyan)]
    public static void ValueType<T>(T value) where T : struct =>
        value.MustNotBeNullReference().Should().Be(value);

    [Theory]
    [InlineData("Foo")]
    [InlineData("Bar")]
    public static void ReferenceNotNull(string reference) =>
        reference.MustNotBeNullReference().Should().BeSameAs(reference);

    [Fact]
    public static void CustomException() =>
        Test.CustomException(exceptionFactory => ((object) null).MustNotBeNullReference(exceptionFactory));

    [Fact]
    public static void CustomExceptionNotNull() => 
        "Foo".MustNotBeNullReference(() => null).Should().BeSameAs("Foo");

    [Fact]
    public static void CustomExceptionValueType() => 
        ConsoleColor.DarkGreen.MustNotBeNullReference(() => null).Should().Be(ConsoleColor.DarkGreen);

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ArgumentNullException>(message => ((object) null).MustNotBeNullReference(message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        var someParameter = (object) null;

        // ReSharper disable once ExpressionIsAlwaysNull -- I want to check the CallerArgumentExpressionAttribute here
        Action act = () => someParameter.MustNotBeNullReference();

        act.Should().Throw<ArgumentNullException>()
           .And.ParamName.Should().Be(nameof(someParameter));
    }
}