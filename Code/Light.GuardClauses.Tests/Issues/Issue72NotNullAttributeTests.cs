using System;
using Xunit;

#nullable enable

namespace Light.GuardClauses.Tests.Issues;

public static class Issue72NotNullAttributeTests
{
    [Fact]
    public static void CheckMustNotBeNull()
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

    [Fact]
    public static void CheckMustNotBeDefault()
    {
        _ = TestMustNotBeDefault("foo");
        _ = TestMustNotBeDefaultWithDelegate("foo");
        return;
        
        static int TestMustNotBeDefault(string? input)
        {
            input.MustNotBeDefault();
            return input.Length;
        }
        
        static int TestMustNotBeDefaultWithDelegate(string? input)
        {
            input.MustNotBeDefault(() => new Exception());
            return input.Length;
        }
    }
}