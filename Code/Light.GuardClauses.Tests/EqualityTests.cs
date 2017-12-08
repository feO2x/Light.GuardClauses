using System;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class EqualityTests
    {
        [Theory(DisplayName = "The CreateHashCode methods must not throw an exception when one of the passed in parameters is null.")]
        [MemberData(nameof(NoExceptionOnNullData))]
        public void NoExceptionOnNull(Action createHashCode)
        {
            createHashCode.ShouldNotThrow();
        }

        public static readonly TestData NoExceptionOnNullData =
            new[]
            {
                new object[] { new Action(() => Equality.CreateHashCode(42, (string) null)) },
                new object[] { new Action(() => Equality.CreateHashCode((string) null, 42)) },
                new object[] { new Action(() => Equality.CreateHashCode((string) null, 42, 56)) },
                new object[] { new Action(() => Equality.CreateHashCode(42, (string) null, 56)) },
                new object[] { new Action(() => Equality.CreateHashCode(42, 56, (string) null)) },
                new object[] { new Action(() => Equality.CreateHashCode((string) null, 42, 56, 87)) },
                new object[] { new Action(() => Equality.CreateHashCode(42, (string) null, 56, 87)) },
                new object[] { new Action(() => Equality.CreateHashCode(42, 56, (string) null, 87)) },
                new object[] { new Action(() => Equality.CreateHashCode(42, 56, 87, (string) null)) },
                new object[] { new Action(() => Equality.CreateHashCode(new object(), 42, 44, "Foo", null)) }
            };
    }
}