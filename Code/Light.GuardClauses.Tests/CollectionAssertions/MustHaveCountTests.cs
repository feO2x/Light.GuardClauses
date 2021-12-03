using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions
{
    public static class MustHaveCountTests
    {
        [Fact]
        public static void CountDifferent()
        {
            var collection = new List<string> { "Foo", "Bar", "Baz" };

            Action act = () => collection.MustHaveCount(10, nameof(collection));

            var assertion = act.Should().Throw<InvalidCollectionCountException>().Which;
            assertion.Message.Should().Contain($"{nameof(collection)} must have count 10, but it actually has count {collection.Count}.");
            assertion.ParamName.Should().BeSameAs(nameof(collection));
        }

        [Fact]
        public static void CountSame()
        {
            var collection = new[] { 1, 2, 3, 4 };

            collection.MustHaveCount(4).Should().BeSameAs(collection);
        }

        [Fact]
        public static void CollectionNull()
        {
            Action act = () => ((int[]) null).MustHaveCount(1);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(new[] { true }, 42)]
        [InlineData(null, 3)]
        [InlineData(new[] { false }, -1)]
        public static void CustomException(bool[] collection, int count) =>
            Test.CustomException(collection,
                                 count,
                                 (a, c, exceptionFactory) => a.MustHaveCount(c, exceptionFactory));

        [Fact]
        public static void CustomExceptionNotThrown()
        {
            var collection = new List<short>{1, -2, 3};
            collection.MustHaveCount(3, (_, _) => new Exception()).Should().BeSameAs(collection);
        }

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<InvalidCollectionCountException>(message => new[] { 1 }.MustHaveCount(3, message: message));

        [Fact]
        public static void CustomMessageCollectionNull() => 
            Test.CustomMessage<ArgumentNullException>(message => ((List<string>) null).MustHaveCount(42, message: message));

        [Fact]
        public static void CallerArgumentExpression()
        {
            var collection = new List<int> { 1, 2, 3 };

            Action act = () => collection.MustHaveCount(5);

            act.Should().Throw<InvalidCollectionCountException>()
               .And.ParamName.Should().Be(nameof(collection));
        }
    }
}