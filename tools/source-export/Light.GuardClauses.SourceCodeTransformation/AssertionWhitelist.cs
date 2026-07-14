using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Light.GuardClauses.SourceCodeTransformation;

public sealed class AssertionWhitelist
{
    public bool IsEnabled { get; init; } = false;

    public AssertionEntry DerivesFrom { get; init; } = new();

    public new AssertionEntry Equals { get; init; } = new();

    public AssertionEntry Implements { get; init; } = new();

    public AssertionEntry InheritsFrom { get; init; } = new();

    public AssertionEntry InvalidArgument { get; init; } = new();

    public AssertionEntry InvalidOperation { get; init; } = new();

    public AssertionEntry InvalidState { get; init; } = new();

    public AssertionEntry IsAscii { get; init; } = new();

    public AssertionEntry IsApproximately { get; init; } = new();

    public AssertionEntry IsDigit { get; init; } = new();

    public AssertionEntry IsEmailAddress { get; init; } = new();

    public AssertionEntry IsEmpty { get; init; } = new();

    public AssertionEntry IsEmptyOrWhiteSpace { get; init; } = new();

    public AssertionEntry IsEquivalentTypeTo { get; init; } = new();

    public AssertionEntry IsFileExtension { get; init; } = new();

    public AssertionEntry IsFinite { get; init; } = new();

    public AssertionEntry IsGreaterThanOrApproximately { get; init; } = new();

    public AssertionEntry IsIn { get; init; } = new();

    public AssertionEntry IsLessThanOrApproximately { get; init; } = new();

    public AssertionEntry IsLetter { get; init; } = new();

    public AssertionEntry IsLetterOrDigit { get; init; } = new();

    public AssertionEntry IsNewLine { get; init; } = new();

    public AssertionEntry IsNotIn { get; init; } = new();

    public AssertionEntry IsNullOrEmpty { get; init; } = new();

    public AssertionEntry IsNullOrWhiteSpace { get; init; } = new();

    public AssertionEntry IsOneOf { get; init; } = new();

    public AssertionEntry IsOpenConstructedGenericType { get; init; } = new();

    public AssertionEntry IsOrDerivesFrom { get; init; } = new();

    public AssertionEntry IsOrImplements { get; init; } = new();

    public AssertionEntry IsOrInheritsFrom { get; init; } = new();

    public AssertionEntry IsSameAs { get; init; } = new();

    public AssertionEntry IsSubstringOf { get; init; } = new();

    public AssertionEntry IsTrimmed { get; init; } = new();

    public AssertionEntry IsTrimmedAtEnd { get; init; } = new();

    public AssertionEntry IsTrimmedAtStart { get; init; } = new();

    public AssertionEntry IsUuidVersion7 { get; init; } = new();

    public AssertionEntry IsValidEnumValue { get; init; } = new();

    public AssertionEntry IsWhiteSpace { get; init; } = new();

    public AssertionEntry MustBeAscii { get; init; } = new();

    public AssertionEntry MustBe { get; init; } = new();

    public AssertionEntry MustBeAbsoluteUri { get; init; } = new();

    public AssertionEntry MustBeApproximately { get; init; } = new();

    public AssertionEntry MustBeEmailAddress { get; init; } = new();

    public AssertionEntry MustBeFileExtension { get; init; } = new();

    public AssertionEntry MustBeFinite { get; init; } = new();

    public AssertionEntry MustBeGreaterThan { get; init; } = new();

    public AssertionEntry MustBeGreaterThanOrApproximately { get; init; } = new();

    public AssertionEntry MustBeGreaterThanOrEqualTo { get; init; } = new();

    public AssertionEntry MustBeHttpOrHttpsUrl { get; init; } = new();

    public AssertionEntry MustBeHttpUrl { get; init; } = new();

    public AssertionEntry MustBeHttpsUrl { get; init; } = new();

    public AssertionEntry MustBeIn { get; init; } = new();

    public AssertionEntry MustBeLessThan { get; init; } = new();

    public AssertionEntry MustBeLessThanOrApproximately { get; init; } = new();

    public AssertionEntry MustBeLessThanOrEqualTo { get; init; } = new();

    public AssertionEntry MustBeLocal { get; init; } = new();

    public AssertionEntry MustBeLongerThan { get; init; } = new();

    public AssertionEntry MustBeLongerThanOrEqualTo { get; init; } = new();

    public AssertionEntry MustBeNegative { get; init; } = new();

    public AssertionEntry MustBeNewLine { get; init; } = new();

    public AssertionEntry MustBeOfType { get; init; } = new();

