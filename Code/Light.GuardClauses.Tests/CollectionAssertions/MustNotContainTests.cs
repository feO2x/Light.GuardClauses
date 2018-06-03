using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions
{
    public static class MustNotContainTests
    {
        [Theory]
        [InlineData(new[] { Metasyntactic.Foo, Metasyntactic.Bar }, Metasyntactic.Foo)]
        [InlineData(new[] { Metasyntactic.Baz, Metasyntactic.Qux, Metasyntactic.Quux }, Metasyntactic.Qux)]
        [InlineData(new[] { Metasyntactic.Corge, Metasyntactic.Grault, null }, null)]
        public static void ItemExists(string[] collection, string item)
        {
            Action act = () => collection.MustNotContain(item, nameof(collection));

            act.Should().Throw<ExistingItemException>()
               .And.Message.Should().Contain($"{nameof(collection)} must not contain {item.ToStringOrNull()}, but it actually does.");
        }

        [Theory]
        [InlineData(new[] { 100, 101, 102 }, 42)]
        [InlineData(new[] { 11 }, -5000)]
        [InlineData(new int[] { }, 13)]
        public static void ItemExistsNot(int[] collection, int item) =>
            collection.MustNotContain(item).Should().BeSameAs(collection);

        [Fact]
        public static void CollectionNull()
        {
            Action act = () => ((ObservableCollection<object>) null).MustNotContain(new object());

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException(new List<string> { Metasyntactic.Foo },
                                 Metasyntactic.Foo,
                                 (collection, value, exceptionFactory) => collection.MustNotContain(value, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<ExistingItemException>(message => new HashSet<int> { 42 }.MustNotContain(42, message: message));
    }
}