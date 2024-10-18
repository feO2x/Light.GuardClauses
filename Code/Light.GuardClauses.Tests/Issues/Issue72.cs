using System;
using Xunit;

#nullable enable

namespace Light.GuardClauses.Tests.Issues;

public static class Issue72
{
    [Fact]
    public static void MustNotBeNullDoesNotPropagate()
    {
        _ = TestMustNotBeNull("foo");
        _ = TestMustNotBeNullWithDelegate("foo");
        return;

        static int TestMustNotBeNull(string? input)
        {
            Check.MustNotBeNull(input);
            return input.Length;
        }

        static int TestMustNotBeNullWithDelegate(string? input)
        {
            input.MustNotBeNull(() => new Exception());
            return input.Length;
        }
    }
}