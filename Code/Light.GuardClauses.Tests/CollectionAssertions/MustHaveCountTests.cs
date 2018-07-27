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
            var collection = new List<string> { Metasyntactic.Foo, Metasyntactic.Bar, Metasyntactic.Baz };

            Action act = () => collection.MustHaveCount(10, nameof(collection));

            act.Should().Throw<InvalidCollectionCountException>()
               .And.Message.Should().Contain($"{nameof(collection)} must have count 10, but it actually has count {collection.Count}.");
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

        [Fact]
        public static void CustomException() =>
            Test.CustomException(new[] { true },
                                 42,
                                 (collection, count, exceptionFactory) => collection.MustHaveCount(count, exceptionFactory));

        [Fact]
        public static void CustomExceptionCollectionNull() =>
            Test.CustomException<IReadOnlyList<string>, int>(null,
                                                             42,
                                                             (collection, count, exceptionFactory) => collection.MustHaveCount(count, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<InvalidCollectionCountException>(message => new[] { 1 }.MustHaveCount(3, message: message));
    }
}