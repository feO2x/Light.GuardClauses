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

            var assertion = act.Should().Throw<ValueIsOneOfException>().Which;
            assertion.Message.Should().Contain(new StringBuilder().AppendLine($"{nameof(value)} must not be one of the following items")
                                                                  .AppendItemsWithNewLine(items)
                                                                  .AppendLine($"but it actually is {value.ToStringOrNull()}.")
                                                                  .ToString());
            assertion.ParamName.Should().BeSameAs(nameof(value));
        }

        [Theory]
        [InlineData(42, new[] { 1, 2, 3 })]
        [InlineData(89, new int[] { })]
        public static void ValueExistsNot(int value, int[] items) =>
            value.MustNotBeOneOf(items).Should().Be(value);

        [Fact]
        public static void ItemsNull()
        {
            Action act = () => Metasyntactic.Foo.MustNotBeOneOf(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(42, new[] { 1, 42, 3 })]
        [InlineData(42, null)]
        public static void CustomException(int item, int[] collection) =>
            Test.CustomException(item,
                                 collection == null ? null : new HashSet<int>(collection),
                                 (value, set, exceptionFactory) => value.MustNotBeOneOf(set, exceptionFactory));

        [Fact]
        public static void NoCustomExceptionThrown() =>
            42.MustNotBeOneOf(new[] { 1, 2, 3 }, (_, _) => new Exception()).Should().Be(42);

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ValueIsOneOfException>(message => false.MustNotBeOneOf(new[] { true, false }, message: message));

        [Fact]
        public static void CustomMessageCollectionNull() =>
            Test.CustomMessage<ArgumentNullException>(message => true.MustNotBeOneOf(null!, message: message));

        [Fact]
        public static void CallerArgumentExpression()
        {
            var fortyTwo = 42;

            Action act = () => fortyTwo.MustNotBeOneOf(new[] { 42 });

            act.Should().Throw<ValueIsOneOfException>()
               .And.ParamName.Should().Be(nameof(fortyTwo));
        }
    }
}