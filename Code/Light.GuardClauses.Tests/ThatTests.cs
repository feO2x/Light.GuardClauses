using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class ThatTests
    {
        [Fact(DisplayName = "That must throw the specified exception when the condition returns false.")]
        public void ExceptionThrownOnFalse()
        {
            Action act = () => Check.That(false, () => new Exception());

            act.ShouldThrow<Exception>();
        }

        [Fact(DisplayName = "That must not throw the specified exception when condition returns true.")]
        public void ExceptionNotThrownOnTrue()
        {
            Action act = () => Check.That(true, () => new Exception());

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "That must throw an ArgumentNullException when exception is null.")]
        public void ExceptionNull()
        {
            Action act = () => Check.That(true, null);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("exception");
        }
    }
}