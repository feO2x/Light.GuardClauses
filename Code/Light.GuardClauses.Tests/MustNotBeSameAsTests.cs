using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotBeSameAsTests
    {
        [Theory(DisplayName = "MustNotBeSameAs must throw an ArgumentException when the specified references point to the same instance.")]
        [InlineData("Foo")]
        [InlineData("Bar")]
        public void ReferencesEqual(string reference)
        {
            Action act = () => reference.MustNotBeSameAs(reference, nameof(reference));

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(reference)} must not point to the object instance \"{reference}\", but it does.");
        }

        [Theory(DisplayName = "MustNotBeSameAs must not throw an exception when the specified references point to different instances.")]
        [InlineData("Hello", "World")]
        [InlineData("1", "2")]
        [InlineData(new object[] { }, new object[] { "Foo" })]
        public void ReferencesDifferent<T>(T first, T second) where T : class
        {
            Action act = () => first.MustNotBeSameAs(second);

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "The caller can specify a custom message that MustNotBeSameAs must inject instead of the default one.")]
        public void CustomMessage()
        {
            const string message = "Thou shall not be one and the same!";

            Action act = () => "foo".MustNotBeSameAs("foo", message: message);

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustNotBeSameAs must raise instead of the default one.")]
        public void CustomException()
        {
            var exception = new Exception();

            Action act = () => "foo".MustNotBeSameAs("foo", exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}