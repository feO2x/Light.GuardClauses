#nullable enable

using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.Issues;

// This occurs for delegates that create exceptions, and we just turn it off for these tests
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate type.

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

    [Fact]
    public static void CheckMustBeLessThan()
    {
        TestMustBeLessThan("bar").Should().Be("bar");
        TestMustBeLessThanWithDelegate("bar").Should().Be("bar");
        return;

#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
        static string TestMustBeLessThan(string? input)
        {
            input.MustBeLessThan("foo");
            return input;
        }

        static string TestMustBeLessThanWithDelegate(string? input)
        {
            input.MustBeLessThan("foo", (_, _) => new Exception());
            return input;
        }
#pragma warning restore CS8631
    }

    [Fact]
    public static void CheckMustNotBeGreaterThanOrEqualTo()
    {
        TestMustNotBeGreaterThanOrEqualTo("bar").Should().Be("bar");
        TestMustNotBeGreaterThanOrEqualToWithDelegate("bar").Should().Be("bar");
        return;

#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
        static string TestMustNotBeGreaterThanOrEqualTo(string? input)
        {
            input.MustNotBeGreaterThanOrEqualTo("foo");
            return input;
        }

        static string TestMustNotBeGreaterThanOrEqualToWithDelegate(string? input)
        {
            input.MustNotBeGreaterThanOrEqualTo("foo", (_, _) => new Exception());
            return input;
        }
#pragma warning restore CS8631
    }

    [Fact]
    public static void CheckMustBeGreaterThan()
    {
        TestMustBeGreaterThan("foo").Should().Be("foo");
        TestMustBeGreaterThanWithDelegate("foo").Should().Be("foo");
        return;

#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
        static string TestMustBeGreaterThan(string? input)
        {
            input.MustBeGreaterThan("bar");
            return input;
        }

        static string TestMustBeGreaterThanWithDelegate(string? input)
        {
            input.MustBeGreaterThan("bar", (_, _) => new Exception());
            return input;
        }
#pragma warning restore CS8631
    }

    [Fact]
    public static void CheckMustNotBeLessThanOrEqualTo()
    {
        TestMustNotBeLessThanOrEqualTo("foo").Should().Be("foo");
        TestMustNotBeLessThanOrEqualToWithDelegate("foo").Should().Be("foo");
        return;

#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
        static string TestMustNotBeLessThanOrEqualTo(string? input)
        {
            input.MustNotBeLessThanOrEqualTo("bar");
            return input;
        }

        static string TestMustNotBeLessThanOrEqualToWithDelegate(string? input)
        {
            input.MustNotBeLessThanOrEqualTo("bar", (_, _) => new Exception());
            return input;
        }
#pragma warning restore CS8631
    }

    [Fact]
    public static void CheckMustNotBeGreaterThan()
    {
        TestMustNotBeGreaterThan("bar").Should().Be("bar");
        TestMustNotBeGreaterThanWithDelegate("bar").Should().Be("bar");
        return;

#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
        static string TestMustNotBeGreaterThan(string? input)
        {
            input.MustNotBeGreaterThan("foo");
            return input;
        }

        static string TestMustNotBeGreaterThanWithDelegate(string? input)
        {
            input.MustNotBeGreaterThan("foo", (_, _) => new Exception());
            return input;
        }
#pragma warning restore CS8631
    }

    [Fact]
    public static void CheckMustBeLessThanOrEqualTo()
    {
        TestMustBeLessThanOrEqualTo("bar").Should().Be("bar");
        TestMustBeLessThanOrEqualToWithDelegate("bar").Should().Be("bar");
        return;

#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
        static string TestMustBeLessThanOrEqualTo(string? input)
        {
            input.MustBeLessThanOrEqualTo("foo");
            return input;
        }

        static string TestMustBeLessThanOrEqualToWithDelegate(string? input)
        {
            input.MustBeLessThanOrEqualTo("foo", (_, _) => new Exception());
            return input;
        }
#pragma warning restore CS8631
    }

    [Fact]
    public static void CheckMustBeIn()
    {
        TestMustBeIn("b").Should().Be(1);
        TestMustBeInWithDelegate("b").Should().Be(1);

#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
        static int TestMustBeIn(string? input)
        {
            input.MustBeIn(Range.FromInclusive("a").ToInclusive("c")!);
            return input.Length;
        }

        static int TestMustBeInWithDelegate(string? input)
        {
            input.MustBeIn(Range.FromInclusive("a").ToInclusive("c")!, (_, _) => new Exception());
            return input.Length;
        }
#pragma warning restore CS8631
    }

    [Fact]
    public static void CheckMustNotBeIn()
    {
        TestMustNotBeIn("d").Should().Be(1);
        TestMustNotBeInWithDelegate("d").Should().Be(1);

#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
        static int TestMustNotBeIn(string? input)
        {
            input.MustNotBeIn(Range.FromInclusive("a").ToInclusive("c")!);
            return input.Length;
        }

        static int TestMustNotBeInWithDelegate(string? input)
        {
            input.MustNotBeIn(Range.FromInclusive("a").ToInclusive("c")!, (_, _) => new Exception());
            return input.Length;
        }
#pragma warning restore CS8631
    }

    [Fact]
    public static void CheckMustHaveCount()
    {
        TestMustHaveCount([]).Should().Be(0);
        TestMustHaveCountWithDelegate([]).Should().Be(0);
        return;

        static int TestMustHaveCount(string[]? input)
        {
            input.MustHaveCount(0);
            return input.Length;
        }

        static int TestMustHaveCountWithDelegate(string[]? input)
        {
            input.MustHaveCount(0, (_, _) => new Exception());
            return input.Length;
        }
    }

    [Fact]
    public static void CheckMustNotBeNullOrEmpty()
    {
        TestMustNotBeNullOrEmpty([1, 2, 3]).Should().Be(3);
        TestMustNotBeNullOrEmptyWithDelegate([1, 2, 3]).Should().Be(3);
        return;

        static int TestMustNotBeNullOrEmpty(int[]? input)
        {
            input.MustNotBeNullOrEmpty();
            return input.Length;
        }

        static int TestMustNotBeNullOrEmptyWithDelegate(int[]? input)
        {
            input.MustNotBeNullOrEmpty(_ => new Exception());
            return input.Length;
        }
    }

    [Fact]
    public static void CheckMustContain()
    {
        TestMustContain([1, 2, 3]).Should().Be(3);
        TestMustContainWithDelegate([1, 2, 3]).Should().Be(3);
        return;

        static int TestMustContain(int[]? input)
        {
            input.MustContain(2);
            return input.Length;
        }

        static int TestMustContainWithDelegate(int[]? input)
        {
            input.MustContain(2, (_, _) => new Exception());
            return input.Length;
        }
    }

    [Fact]
    public static void CheckMustNotContain()
    {
        TestMustNotContain([1, 2, 3]).Should().Be(3);
        TestMustNotContainWithDelegate([1, 2, 3]).Should().Be(3);
        return;

        static int TestMustNotContain(int[]? input)
        {
            input.MustNotContain(4);
            return input.Length;
        }

        static int TestMustNotContainWithDelegate(int[]? input)
        {
            input.MustNotContain(4, (_, _) => new Exception());
            return input.Length;
        }
    }

    [Fact]
    public static void CheckIsOneOf()
    {
        TestIsOneOf("foo").Should().Be(3);
        return;

        static int TestIsOneOf(string? input)
        {
            'o'.IsOneOf(input!);
            return input.Length;
        }
    }

    [Fact]
    public static void CheckMustBeOneOf()
    {
        TestMustBeOneOf("foo").Should().Be(3);
        TestMustBeOneOfWithDelegate("foo").Should().Be(3);
        return;

        static int TestMustBeOneOf(string? input)
        {
            'o'.MustBeOneOf(input!);
            return input.Length;
        }

        static int TestMustBeOneOfWithDelegate(string? input)
        {
            'o'.MustBeOneOf(input!, (_, _) => new Exception());
            return input.Length;
        }
    }
}