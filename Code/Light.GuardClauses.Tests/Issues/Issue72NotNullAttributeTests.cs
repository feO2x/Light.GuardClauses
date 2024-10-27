#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions;
using Light.GuardClauses.FrameworkExtensions;
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
    public static void CheckMustContainForCollections()
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

    [Fact]
    public static void CheckMustNotBeOneOf()
    {
        TestMustNotBeOneOf("bar").Should().Be(3);
        TestMustNotBeOneOfWithDelegate("bar").Should().Be(3);
        return;

        static int TestMustNotBeOneOf(string? input)
        {
            'o'.MustNotBeOneOf(input!);
            return input.Length;
        }

        static int TestMustNotBeOneOfWithDelegate(string? input)
        {
            'o'.MustNotBeOneOf(input!, (_, _) => new Exception());
            return input.Length;
        }
    }

    [Fact]
    public static void CheckMustHaveMinimumCount()
    {
        TestMustHaveMinimumCount([1, 2, 3]).Should().Be(3);
        TestMustHaveMinimumCountWithDelegate([1, 2, 3]).Should().Be(3);
        return;

        static int TestMustHaveMinimumCount(int[]? input)
        {
            input.MustHaveMinimumCount(2);
            return input.Length;
        }

        static int TestMustHaveMinimumCountWithDelegate(int[]? input)
        {
            input.MustHaveMinimumCount(2, (_, _) => new Exception());
            return input.Length;
        }
    }

    [Fact]
    public static void CheckMustHaveMaximumCount()
    {
        TestMustHaveMaximumCount([1, 2, 3]).Should().Be(3);
        TestMustHaveMaximumCountWithDelegate([1, 2, 3]).Should().Be(3);
        return;

        static int TestMustHaveMaximumCount(int[]? input)
        {
            input.MustHaveMaximumCount(3);
            return input.Length;
        }

        static int TestMustHaveMaximumCountWithDelegate(int[]? input)
        {
            input.MustHaveMaximumCount(3, (_, _) => new Exception());
            return input.Length;
        }
    }

    [Fact]
    public static void CheckMustNotBeNullOrEmptyString()
    {
        TestMustNotBeNullOrEmptyString("foo").Should().Be(3);
        TestMustNotBeNullOrEmptyStringWithDelegate("foo").Should().Be(3);
        return;

        static int TestMustNotBeNullOrEmptyString(string? input)
        {
            input.MustNotBeNullOrEmpty();
            return input.Length;
        }

        static int TestMustNotBeNullOrEmptyStringWithDelegate(string? input)
        {
            input.MustNotBeNullOrEmpty(_ => new Exception());
            return input.Length;
        }
    }

    [Fact]
    public static void CheckMustNotBeNullOrWhiteSpace()
    {
        TestMustNotBeNullOrWhiteSpace("foo").Should().Be(3);
        TestMustNotBeNullOrWhiteSpaceWithDelegate("foo").Should().Be(3);
        return;

        static int TestMustNotBeNullOrWhiteSpace(string? input)
        {
            input.MustNotBeNullOrWhiteSpace();
            return input.Length;
        }

        static int TestMustNotBeNullOrWhiteSpaceWithDelegate(string? input)
        {
            input.MustNotBeNullOrWhiteSpace(_ => new Exception());
            return input.Length;
        }
    }

    [Fact]
    public static void CheckMustMatch()
    {
        TestMustMatch("foo", "^f.*").Should().Be(3);
        TestMustMatchWithDelegate("foo", "^f.*").Should().Be(3);
        return;

        static int TestMustMatch(string? input, string pattern)
        {
            input.MustMatch(new Regex(pattern));
            return input.Length;
        }

        static int TestMustMatchWithDelegate(string? input, string pattern)
        {
            input.MustMatch(new Regex(pattern), (_, _) => new Exception());
            return input.Length;
        }
    }

    [Fact]
    public static void CheckMustContainForStrings()
    {
        TestMustContain("foo").Should().Be(3);
        TestMustContainWithDelegate("foo").Should().Be(3);
        TestMustContainWithStringComparison("foo").Should().Be(3);
        TestMustContainWithDelegateAndStringComparison("foo").Should().Be(3);
        return;

        static int TestMustContain(string? input)
        {
            input.MustContain("oo");
            return input.Length;
        }

        static int TestMustContainWithDelegate(string? input)
        {
            input.MustContain("oo", (_, _) => new Exception());
            return input.Length;
        }

        static int TestMustContainWithStringComparison(string? input)
        {
            input.MustContain("OO", StringComparison.OrdinalIgnoreCase);
            return input.Length;
        }

        static int TestMustContainWithDelegateAndStringComparison(string? input)
        {
            input.MustContain("OO", StringComparison.OrdinalIgnoreCase, (_, _, _) => new Exception());
            return input.Length;
        }
    }

    [Fact]
    public static void CheckMustNotContainForStrings()
    {
        TestMustNotContain("foo").Should().Be(3);
        TestMustNotContainWithDelegate("foo").Should().Be(3);
        TestMustNotContainWithStringComparison("foo").Should().Be(3);
        TestMustNotContainWithDelegateAndStringComparison("foo").Should().Be(3);
        return;

        static int TestMustNotContain(string? input)
        {
            input.MustNotContain("bar");
            return input.Length;
        }

        static int TestMustNotContainWithDelegate(string? input)
        {
            input.MustNotContain("bar", (_, _) => new Exception());
            return input.Length;
        }

        static int TestMustNotContainWithStringComparison(string? input)
        {
            input.MustNotContain("BAR", StringComparison.OrdinalIgnoreCase);
            return input.Length;
        }

        static int TestMustNotContainWithDelegateAndStringComparison(string? input)
        {
            input.MustNotContain("BAR", StringComparison.OrdinalIgnoreCase, (_, _, _) => new Exception());
            return input.Length;
        }
    }

    [Fact]
    public static void CheckMustBeSubstringOf()
    {
        TestMustBeSubstringOf("foo", "foobar").Should().Be(3);
        TestMustBeSubstringOfWithDelegate("foo", "foobar").Should().Be(3);
        TestMustBeSubstringOfWithStringComparison("OO").Should().Be(2);
        TestMustBeSubstringOfWithDelegateAndStringComparison("OO").Should().Be(2);
        return;

        static int TestMustBeSubstringOf(string? input, string target)
        {
            input.MustBeSubstringOf(target);
            return input.Length;
        }

        static int TestMustBeSubstringOfWithDelegate(string? input, string target)
        {
            input.MustBeSubstringOf(target, (_, _) => new Exception());
            return input.Length;
        }

        static int TestMustBeSubstringOfWithStringComparison(string? input)
        {
            input.MustBeSubstringOf("foo", StringComparison.OrdinalIgnoreCase);
            return input.Length;
        }

        static int TestMustBeSubstringOfWithDelegateAndStringComparison(string? input)
        {
            input.MustBeSubstringOf("foo", StringComparison.OrdinalIgnoreCase, (_, _, _) => new Exception());
            return input.Length;
        }
    }

    [Fact]
    public static void CheckMustNotBeSubstringOf()
    {
        TestMustNotBeSubstringOf("foo", "bar").Should().Be(3);
        TestMustNotBeSubstringOfWithDelegate("foo", "bar").Should().Be(3);
        TestMustNotBeSubstringOfWithStringComparison("FOO").Should().Be(3);
        TestMustNotBeSubstringOfWithDelegateAndStringComparison("FOO").Should().Be(3);
        return;

        static int TestMustNotBeSubstringOf(string? input, string target)
        {
            input.MustNotBeSubstringOf(target);
            return input.Length;
        }

        static int TestMustNotBeSubstringOfWithDelegate(string? input, string target)
        {
            input.MustNotBeSubstringOf(target, (_, _) => new Exception());
            return input.Length;
        }

        static int TestMustNotBeSubstringOfWithStringComparison(string? input)
        {
            input.MustNotBeSubstringOf("bar", StringComparison.OrdinalIgnoreCase);
            return input.Length;
        }

        static int TestMustNotBeSubstringOfWithDelegateAndStringComparison(string? input)
        {
            input.MustNotBeSubstringOf("bar", StringComparison.OrdinalIgnoreCase, (_, _, _) => new Exception());
            return input.Length;
        }
    }

    [Fact]
    public static void CheckMustBeEmailAddress()
    {
        TestMustBeEmailAddress("test@example.com").Should().Be("test@example.com");
        TestMustBeEmailAddressWithDelegate("test@example.com").Should().Be("test@example.com");
        TestMustBeEmailAddressWithRegex("test@example.com").Should().Be("test@example.com");
        TestMustBeEmailAddressWithRegexAndDelegate("test@example.com").Should().Be("test@example.com");
        return;

        static string TestMustBeEmailAddress(string? input)
        {
            input.MustBeEmailAddress();
            return input;
        }

        static string TestMustBeEmailAddressWithDelegate(string? input)
        {
            input.MustBeEmailAddress(_ => new Exception());
            return input;
        }

        static string TestMustBeEmailAddressWithRegex(string? input)
        {
            input.MustBeEmailAddress(new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$"));
            return input;
        }

        static string TestMustBeEmailAddressWithRegexAndDelegate(string? input)
        {
            input.MustBeEmailAddress(new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$"), (_, _) => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustBeShorterThan()
    {
        TestMustBeShorterThan("foo").Should().Be("foo");
        TestMustBeShorterThanWithDelegate("foo").Should().Be("foo");
        return;

        static string TestMustBeShorterThan(string? input)
        {
            input.MustBeShorterThan(5);
            return input;
        }

        static string TestMustBeShorterThanWithDelegate(string? input)
        {
            input.MustBeShorterThan(5, (_, _) => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustBeShorterThanOrEqualTo()
    {
        TestMustBeShorterThanOrEqualTo("foo").Should().Be("foo");
        TestMustBeShorterThanOrEqualToWithDelegate("foo").Should().Be("foo");
        return;

        static string TestMustBeShorterThanOrEqualTo(string? input)
        {
            input.MustBeShorterThanOrEqualTo(3);
            return input;
        }

        static string TestMustBeShorterThanOrEqualToWithDelegate(string? input)
        {
            input.MustBeShorterThanOrEqualTo(3, (_, _) => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustHaveLength()
    {
        TestMustHaveLength("foo").Should().Be("foo");
        TestMustHaveLengthWithDelegate("foo").Should().Be("foo");
        return;

        static string TestMustHaveLength(string? input)
        {
            input.MustHaveLength(3);
            return input;
        }

        static string TestMustHaveLengthWithDelegate(string? input)
        {
            input.MustHaveLength(3, (_, _) => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustBeLongerThan()
    {
        TestMustBeLongerThan("foobar").Should().Be("foobar");
        TestMustBeLongerThanWithDelegate("foobar").Should().Be("foobar");
        return;

        static string TestMustBeLongerThan(string? input)
        {
            input.MustBeLongerThan(3);
            return input;
        }

        static string TestMustBeLongerThanWithDelegate(string? input)
        {
            input.MustBeLongerThan(3, (_, _) => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustBeLongerThanOrEqualTo()
    {
        TestMustBeLongerThanOrEqualTo("foobar").Should().Be("foobar");
        TestMustBeLongerThanOrEqualToWithDelegate("foobar").Should().Be("foobar");
        return;

        static string TestMustBeLongerThanOrEqualTo(string? input)
        {
            input.MustBeLongerThanOrEqualTo(6);
            return input;
        }

        static string TestMustBeLongerThanOrEqualToWithDelegate(string? input)
        {
            input.MustBeLongerThanOrEqualTo(6, (_, _) => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustHaveLengthIn()
    {
        TestMustHaveLengthIn("foo").Should().Be("foo");
        TestMustHaveLengthInWithDelegate("foo").Should().Be("foo");
        return;

        static string TestMustHaveLengthIn(string? input)
        {
            input.MustHaveLengthIn(Range.FromInclusive(2).ToInclusive(4));
            return input;
        }

        static string TestMustHaveLengthInWithDelegate(string? input)
        {
            input.MustHaveLengthIn(Range.FromInclusive(2).ToInclusive(4), (_, _) => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustBeNewLine()
    {
        TestMustBeNewLine("\n").Should().Be("\n");
        TestMustBeNewLineWithDelegate("\n").Should().Be("\n");
        return;

        static string TestMustBeNewLine(string? input)
        {
            input.MustBeNewLine();
            return input;
        }

        static string TestMustBeNewLineWithDelegate(string? input)
        {
            input.MustBeNewLine(_ => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustBeTrimmed()
    {
        TestMustBeTrimmed("foo").Should().Be("foo");
        TestMustBeTrimmedWithDelegate("foo").Should().Be("foo");
        return;

        static string TestMustBeTrimmed(string? input)
        {
            input.MustBeTrimmed();
            return input;
        }

        static string TestMustBeTrimmedWithDelegate(string? input)
        {
            input.MustBeTrimmed(_ => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustBeTrimmedAtStart()
    {
        TestMustBeTrimmedAtStart("foo").Should().Be("foo");
        TestMustBeTrimmedAtStartWithDelegate("foo").Should().Be("foo");
        return;

        static string TestMustBeTrimmedAtStart(string? input)
        {
            input.MustBeTrimmedAtStart();
            return input;
        }

        static string TestMustBeTrimmedAtStartWithDelegate(string? input)
        {
            input.MustBeTrimmedAtStart(_ => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustBeTrimmedAtEnd()
    {
        TestMustBeTrimmedAtEnd("foo").Should().Be("foo");
        TestMustBeTrimmedAtEndWithDelegate("foo").Should().Be("foo");
        return;

        static string TestMustBeTrimmedAtEnd(string? input)
        {
            input.MustBeTrimmedAtEnd();
            return input;
        }

        static string TestMustBeTrimmedAtEndWithDelegate(string? input)
        {
            input.MustBeTrimmedAtEnd(_ => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckImplements()
    {
        TestImplements(typeof(FileInfo), typeof(ISerializable)).Should().Be((typeof(FileInfo), typeof(ISerializable)));
        TestImplementsWithComparer(typeof(FileInfo), typeof(ISerializable)).Should().Be((typeof(FileInfo), typeof(ISerializable), EqualityComparer<Type>.Default));
        return;

        static (Type Type, Type InterfaceType) TestImplements(Type? type, Type? interfaceType)
        {
            type!.Implements(interfaceType!);
            return (type, interfaceType);
        }

        static (Type Type, Type InterfaceType, IEqualityComparer<Type> Comparer) TestImplementsWithComparer(Type? type, Type? interfaceType)
        {
            type!.Implements(interfaceType!, EqualityComparer<Type>.Default);
            return (type, interfaceType, EqualityComparer<Type>.Default);
        }
    }

    [Fact]
    public static void CheckIsOrImplements()
    {
        TestIsOrImplements(typeof(FileInfo), typeof(ISerializable)).Should().Be((typeof(FileInfo), typeof(ISerializable)));
        TestIsOrImplementsWithComparer(typeof(FileInfo), typeof(ISerializable)).Should().Be((typeof(FileInfo), typeof(ISerializable), EqualityComparer<Type>.Default));
        return;

        static (Type Type, Type InterfaceType) TestIsOrImplements(Type? type, Type? interfaceType)
        {
            type!.IsOrImplements(interfaceType!);
            return (type, interfaceType);
        }

        static (Type Type, Type InterfaceType, IEqualityComparer<Type> Comparer) TestIsOrImplementsWithComparer(Type? type, Type? interfaceType)
        {
            type!.IsOrImplements(interfaceType!, EqualityComparer<Type>.Default);
            return (type, interfaceType, EqualityComparer<Type>.Default);
        }
    }

    [Fact]
    public static void CheckDerivesFrom()
    {
        TestDerivesFrom(typeof(FileStream), typeof(Stream)).Should().Be((typeof(FileStream), typeof(Stream)));
        TestDerivesFromWithTypeComparer(typeof(FileStream), typeof(Stream)).Should().Be((typeof(FileStream), typeof(Stream)));
        return;

        static (Type Type, Type BaseType) TestDerivesFrom(Type? type, Type? baseType)
        {
            type!.DerivesFrom(baseType!);
            return (type, baseType);
        }

        static (Type Type, Type BaseType) TestDerivesFromWithTypeComparer(Type? type, Type? baseType)
        {
            type!.DerivesFrom(baseType!, EqualityComparer<Type>.Default);
            return (type, baseType);
        }
    }

    [Fact]
    public static void CheckIsOrDerivesFrom()
    {
        TestIsOrDerivesFrom(typeof(FileStream), typeof(Stream)).Should().Be((typeof(FileStream), typeof(Stream)));
        TestIsOrDerivesFromWithComparer(typeof(FileStream), typeof(Stream)).Should().Be((typeof(FileStream), typeof(Stream), EqualityComparer<Type>.Default));
        return;

        static (Type Type, Type BaseType) TestIsOrDerivesFrom(Type? type, Type? baseType)
        {
            type!.IsOrDerivesFrom(baseType!);
            return (type, baseType);
        }

        static (Type Type, Type BaseType, IEqualityComparer<Type> Comparer) TestIsOrDerivesFromWithComparer(Type? type, Type? baseType)
        {
            type!.IsOrDerivesFrom(baseType!, EqualityComparer<Type>.Default);
            return (type, baseType, EqualityComparer<Type>.Default);
        }
    }

    [Fact]
    public static void CheckInheritsFrom()
    {
        TestInheritsFrom(typeof(FileStream), typeof(Stream)).Should().Be((typeof(FileStream), typeof(Stream)));
        TestInheritsFromWithTypeComparer(typeof(FileStream), typeof(Stream)).Should().Be((typeof(FileStream), typeof(Stream), EqualityComparer<Type>.Default));
        return;

        static (Type Type, Type BaseType) TestInheritsFrom(Type? type, Type? baseType)
        {
            type!.InheritsFrom(baseType!);
            return (type, baseType);
        }

        static (Type Type, Type BaseType, IEqualityComparer<Type> Comparer) TestInheritsFromWithTypeComparer(Type? type, Type? baseType)
        {
            type!.InheritsFrom(baseType!, EqualityComparer<Type>.Default);
            return (type, baseType, EqualityComparer<Type>.Default);
        }
    }


    [Fact]
    public static void CheckIsOrInheritsFrom()
    {
        TestIsOrInheritsFrom(typeof(FileStream), typeof(Stream)).Should().Be((typeof(FileStream), typeof(Stream)));
        TestIsOrInheritsFromWithComparer(typeof(FileStream), typeof(Stream)).Should().Be((typeof(FileStream), typeof(Stream), EqualityComparer<Type>.Default));
        return;

        static (Type Type, Type BaseType) TestIsOrInheritsFrom(Type? type, Type? baseType)
        {
            type!.IsOrInheritsFrom(baseType!);
            return (type, baseType);
        }

        static (Type Type, Type BaseType, IEqualityComparer<Type> Comparer) TestIsOrInheritsFromWithComparer(Type? type, Type? baseType)
        {
            type!.IsOrInheritsFrom(baseType!, EqualityComparer<Type>.Default);
            return (type, baseType, EqualityComparer<Type>.Default);
        }
    }

    [Fact]
    public static void CheckMustBeAbsoluteUri()
    {
        TestMustBeAbsoluteUri(new Uri("https://example.com")).Should().Be(new Uri("https://example.com"));
        TestMustBeAbsoluteUriWithDelegate(new Uri("https://example.com")).Should().Be(new Uri("https://example.com"));
        return;

        static Uri TestMustBeAbsoluteUri(Uri? input)
        {
            input.MustBeAbsoluteUri();
            return input;
        }

        static Uri TestMustBeAbsoluteUriWithDelegate(Uri? input)
        {
            input.MustBeAbsoluteUri(_ => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustBeRelativeUri()
    {
        TestMustBeRelativeUri(new Uri("/relative/path", UriKind.Relative)).Should().Be(new Uri("/relative/path", UriKind.Relative));
        TestMustBeRelativeUriWithDelegate(new Uri("/relative/path", UriKind.Relative)).Should().Be(new Uri("/relative/path", UriKind.Relative));
        return;

        static Uri TestMustBeRelativeUri(Uri? input)
        {
            input.MustBeRelativeUri();
            return input;
        }

        static Uri TestMustBeRelativeUriWithDelegate(Uri? input)
        {
            input.MustBeRelativeUri(_ => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustHaveScheme()
    {
        TestMustHaveScheme(new Uri("https://example.com"), "https").Should().Be(new Uri("https://example.com"));
        TestMustHaveSchemeWithDelegate(new Uri("https://example.com"), "https").Should().Be(new Uri("https://example.com"));
        TestMustHaveSchemeWithSecondDelegate(new Uri("https://example.com"), "https").Should().Be(new Uri("https://example.com"));
        return;

        static Uri TestMustHaveScheme(Uri? input, string scheme)
        {
            input.MustHaveScheme(scheme);
            return input;
        }

        static Uri TestMustHaveSchemeWithDelegate(Uri? input, string scheme)
        {
            input.MustHaveScheme(scheme, _ => new Exception());
            return input;
        }

        static Uri TestMustHaveSchemeWithSecondDelegate(Uri? input, string scheme)
        {
            input.MustHaveScheme(scheme, (_, _) => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustBeHttpsUrl()
    {
        TestMustBeHttpsUrl(new Uri("https://example.com")).Should().Be(new Uri("https://example.com"));
        TestMustBeHttpsUrlWithDelegate(new Uri("https://example.com")).Should().Be(new Uri("https://example.com"));
        return;

        static Uri TestMustBeHttpsUrl(Uri? input)
        {
            input.MustBeHttpsUrl();
            return input;
        }

        static Uri TestMustBeHttpsUrlWithDelegate(Uri? input)
        {
            input.MustBeHttpsUrl(_ => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustBeHttpUrl()
    {
        TestMustBeHttpUrl(new Uri("http://example.com")).Should().Be(new Uri("http://example.com"));
        TestMustBeHttpUrlWithDelegate(new Uri("http://example.com")).Should().Be(new Uri("http://example.com"));
        return;

        static Uri TestMustBeHttpUrl(Uri? input)
        {
            input.MustBeHttpUrl();
            return input;
        }

        static Uri TestMustBeHttpUrlWithDelegate(Uri? input)
        {
            input.MustBeHttpUrl(_ => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustBeHttpOrHttpsUrl()
    {
        TestMustBeHttpOrHttpsUrl(new Uri("https://example.com")).Should().Be(new Uri("https://example.com"));
        TestMustBeHttpOrHttpsUrlWithDelegate(new Uri("https://example.com")).Should().Be(new Uri("https://example.com"));
        return;

        static Uri TestMustBeHttpOrHttpsUrl(Uri? input)
        {
            input.MustBeHttpOrHttpsUrl();
            return input;
        }

        static Uri TestMustBeHttpOrHttpsUrlWithDelegate(Uri? input)
        {
            input.MustBeHttpOrHttpsUrl(_ => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckMustHaveOneSchemeOf()
    {
        TestMustHaveOneSchemeOf(new Uri("https://example.com")).Should().Be(new Uri("https://example.com"));
        TestMustHaveOneSchemeOfWithDelegate(new Uri("https://example.com")).Should().Be(new Uri("https://example.com"));
        return;

        static Uri TestMustHaveOneSchemeOf(Uri? input)
        {
            input.MustHaveOneSchemeOf(["http", "https"]);
            return input;
        }

        static Uri TestMustHaveOneSchemeOfWithDelegate(Uri? input)
        {
            input.MustHaveOneSchemeOf(new[] { "http", "https" }, (_, _) => new Exception());
            return input;
        }
    }

    [Fact]
    public static void CheckIsIn()
    {
        TestIsIn("k").Should().Be("k");
        return;

        static string TestIsIn(string? input)
        {
            input!.IsIn(Range.FromInclusive("a").ToInclusive("z"));
            return input;
        }
    }

    [Fact]
    public static void CheckIsNotIn()
    {
        TestIsNotIn("k").Should().Be("k");
        return;

        static string TestIsNotIn(string? input)
        {
            input!.IsNotIn(Range.FromInclusive("a").ToInclusive("h"));
            return input;
        }
    }

    [Fact]
    public static void CheckIsSubstringOf()
    {
        TestIsSubstringOf("foo", "oo").Should().Be("foo");
        TestIsSubstringOfWithComparison("foo", "oo").Should().Be("foo");
        return;

        static string TestIsSubstringOf(string? input, string? comparison)
        {
            comparison!.IsSubstringOf(input!);
            return input;
        }

        static string TestIsSubstringOfWithComparison(string? input, string? comparison)
        {
            comparison!.IsSubstringOf(input!, StringComparison.OrdinalIgnoreCase);
            return input;
        }
    }

    [Fact]
    public static void CheckIsOpenConstructedGenericType()
    {
        TestIsOpenConstructedGenericType(typeof(List<>)).Should().Be(typeof(List<>));
        return;

        static Type TestIsOpenConstructedGenericType(Type? type)
        {
            type!.IsOpenConstructedGenericType();
            return type;
        }
    }

    [Fact]
    public static void CheckAsList()
    {
        TestAsList(new List<int>()).Should().Be(0);
        TestAsListWithDelegate(new List<int>()).Should().Be(0);
        return;

        static int TestAsList(IList<int>? list)
        {
            list!.AsList();
            return list.Count;
        }

        static int TestAsListWithDelegate(IList<int>? list)
        {
            list!.AsList(collection => new List<int>(collection));
            return list.Count;
        }
    }

    [Fact]
    public static void CheckAsArray()
    {
        TestAsArray([1, 2, 3]).Should().Be(3);
        return;

        static int TestAsArray(int[]? array)
        {
            array!.AsArray();
            return array.Length;
        }
    }

    [Fact]
    public static void CheckForEach()
    {
        TestForEach(["foo", "bar"]).Should().Be(2);
        return;

        static int TestForEach(string[]? array)
        {
            array!.ForEach(_ => { });
            return array.Length;
        }
    }

    [Fact]
    public static void CheckAsReadOnlyList()
    {
        TestAsReadOnlyList([1, 2, 3]).Should().Be(3);
        TestAsReadOnlyListWithDelegate([1, 2, 3], collection => new List<int>(collection)).Should().Be(3);
        return;

        static int TestAsReadOnlyList(int[]? array)
        {
            array!.AsReadOnlyList();
            return array.Length;
        }

        static int TestAsReadOnlyListWithDelegate(int[]? array, Func<IEnumerable<int>, IReadOnlyList<int>>? collectionFactory)
        {
            array!.AsReadOnlyList(collectionFactory!);
            return array.Length;
        }
    }

    [Fact]
    public static void CheckCount()
    {
        TestCount(new ArrayList(new[] { 1, 2, 3 })).Should().Be(3);
        TestCountWithParameterNameAndMessage(new ArrayList(new[] { 1, 2, 3 })).Should().Be(3);
        return;

        static int TestCount(ICollection? collection)
        {
            collection!.Count();
            return collection.Count;
        }

        static int TestCountWithParameterNameAndMessage(ICollection? collection)
        {
            collection!.Count(nameof(collection), "The collection must not be null");
            return collection.Count;
        }
    }

    [Fact]
    public static void CheckGetCount()
    {
        TestGetCount([1, 2, 3]).Should().Be(3);
        TestGetCountWithParameterNameAndMessage([1, 2, 3]).Should().Be(3);
        return;

        static int TestGetCount<T>(ICollection<T>? collection)
        {
            collection!.GetCount();
            return collection.Count;
        }

        static int TestGetCountWithParameterNameAndMessage<T>(ICollection<T>? collection)
        {
            collection!.GetCount(nameof(collection), "The collection must not be null");
            return collection.Count;
        }
    }

    [Fact]
    public static void CheckExtractProperty()
    {
        TestExtractProperty((string a) => a.Length).Should().NotBeNull();
        return;

        static Type TestExtractProperty<T, TProperty>(Expression<Func<T, TProperty>>? expression)
        {
            expression!.ExtractProperty();
            return expression.Type;
        }
    }

    [Fact]
    public static void CheckExtractField()
    {
        TestExtractField((TestClassWithPublicField x) => x.PublicField).Should().NotBeNull();
        return;

        static Type TestExtractField<T, TField>(Expression<Func<T, TField>>? expression)
        {
            expression!.ExtractField();
            return expression.Type;
        }
    }

    private sealed class TestClassWithPublicField
    {
#pragma warning disable CS0414 // Field is assigned but its value is never used
        // ReSharper disable once ConvertToConstant.Local
        public readonly int PublicField = 42;
#pragma warning restore CS0414
    }

    [Fact]
    public static void CheckAppendCollectionContent()
    {
        var stringBuilder = new StringBuilder();
        TestAppendCollectionContent(stringBuilder, [1, 2, 3]).Should().Be((stringBuilder, 3));
        return;
        
        static (StringBuilder, int) TestAppendCollectionContent<T>(StringBuilder? stringBuilder, ICollection<T>? items)
        {
            stringBuilder!.AppendCollectionContent(items!);
            return (stringBuilder, items.Count);
        }
    }

    [Fact]
    public static void CheckToStringRepresentation()
    {
        TestToStringRepresentation("foo").Should().Be(3);
        return;
        
        static int TestToStringRepresentation(string? value)
        {
            value.ToStringRepresentation();
            return value.Length;
        }
    }

    [Fact]
    public static void CheckAppendItemsWithNewLine()
    {
        var stringBuilder = new StringBuilder();
        TestAppendItemsWithNewLine(stringBuilder, [1, 2, 3]).Should().Be((stringBuilder, 3));
        return;
        
        static (StringBuilder, int) TestAppendItemsWithNewLine<T>(StringBuilder? stringBuilder, ICollection<T>? items)
        {
            stringBuilder!.AppendItemsWithNewLine(items!);
            return (stringBuilder, items.Count);
        }
    }

    [Fact]
    public static void CheckAppendIf()
    {
        var stringBuilder = new StringBuilder();
        TestAppendIf(stringBuilder, true, "foo").Should().Be((stringBuilder, 3));
        return;
        
        static (StringBuilder, int) TestAppendIf(StringBuilder? stringBuilder, bool condition, string value)
        {
            stringBuilder!.AppendIf(condition, value);
            return (stringBuilder, value.Length);
        }
    }

    [Fact]
    public static void CheckAppendLineIf()
    {
        var stringBuilder = new StringBuilder();
        TestAppendLineIf(stringBuilder, true, "foo").Should().Be((stringBuilder, 3));
        return;
        
        static (StringBuilder, int) TestAppendLineIf(StringBuilder? stringBuilder, bool condition, string value)
        {
            stringBuilder!.AppendLineIf(condition, value);
            return (stringBuilder, value.Length);
        }
    }

    [Fact]
    public static void CheckAppendItems()
    {
        var stringBuilder = new StringBuilder();
        TestAppendItems(stringBuilder, [1, 2, 3]).Should().Be((stringBuilder, 3));
        return;
        
        static (StringBuilder, int) TestAppendItems<T>(StringBuilder? stringBuilder, ICollection<T>? items)
        {
            stringBuilder!.AppendItems(items!);
            return (stringBuilder, items.Count);
        }
    }
}