    public AssertionEntry MustBeOneOf { get; init; } = new();

    public AssertionEntry MustBePositive { get; init; } = new();

    public AssertionEntry MustBeRelativeUri { get; init; } = new();

    public AssertionEntry MustBeShorterThan { get; init; } = new();

    public AssertionEntry MustBeShorterThanOrEqualTo { get; init; } = new();

    public AssertionEntry MustBeSubstringOf { get; init; } = new();

    public AssertionEntry MustBeTrimmed { get; init; } = new();

    public AssertionEntry MustBeTrimmedAtEnd { get; init; } = new();

    public AssertionEntry MustBeTrimmedAtStart { get; init; } = new();

    public AssertionEntry MustBeUnspecified { get; init; } = new();

    public AssertionEntry MustBeUtc { get; init; } = new();

    public AssertionEntry MustBeUuidVersion7 { get; init; } = new();

    public AssertionEntry MustBeValidEnumValue { get; init; } = new();

    public AssertionEntry MustContain { get; init; } = new();

    public AssertionEntry MustContainKey { get; init; } = new();

    public AssertionEntry MustEndWith { get; init; } = new();

    public AssertionEntry MustHaveCount { get; init; } = new();

    public AssertionEntry MustHaveCountIn { get; init; } = new();

    public AssertionEntry MustHaveLength { get; init; } = new();

    public AssertionEntry MustHaveLengthIn { get; init; } = new();

    public AssertionEntry MustHaveMaximumCount { get; init; } = new();

    public AssertionEntry MustHaveMaximumLength { get; init; } = new();

    public AssertionEntry MustHaveMinimumCount { get; init; } = new();

    public AssertionEntry MustHaveMinimumLength { get; init; } = new();

    public AssertionEntry MustHaveOneSchemeOf { get; init; } = new();

    public AssertionEntry MustHaveScheme { get; init; } = new();

    public AssertionEntry MustHaveValue { get; init; } = new();

    public AssertionEntry MustMatch { get; init; } = new();

    public AssertionEntry MustNotBe { get; init; } = new();

    public AssertionEntry MustNotBeApproximately { get; init; } = new();

    public AssertionEntry MustNotBeDefault { get; init; } = new();

    public AssertionEntry MustNotBeDefaultOrEmpty { get; init; } = new();

    public AssertionEntry MustNotBeEmpty { get; init; } = new();

    public AssertionEntry MustNotBeEmptyOrWhiteSpace { get; init; } = new();

    public AssertionEntry MustNotBeGreaterThan { get; init; } = new();

    public AssertionEntry MustNotBeGreaterThanOrEqualTo { get; init; } = new();

    public AssertionEntry MustNotBeIn { get; init; } = new();

    public AssertionEntry MustNotBeLessThan { get; init; } = new();

    public AssertionEntry MustNotBeLessThanOrEqualTo { get; init; } = new();

    public AssertionEntry MustNotBeNegative { get; init; } = new();

    public AssertionEntry MustNotBeNull { get; init; } = new();

    public AssertionEntry MustNotBeNullOrEmpty { get; init; } = new();

    public AssertionEntry MustNotBeNullOrWhiteSpace { get; init; } = new();

    public AssertionEntry MustNotBeNullReference { get; init; } = new();

    public AssertionEntry MustNotBeOneOf { get; init; } = new();

    public AssertionEntry MustNotBePositive { get; init; } = new();

    public AssertionEntry MustNotBeSameAs { get; init; } = new();

    public AssertionEntry MustNotBeSubstringOf { get; init; } = new();

    public AssertionEntry MustNotBeZero { get; init; } = new();

    public AssertionEntry MustNotContain { get; init; } = new();

    public AssertionEntry MustNotContainKey { get; init; } = new();

    public AssertionEntry MustNotContainNull { get; init; } = new();

    public AssertionEntry MustNotContainNullOrWhiteSpace { get; init; } = new();

    public AssertionEntry MustNotEndWith { get; init; } = new();

    public AssertionEntry MustNotStartWith { get; init; } = new();

    public AssertionEntry MustStartWith { get; init; } = new();

    internal IReadOnlyDictionary<string, AssertionEntry> GetEntriesByAssertionName() =>
        GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                 .Where(property => property.PropertyType == typeof(AssertionEntry))
                 .ToDictionary(
                      property => property.Name,
                      property => (AssertionEntry) property.GetValue(this)!,
                      StringComparer.Ordinal
                  );

    public sealed record AssertionEntry
    {
        public bool Include { get; init; } = true;

        public bool IncludeExceptionFactoryOverload { get; init; } = true;
    }
}
