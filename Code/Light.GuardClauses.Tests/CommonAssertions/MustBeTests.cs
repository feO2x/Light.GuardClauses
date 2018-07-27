using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class MustBeTests
    {
        [Theory]
        [InlineData(Metasyntactic.Foo, Metasyntactic.Bar)]
        [InlineData(Metasyntactic.Baz, Metasyntactic.Qux)]
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
            Test.CustomException(Metasyntactic.Foo,
                                 Metasyntactic.Bar,
                                 (x, y, exceptionFactory) => x.MustBe(y, exceptionFactory));

        [Fact]
        public static void CustomExceptionValuesEqual() => 
            87.MustBe(87, (x, y) => new Exception()).MustBe(87);

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ValuesNotEqualException>(message => false.MustBe(true, message: message));

        [Fact]
        public static void ValuesNotEqualCustomEqualityComparer()
        {
            Action act = () => Metasyntactic.Foo.MustBe(Metasyntactic.Bar, new EqualityComparerStub<string>(false));

            act.Should().Throw<ValuesNotEqualException>()
               .And.Message.Should().Contain($"The value must be equal to {Metasyntactic.Bar.ToStringOrNull()}, but it actually is {Metasyntactic.Foo.ToStringOrNull()}.");
        }

        [Fact]
        public static void ValuesEqualCustomEqualityComparer() => 42.MustBe(42, new EqualityComparerStub<int>(true)).Should().Be(42);

        [Fact]
        public static void CustomExceptionEqualityComparer() =>
            Test.CustomException(Metasyntactic.Foo,
                                 Metasyntactic.Bar,
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
            Test.CustomMessage<ArgumentNullException>(message => 42.MustBe(42, (IEqualityComparer<int>) null, message: message));
    }
}