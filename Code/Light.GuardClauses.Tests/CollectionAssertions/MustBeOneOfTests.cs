using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions
{
    public static class MustBeOneOfTests
    {
        [Theory]
        [InlineData(42, new[] { 1, 2, 3 })]
        [InlineData(-1523, new int[] { })]
        [InlineData(100153, new[] { 2 })]
        public static void NotOneOf(int item, int[] items)
        {
            Action act = () => item.MustBeOneOf(items, nameof(item));

            act.Should().Throw<ValueIsNotOneOfException>()
               .And.Message.Should().Contain($"{nameof(item)} {item} must be one of the following items, but it actually is not.");
        }

        [Theory]
        [InlineData(Metasyntactic.Foo, new[] { Metasyntactic.Foo, Metasyntactic.Bar })]
        [InlineData(Metasyntactic.Qux, new[] { Metasyntactic.Baz, Metasyntactic.Qux, Metasyntactic.Quux })]
        [InlineData(null, new[] { Metasyntactic.Foo, Metasyntactic.Bar, null })]
        public static void OneOf(string item, string[] items) =>
            item.MustBeOneOf(items).Should().BeSameAs(item);

        [Fact]
        public static void ItemsNull()
        {
            Action act = () => Metasyntactic.Foo.MustBeOneOf<string, List<string>>(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException(Metasyntactic.Foo,
                                 new List<string> { Metasyntactic.Bar, Metasyntactic.Baz },
                                 (@string, items, exceptionFactory) => @string.MustBeOneOf(items, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ValueIsNotOneOfException>(message => 42.MustBeOneOf(new [] { 1, 2 }, message: message));
    }
}