using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class MustNotBeSameAsTests
    {
        [Theory]
        [InlineData(Metasyntactic.Foo)]
        [InlineData(Metasyntactic.Bar)]
        public static void ReferencesEqual(string reference)
        {
            Action act = () => reference.MustNotBeSameAs(reference, nameof(reference));

            act.Should().Throw<SameObjectReferenceException>()
               .And.Message.Should().Contain($"{nameof(reference)} must not point to object \"{reference}\", but it actually does.");
        }

        [Theory]
        [InlineData("Hello", "World")]
        [InlineData("1", "2")]
        [InlineData(new object[] { }, new object[] { "Foo" })]
        public static void ReferencesDifferent<T>(T first, T second) where T : class
        {
            var result = first.MustNotBeSameAs(second);

            result.Should().Be(first);
        }

        [Fact]
        public static void CustomException() => 
            Test.CustomException(Metasyntactic.Baz,
                                 (reference, exceptionFactory) => reference.MustNotBeSameAs(reference, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<SameObjectReferenceException>(message => Metasyntactic.Qux.MustNotBeSameAs(Metasyntactic.Qux, message: message));
    }
}