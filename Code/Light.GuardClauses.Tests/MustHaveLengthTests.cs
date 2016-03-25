using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustHaveLengthTests
    {
        [Theory(DisplayName = "MustHaveLength must throw a StringException when the specified length is different from the string's length.")]
        [InlineData("Hello", 4)]
        [InlineData("", 1)]
        [InlineData("Turns out, far too much has been written about great men and not nearly enough about morons. Doesn't seem right.", 42)]
        public void LenthDifferent(string @string, int length)
        {
            Action act = () => @string.MustHaveLength(length, nameof(@string));

            act.ShouldThrow<SystemException>()
               .And.Message.Should().Contain($"{nameof(@string)} must have a length of {length}, but it actually has a length of {@string.Length}.");
        }
    }
}