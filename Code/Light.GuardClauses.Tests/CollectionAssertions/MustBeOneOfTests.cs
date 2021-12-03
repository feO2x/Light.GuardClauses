using System;
using System.Linq;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
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

            var assertion = act.Should().Throw<ValueIsNotOneOfException>().Which;
            assertion.Message.Should().Contain(new StringBuilder().AppendLine($"{nameof(item)} must be one of the following items")
                                                                  .AppendItemsWithNewLine(items)
                                                                  .AppendLine($"but it actually is {item.ToStringOrNull()}.")
                                                                  .ToString());
            assertion.ParamName.Should().BeSameAs(nameof(item));
        }

        [Theory]
        [InlineData("Foo", new[] { "Foo", "Bar" })]
        [InlineData("Qux", new[] { "Baz", "Qux", "Quux" })]
        [InlineData(null, new[] { "Foo", "Bar", null })]
        public static void OneOf(string item, string[] items) =>
            item.MustBeOneOf(items).Should().BeSameAs(item);

        [Fact]
        public static void ItemsNull()
        {
            Action act = () => "Foo".MustBeOneOf(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(42, new[] { 1, 2, 3 })]
        [InlineData(87, null)]
        public static void CustomException(int item, int[] collection) =>
            Test.CustomException(item,
                                 collection,
                                 (i, items, exceptionFactory) => i.MustBeOneOf(items, exceptionFactory));

        [Fact]
        public static void NoCustomExceptionThrown() => 
            42.MustBeOneOf(new[] { 42, 43 }, (_, _) => new Exception()).Should().Be(42);

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ValueIsNotOneOfException>(message => 42.MustBeOneOf(new[] { 1, 2 }, message: message));

        [Fact]
        public static void CustomMessageCollectionNull() => 
            Test.CustomMessage<ArgumentNullException>(message => long.MaxValue.MustBeOneOf(null!, message: message));

        [Fact]
        public static void CallerArgumentExpression()
        {
            const int myNumber = 50;

            Action act = () => myNumber.MustBeOneOf(Enumerable.Range(1, 10));

            act.Should().Throw<ValueIsNotOneOfException>()
               .And.ParamName.Should().Be(nameof(myNumber));
        }
    }
}