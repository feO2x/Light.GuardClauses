using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustBeNullTests
    {
        [Fact(DisplayName = "MustBeNull throws an exception when the specified value is not null.")]
        public void ArgumentNotNull()
        {
            var @string = "Hey";

            Action act = () => @string.MustBeNull(nameof(@string));

            act.ShouldThrow<ArgumentNotNullException>()
               .And.ParamName.Should().Be(nameof(@string));
        }

        [Fact(DisplayName = "MustBeNull must not throw an exception when the specified value is null.")]
        public void ArgumentNull()
        {
            object @object = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => @object.MustBeNull(nameof(@object));

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "The caller can specify a custom message that MustBeNull must inject instead of the default one.")]
        public void SpecifyMessage()
        {
            var @object = new object();
            const string message = "Thou shall be null!";

            Action act = () => @object.MustBeNull(message: message);

            act.ShouldThrow<ArgumentNotNullException>()
               .And.Message.Should().Be(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustBeNull must raise instead of the default one.")]
        public void SpecifyException()
        {
            var @object = new object();
            var exception = new Exception();

            Action act = () => @object.MustBeNull(exception: exception);

            act.ShouldThrow<Exception>().Which.Should().Be(exception);
        }
    }
}