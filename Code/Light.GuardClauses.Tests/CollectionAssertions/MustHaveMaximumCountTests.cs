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

            act.Should().Throw<InvalidCollectionCountException>()
               .And.Message.Should().Contain($"{nameof(items)} must have at most count {count}, but it actually has count {items.Length}.");
        }

        [Theory]
        [InlineData(new []{Metasyntactic.Foo}, 1)]
        [InlineData(new []{Metasyntactic.Bar}, 2)]
        [InlineData(new []{Metasyntactic.Baz, Metasyntactic.Qux, Metasyntactic.Quux}, 5)]
        public static void LessOrEqualItems(string[] items, int count) => 
            items.MustHaveMaximumCount(count).Should().BeSameAs(items);

        [Fact]
        public static void CollectionNull()
        {
            Action act = () => ((ObservableCollection<object>) null).MustHaveMaximumCount(42);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() => 
            Test.CustomException(new List<int> { 87, 89, 99 },
                                 1,
                                 (collection, count, exceptionFactory) => collection.MustHaveMaximumCount(count, exceptionFactory));

        [Fact]
        public static void CustomMessage() => 
            Test.CustomMessage<InvalidCollectionCountException>(message => new List<short>{1, 2, 3}.MustHaveMaximumCount(2, message: message));
    }
}