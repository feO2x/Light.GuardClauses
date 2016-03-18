using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotBeEmptyTests
    {
        [Fact(DisplayName = "MustNotBeEmpty must throw an exception when the specified GUID is empty.")]
        public void GuidEmpty()
        {
            var emptyGuid = Guid.Empty;

            Action act = () => emptyGuid.MustNotBeEmpty(nameof(emptyGuid));

            act.ShouldThrow<EmptyGuidException>()
               .And.ParamName.Should().Be(nameof(emptyGuid));
        }

        [Fact(DisplayName = "MustNotBeEmpty must not throw an exception when the specified GUID is not empty.")]
        public void GuidNotEmpty()
        {
            var validGuid = Guid.NewGuid();

            Action act = () => validGuid.MustNotBeEmpty(nameof(validGuid));

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "The caller can specify a custom message that MustNotBeEmpty must inject instead of the default one.")]
        public void CustomMessage()
        {
            const string message = "Thou shall not be an empty GUID!";

            Action act = () => Guid.Empty.MustNotBeEmpty(message: message);

            act.ShouldThrow<EmptyGuidException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustNotBeEmpty must raise instead of the default one.")]
        public void CustomException()
        {
            var exception = new Exception();

            Action act = () => Guid.Empty.MustNotBeEmpty(exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}