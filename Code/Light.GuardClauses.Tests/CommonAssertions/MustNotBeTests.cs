using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions;

public static class MustNotBeTests
{
    [Theory]
    [InlineData("Qux")]
    [InlineData("Foo")]
    public static void ValuesEqual(string value)
    {
        Action act = () => value.MustNotBe(value, nameof(value));

        act.Should().Throw<ValuesEqualException>()
           .And.Message.Should().Contain($"{nameof(value)} must not be equal to {value.ToStringOrNull()}, but it actually is {value.ToStringOrNull()}.");
    }

    [Theory]
    [InlineData(42, 43)]
    [InlineData(34, -153)]
    public static void ValuesNotEqual(int x, int y) => x.MustNotBe(y).Should().Be(x);


    [Fact]
    public static void CustomException() =>
        Test.CustomException("Foo",
                             "Foo",
                             (x, y, exceptionFactory) => x.MustNotBe(y, exceptionFactory));

    [Fact]
    public static void CustomExceptionNotEqual() => 
        42.MustNotBe(43, (_, _) => null).Should().Be(42);

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ValuesEqualException>(message => false.MustNotBe(false, message: message));

    [Fact]
    public static void ValuesNotEqualCustomEqualityComparer()
    {
        var myString = "Foo";

        Action act = () => myString.MustNotBe("Foo", new EqualityComparerStub<string>(true));

        act.Should().Throw<ValuesEqualException>()
           .And.Message.Should().Contain($"myString must not be equal to {"Foo".ToStringOrNull()}, but it actually is {"Foo".ToStringOrNull()}.");
    }

    [Fact]
    public static void ValuesEqualCustomEqualityComparer() => 42.MustNotBe(47, new EqualityComparerStub<int>(false)).Should().Be(42);

    [Fact]
    public static void CustomExceptionEqualityComparer() =>
        Test.CustomException("Foo",
                             "Bar",
                             (IEqualityComparer<string>) new EqualityComparerStub<string>(true),
                             (x, y, comparer, exceptionFactory) => x.MustNotBe(y, comparer, exceptionFactory));

    [Fact]
    public static void CustomExceptionCustomComparerNotEqual() => 
        "Foo".MustNotBe("Bar", new EqualityComparerStub<string>(false), (_, _, _) => null).Should().BeSameAs("Foo");

    [Fact]
    public static void CustomMessageEqualityComparer() =>
        Test.CustomMessage<ValuesEqualException>(message => 50m.MustNotBe(50m, new EqualityComparerStub<decimal>(true), message: message));

    [Fact]
    public static void CustomMessageComparerNull() => 
        // ReSharper disable once AssignNullToNotNullAttribute
        Test.CustomMessage<ArgumentNullException>(message => 42.MustNotBe(89, (IEqualityComparer<int>) null, message: message));
        
    [Fact]
    public static void CallerArgumentExpression()
    {
        var eight = 8;

        Action act = () => eight.MustNotBe(8);

        act.Should().Throw<ValuesEqualException>()
           .And.ParamName.Should().Be(nameof(eight));
    }

    [Fact]
    public static void CallerArgumentExpressionForEqualityComparerOverload()
    {
        var foo = "Foo";

        Action act = () => foo.MustNotBe("Foo", new EqualityComparerStub<string>(true));

        act.Should().Throw<ValuesEqualException>()
           .And.ParamName.Should().Be(nameof(foo));
    }
}