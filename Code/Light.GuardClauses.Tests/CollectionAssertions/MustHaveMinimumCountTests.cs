using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions
{
    public static class MustHaveMinimumCountTests
    {
        [Theory]
        [InlineData(new[] { Metasyntactic.Foo, Metasyntactic.Bar }, 4)]
        [InlineData(new[] { 1, 2, 3, 4 }, 40)]
        [InlineData(new bool[] { }, 1)]
        public static void LessItems(IEnumerable collection, int minimumCount)
        {
            Action act = () => collection.MustHaveMinimumCount(minimumCount, nameof(collection));

            act.Should().Throw<InvalidCollectionCountException>()
               .And.Message.Should().Contain($"{nameof(collection)} must have at least count {minimumCount}, but it actually has count {collection.Count()}.");
        }

        [Theory]
        [InlineData(new[] { Metasyntactic.Foo, Metasyntactic.Bar, Metasyntactic.Baz }, 3)]
        [InlineData(new[] { Metasyntactic.Qux, Metasyntactic.Quux, Metasyntactic.Corge }, 2)]
        [InlineData(new string[] { }, 0)]
        public static void EqualOrMoreItems(string[] collection, int minimumCount) =>
            collection.MustHaveMinimumCount(minimumCount).Should().BeSameAs(collection);

        [Fact]
        public static void CollectionNull()
        {
            Action act = () => ((ISet<object>) null).MustHaveMinimumCount(42);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException(new List<short>(),
                                 12,
                                 (collection, minimumCount, exceptionFactory) => collection.MustHaveMinimumCount(minimumCount, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<InvalidCollectionCountException>(message => new ObservableCollection<bool> { true }.MustHaveMinimumCount(2, message: message));
    }
}