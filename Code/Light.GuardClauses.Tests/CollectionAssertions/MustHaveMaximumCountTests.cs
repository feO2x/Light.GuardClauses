using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions
{
    public static class MustHaveMaximumCountTests
    {
        [Theory]
        [InlineData(new[] { 1, 2, 3, 4 }, 3)]
        [InlineData(new[] { 1, 2 }, 1)]
        [InlineData(new[] { 500 }, 0)]
        public static void MoreItems(int[] items, int count)
        {
            Action act = () => items.MustHaveMaximumCount(count, nameof(items));

            var assertion = act.Should().Throw<InvalidCollectionCountException>().Which;
            assertion.Message.Should().Contain($"{nameof(items)} must have at most count {count}, but it actually has count {items.Length}.");
            assertion.ParamName.Should().BeSameAs(nameof(items));
        }

        [Theory]
        [InlineData(new[] { "Foo" }, 1)]
        [InlineData(new[] { "Bar" }, 2)]
        [InlineData(new[] { "Baz", "Qux", "Quux" }, 5)]
        public static void LessOrEqualItems(string[] items, int count) =>
            items.MustHaveMaximumCount(count).Should().BeSameAs(items);

        [Fact]
        public static void CollectionNull()
        {
            Action act = () => ((ObservableCollection<object>) null).MustHaveMaximumCount(42);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(new[] { 87, 89, 99 }, 1)]
        [InlineData(null, 5)]
        [InlineData(new[] { 1, 2, 3 }, -30)]
        public static void CustomException(int[] collection, int maximumCount) =>
            Test.CustomException(collection,
                                 maximumCount,
                                 (c, count, exceptionFactory) => c.MustHaveMaximumCount(count, exceptionFactory));

        [Fact]
        public static void NoCustomExceptionThrown()
        {
            var collection = new HashSet<string> { "Foo", "Bar" };
            collection.MustHaveMaximumCount(2, (_, _) => new Exception()).Should().BeSameAs(collection);
        }

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<InvalidCollectionCountException>(message => new List<short> { 1, 2, 3 }.MustHaveMaximumCount(2, message: message));

        [Fact]
        public static void CustomMessageCollectionNull() =>
            Test.CustomMessage<ArgumentNullException>(message => ((ObservableCollection<int>) null).MustHaveMaximumCount(3, message: message));
    }
}