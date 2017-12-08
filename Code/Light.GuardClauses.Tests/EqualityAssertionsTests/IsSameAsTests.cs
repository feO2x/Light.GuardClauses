using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.EqualityAssertionsTests
{
    public sealed class IsSameAsTests
    {
        [Fact(DisplayName = "IsSameAs must return true if the two references point to the same object.")]
        public void SameReference()
        {
            var @object = new object();

            var result = @object.IsSameAs(@object);

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "IsSameAs must return false if the two references point to different objects.")]
        public void DifferentReferences()
        {
            var result = new object().IsSameAs(new object());

            result.Should().BeFalse();
        }
    }
}