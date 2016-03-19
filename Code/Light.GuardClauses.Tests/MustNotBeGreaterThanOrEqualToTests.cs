using FluentAssertions;
using System;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotBeGreaterThanOrEqualToTests
    {
        [Theory(DisplayName = "MustNotBeGreaterThanOrEqualTo must throw an exception when the specified value is greater than or equal to the given boundary.")]
        [InlineData(2, 1)]
        [InlineData(1, 1)]
        [InlineData(-87, -88)]
        [InlineData("a", "a")]
        [InlineData("B", "A")]
        public void ParameterAtOrAboveBoundary<T>(T value, T boundary) where T : IComparable<T>
        {
            Action act = () => value.MustNotBeGreaterThanOrEqualTo(boundary, nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be greater than or equal to {boundary}, but you specified {value}.");
        }

        [Theory(DisplayName = "MustNotBeGreaterThanOrEqualTo must not throw an exception when the specified value is less than the given boundary.")]
        [InlineData(0, 1)]
        [InlineData(-80, -70)]
        [InlineData("A", "B")]
        [InlineData("a", "A")]
        public void ParameterBelowBoundary<T>(T value, T boundary) where T : IComparable<T>
        {
            Action act = () => value.MustNotBeGreaterThanOrEqualTo(boundary, nameof(value));

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "The caller can specify a custom message that MustNotBeGreaterThanOrEqualTo must inject instead of the default one.")]
        public void CustomMessage()
        {
            const string message = "Thou shall not be greater than or equal to the other!";

            Action act = () => 42.MustNotBeGreaterThanOrEqualTo(42, message: message);

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustNotBeGreaterThanOrEqualTo must raise instead of the default one.")]
        public void CustomException()
        {
            var exception = new Exception();

            Action act = () => 42.MustNotBeGreaterThanOrEqualTo(41, exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}

