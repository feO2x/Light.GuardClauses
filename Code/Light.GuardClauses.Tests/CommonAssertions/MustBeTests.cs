using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions;

public static class MustBeTests
{
    [Theory]
    [InlineData("Foo", "Bar")]
    [InlineData("Baz", "Qux")]
    public static void ValuesNotEqual(string x, string y)
    {
        Action act = () => x.MustBe(y, nameof(x));

        act.Should().Throw<ValuesNotEqualException>()
           .And.Message.Should().Contain($"{nameof(x)} must be equal to {y.ToStringOrNull()}, but it actually is {x.ToStringOrNull()}.");
    }

    [Theory]
    [InlineData(42L)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    public static void ValuesEqual(long value) => value.MustBe(value).Should().Be(value);

    [Fact]
    public static void CustomException() =>
        Test.CustomException("Foo",
                             "Bar",
                             (x, y, exceptionFactory) => x.MustBe(y, exceptionFactory));

    [Fact]
    public static void CustomExceptionValuesEqual() => 
        87.MustBe(87, (_, _) => new Exception()).MustBe(87);

    [Fact]
    public static void CustomMessage() =>
        Test.CustomMessage<ValuesNotEqualException>(message => false.MustBe(true, message: message));

    [Fact]
    public static void ValuesNotEqualCustomEqualityComparer()
    {
        var myString = "Foo";

        Action act = () => myString.MustBe("Bar", new EqualityComparerStub<string>(false));

        act.Should().Throw<ValuesNotEqualException>()
           .And.Message.Should().Contain($"myString must be equal to {"Bar".ToStringOrNull()}, but it actually is {"Foo".ToStringOrNull()}.");
    }

    [Fact]
    public static void ValuesEqualCustomEqualityComparer() => 42.MustBe(42, new EqualityComparerStub<int>(true)).Should().Be(42);

    [Fact]
    public static void CustomExceptionEqualityComparer() =>
        Test.CustomException("Foo",
                             "Bar",
                             (IEqualityComparer<string>) new EqualityComparerStub<string>(false),
                             (x, y, comparer, exceptionFactory) => x.MustBe(y, comparer, exceptionFactory));

    [Fact]
    public static void CustomExceptionEqualityComparerNull() => 
        Test.CustomException(35L,
                             22L,
                             (IEqualityComparer<long>) null,
                             (x, y, comparer, exceptionFactory) => x.MustBe(y, comparer, exceptionFactory));

    [Fact]
    public static void CustomMessageEqualityComparer() =>
        Test.CustomMessage<ValuesNotEqualException>(message => 99m.MustBe(100m, new EqualityComparerStub<decimal>(false), message: message));

    [Fact]
    public static void CustomMessageEqualityComparerNull() => 
        // ReSharper disable once AssignNullToNotNullAttribute
        Test.CustomMessage<ArgumentNullException>(message => 42.MustBe(42, (IEqualityComparer<int>) null, message: message));

    [Fact]
    public static void CallerArgumentExpression()
    {
        var five = 5;

        Action act = () => five.MustBe(4);

        act.Should().Throw<ValuesNotEqualException>()
           .And.ParamName.Should().Be(nameof(five));
    }

    [Fact]
    public static void CallerArgumentExpressionForEqualityComparerOverload()
    {
        var seven = 7;

        Action act = () => seven.MustBe(1, new EqualityComparerStub<int>(false));

        act.Should().Throw<ValuesNotEqualException>()
           .And.ParamName.Should().Be(nameof(seven));
    }
}