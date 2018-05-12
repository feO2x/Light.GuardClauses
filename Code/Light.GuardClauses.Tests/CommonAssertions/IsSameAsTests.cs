using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static class IsSameAsTests
    {
        [Fact]
        public static void SameReference()
        {
            var @object = new object();

            @object.IsSameAs(@object).Should().BeTrue();
        }

        [Fact]
        public static void DifferentReferences() => new object().IsSameAs(new object()).Should().BeFalse();
    }
}