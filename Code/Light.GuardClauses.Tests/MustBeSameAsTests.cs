using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustBeSameAsTests
    {
        [Theory(DisplayName = "MustBeSameAs must throw an ArgumentException when the two specified references do not point to the same instance.")]
        [InlineData("Hello", "World")]
        [InlineData("1", "2")]
        [InlineData(new object[] { }, new object[] { "Foo" })]
        public void ReferencesDifferent<T>(T first, T second) where T : class
        {
            Action act = () => first.MustBeSameAs(second, nameof(first));

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(first)} must point to the object instance \"{second}\", but it does not.");
        }

        [Theory(DisplayName = "MustBeSameAs must not throw an exception when the two specified references point to the same instance.")]
        [InlineData("Foo")]
        [InlineData("Bar")]
        public void ReferencesEqual(string reference)
        {
            Action act = () => reference.MustBeSameAs(reference);

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "The caller can specify a custom message that MustBeSameAs must inject instead of the default one.")]
        public void CustomMessage()
        {
            const string message = "Thou shall be one and the same!";

            Action act = () => "foo".MustBeSameAs("bar", message: message);

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustBeSameAs must raise instead of the default one.")]
        public void CustomException()
        {
            var exception = new Exception();

            Action act = () => "foo".MustBeSameAs("bar", exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}