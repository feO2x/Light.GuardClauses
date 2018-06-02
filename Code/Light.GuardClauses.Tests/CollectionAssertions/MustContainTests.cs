using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions
{
    public static class MustContainTests
    {
        [Theory]
        [InlineData(new []{1, 2, 3}, 5)]
        [InlineData(new []{-5491, 6199}, 42)]
        public static void ItemNotPartOf(int[] collection, int item)
        {
            Action act = () => collection.MustContain(item, nameof(collection));

            act.Should().Throw<MissingItemException>()
               .And.Message.Should().Contain($"{nameof(collection)} must contain {item}, but it actually does not.");
        }

        [Theory]
        [InlineData(new[] { Metasyntactic.Foo, Metasyntactic.Bar }, Metasyntactic.Foo)]
        [InlineData(new[] { Metasyntactic.Foo, Metasyntactic.Bar, Metasyntactic.Foo }, Metasyntactic.Foo)]
        [InlineData(new[] { Metasyntactic.Qux }, Metasyntactic.Qux)]
        [InlineData(new[] { Metasyntactic.Qux, null }, null)]
        public static void ItemPartOf(string[] collection, string item) =>
            collection.MustContain(item).Should().BeSameAs(collection);

        [Fact]
        public static void CollectionNull()
        {
            Action act = () => ((string[]) null).MustContain("Foo");

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() => 
            Test.CustomException(new List<long>{long.MaxValue, long.MinValue},
                                 42L,
                                 (collection, item, exceptionFactory) => collection.MustContain(item, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<MissingItemException>(message => new List<string>().MustContain(Metasyntactic.Foo, message: message));
        
    }
}