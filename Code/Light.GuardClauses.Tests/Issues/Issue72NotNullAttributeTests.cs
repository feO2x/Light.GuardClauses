#nullable enable

using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.Issues;

public static class Issue72NotNullAttributeTests
{
    [Fact]
    public static void CheckMustNotBeNull()
    {
        TestMustNotBeNull("foo").Should().Be(3);
        _ = TestMustNotBeNullWithDelegate("foo").Should().Be(3);
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
        TestMustNotBeDefault("foo").Should().Be(3);
        TestMustNotBeDefaultWithDelegate("foo").Should().Be(3);
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

    [Fact]
    public static void CheckMustNotBeNullReference()
    {
        TestMustNotBeNullReference("foo").Should().Be(3);
        TestMustNotBeNullReferenceWithDelegate("foo").Should().Be(3);
        return;

        static int TestMustNotBeNullReference(string? input)
        {
            input.MustNotBeNullReference();
            return input.Length;
        }

        static int TestMustNotBeNullReferenceWithDelegate(string? input)
        {
            input.MustNotBeNullReference(() => new Exception());
            return input.Length;
        }
    }

    [Fact]
    public static void CheckMustBeOfType()
    {
        TestMustBeOfType("foo").Should().Be(3);
        TestMustBeOfTypeWithDelegate("foo").Should().Be(3);
        return;

        static int TestMustBeOfType(object? input)
        {
            input.MustBeOfType<string>();
            return ((string) input).Length;
        }

        static int TestMustBeOfTypeWithDelegate(object? input)
        {
            input.MustBeOfType<string>(_ => new Exception());
            return ((string) input).Length;
        }
    }

    [Fact]
    public static void CheckMustHaveValue()
    {
        TestMustHaveValue(42).Should().Be(42);
        TestMustHaveValueWithDelegate(42).Should().Be(42);
        return;

        static int TestMustHaveValue(int? input)
        {
            input.MustHaveValue();
            return input.Value;
        }

        static int TestMustHaveValueWithDelegate(int? input)
        {
            input.MustHaveValue(() => new Exception());
            return input.Value;
        }
    }

    [Fact]
    public static void CheckMustNotBeLessThan()
    {
        TestMustNotBeLessThan("foo").Should().Be("foo");
        TestMustNotBeLessThanWithDelegate("foo").Should().Be("foo");
        return;

        // I need to provoke a compiler warning
#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
        static string TestMustNotBeLessThan(string? input)
        {
            input.MustNotBeLessThan("bar");
            return input;
        }

        static string TestMustNotBeLessThanWithDelegate(string? input)
        {
            input.MustNotBeLessThan("bar", (_, _) => new Exception());
            return input;
        }
#pragma warning restore CS8631
    }

    [Fact]
    public static void CheckMustBeGreaterThanOrEqualTo()
    {
        TestMustBeGreaterThanOrEqualTo("foo").Should().Be("foo");
        TestMustBeGreaterThanOrEqualToWithDelegate("foo").Should().Be("foo");
        return;

#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
        static string TestMustBeGreaterThanOrEqualTo(string? input)
        {
            input.MustBeGreaterThanOrEqualTo("bar");
            return input;
        }

        static string TestMustBeGreaterThanOrEqualToWithDelegate(string? input)
        {
            input.MustBeGreaterThanOrEqualTo("bar", (_, _) => new Exception());
            return input;
        }
#pragma warning restore CS8631
    }
}