using FluentAssertions;
using System;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotBeGreaterThanTests
    {
        [Theory(DisplayName = "MustNotBeGreaterThan must throw an exception when the specified value is above the given boundary.")]
        [InlineData(1, 0)]
        [InlineData(1, -1)]
        [InlineData(-88, -100)]
        [InlineData("c", "b")]
        [InlineData("Z", "Y")]
        [InlineData("A", "a")]
        public void ParamterAboveBoundary<T>(T value, T boundary) where T : IComparable<T>
        {
            Action act = () => value.MustNotBeGreaterThan(boundary, nameof(value));

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain($"{nameof(value)} must not be greater than {boundary}, but you specified {value}.");
        }

        [Theory(DisplayName = "MustNotBeGreaterThan must not throw an exception when the specified value is equal or lower than the given boundary.")]
        [InlineData(0, 0)]
        [InlineData(-1, 0)]
        [InlineData(-88, 0)]
        [InlineData("a", "b")]
        [InlineData("b", "b")]
        [InlineData("X", "Y")]
        [InlineData("y", "Y")]
        public void ParameterAtOrBelowBoundary<T>(T value, T boundary) where T : IComparable<T>
        {
            Action act = () => value.MustNotBeGreaterThan(boundary, nameof(value));

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "The caller can specify a custom message that MustNotBeGreaterThan must inject instead of the default one.")]
        public void CustomMessage()
        {
            const string message = "Thou shall not be greater than the other!";

            Action act = () => 42.MustNotBeGreaterThan(41, message: message);

            act.ShouldThrow<ArgumentOutOfRangeException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustNotBeGreaterThan must raise instead of the default one.")]
        public void CustomException()
        {
            var exception = new Exception();

            Action act = () => 12.MustNotBeGreaterThan(0, exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}
