using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions
{
    public static class MustNotBeOneOfTests
    {
        [Theory]
        [InlineData(Metasyntactic.Foo, new[] { Metasyntactic.Foo, Metasyntactic.Bar })]
        [InlineData(null, new[] { Metasyntactic.Foo, Metasyntactic.Bar, null, Metasyntactic.Qux })]
        public static void ValueExists(string value, string[] items)
        {
            Action act = () => value.MustNotBeOneOf(items, nameof(value));

            act.Should().Throw<ValueIsOneOfException>()
               .And.Message.Should().Contain(new StringBuilder().AppendLine($"{nameof(value)} must not be one of the following items")
                                                                .AppendItemsWithNewLine(items)
                                                                .AppendLine($"but it actually is {value.ToStringOrNull()}.")
                                                                .ToString());
        }

        [Theory]
        [InlineData(42, new[] { 1, 2, 3 })]
        [InlineData(89, new int[] { })]
        public static void ValueExistsNot(int value, int[] items) =>
            value.MustNotBeOneOf(items).Should().Be(value);

        [Fact]
        public static void ItemsNull()
        {
            Action act = () => Metasyntactic.Foo.MustNotBeOneOf((List<string>) null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException(42,
                                 new HashSet<int> { 42, 35 },
                                 (value, set, exceptionFactory) => value.MustNotBeOneOf(set, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ValueIsOneOfException>(message => false.MustNotBeOneOf(new[] { true, false }, message: message));
    }
}