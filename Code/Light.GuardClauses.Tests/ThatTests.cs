using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
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
    }
}