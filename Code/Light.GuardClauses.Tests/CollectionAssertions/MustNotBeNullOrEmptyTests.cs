using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CollectionAssertions
{
    public static class MustNotBeNullOrEmptyTests
    {
        [Fact]
        public static void CollectionNull()
        {
            Action act = () => ((object[]) null).MustNotBeNullOrEmpty(Metasyntactic.Foo);

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be(Metasyntactic.Foo);
        }

        [Fact]
        public static void CollectionEmpty()
        {
            var empty = new List<double>();

            Action act = () => empty.MustNotBeNullOrEmpty(nameof(Metasyntactic.Bar));

            act.Should().Throw<EmptyCollectionException>()
               .And.Message.Should().Contain($"{Metasyntactic.Bar} must not be an empty collection, but it actually is.");
        }

        [Fact]
        public static void CollectionNotEmpty()
        {
            var collection = new HashSet<string> { Metasyntactic.Baz, Metasyntactic.Qux, Metasyntactic.Corge };

            collection.MustNotBeNullOrEmpty().Should().BeSameAs(collection);
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException(exceptionFactory => new List<object>().MustNotBeNullOrEmpty(exceptionFactory));

        [Fact]
        public static void CustomMessage() => 
            Test.CustomMessage<EmptyCollectionException>(message => new HashSet<string>().MustNotBeNullOrEmpty(message: message));
    }
}