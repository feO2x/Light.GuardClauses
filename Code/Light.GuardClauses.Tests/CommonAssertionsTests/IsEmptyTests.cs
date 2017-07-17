using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsEmptyTests
    {
        [Fact(DisplayName = "IsEmpty must return true if the specified value is an empty GUID.")]
        public void GuidEmpty()
        {
            var result = Guid.Empty.IsEmpty();

            result.Should().BeTrue();
        }

        [Fact(DisplayName = "IsEmpty must return false when the specified value is not an empty GUID.")]
        public void GuidNotEmpty()
        {
            var result = Guid.NewGuid().IsEmpty();

            result.Should().BeFalse();
        }
    }
}