using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
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

        [Fact(DisplayName = "Against must throw an ArgumentNullException when exception is null.")]
        public void ExceptionNull()
        {
            Action act = () => Check.Against(false, null);

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("exception");
        }
    }
}