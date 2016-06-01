using FluentAssertions;
using System;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class AgainstTests
    {
        [Fact(DisplayName = "Against must throw the specified exception when condition returns true.")]
        public void ExceptionThrownOnTrue()
        {
            Action act = () => Check.Against(true, () => new Exception());

            act.ShouldThrow<Exception>();
        }

        [Fact(DisplayName = "Against must not throw the specified exception when condition returns false.")]
        public void ExceptionNotThrownOnFalse()
        {
            Action act = () => Check.Against(false, () => new Exception());

            act.ShouldNotThrow();
        }
    }
}
