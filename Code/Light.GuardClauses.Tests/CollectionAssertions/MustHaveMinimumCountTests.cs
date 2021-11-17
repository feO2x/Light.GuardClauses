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

            var assertion = act.Should().Throw<InvalidCollectionCountException>().Which;
            assertion.Message.Should().Contain($"{nameof(collection)} must have at least count {minimumCount}, but it actually has count {collection.Count()}.");
            assertion.ParamName.Should().BeSameAs(nameof(collection));
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
            Action act = () => ((ISet<object>)null).MustHaveMinimumCount(42);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(new[] { 1, 2, 3 }, 4)]
        [InlineData(null, 3)]
        public static void CustomException(int[] collection, int minimumCount) =>
            Test.CustomException(collection,
                                 minimumCount,
                                 (c, minCount, exceptionFactory) => c.MustHaveMinimumCount(minCount, exceptionFactory));

        [Fact]
        public static void NoCustomExceptionThrown()
        {
            var set = new HashSet<string> { Metasyntactic.Foo, Metasyntactic.Bar, Metasyntactic.Baz };
            set.MustHaveMinimumCount(2, (_, _) => new Exception()).Should().BeSameAs(set);
        }

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<InvalidCollectionCountException>(message => new ObservableCollection<bool> { true }.MustHaveMinimumCount(2, message: message));

        [Fact]
        public static void CustomMessageCollectionNull() =>
            Test.CustomMessage<ArgumentNullException>(message => ((ObservableCollection<string>)null).MustHaveMinimumCount(42, message: message));

        [Fact]
        public static void CallerArgumentExpression()
        {
            var myCollection = new List<int> { 1, 2, 3 };

            Action act = () => myCollection.MustHaveMinimumCount(5);

            act.Should().Throw<InvalidCollectionCountException>()
               .And.ParamName.Should().Be(nameof(myCollection));
        }
    }
}