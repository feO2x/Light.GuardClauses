using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.SourceCodeTransformation.Tests;

public static class SourceFileMergerWhitelistTests
{
    private static readonly DirectoryInfo SourceDirectory = TestEnvironment.SourceDirectory;

    [Fact]
    public static void DisabledWhitelistIgnoresAllEntriesAndProducesEquivalentOutput()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var baselineFile = Path.Combine(temporaryDirectory.DirectoryPath, "Baseline.cs");
        var disabledWhitelistFile = Path.Combine(temporaryDirectory.DirectoryPath, "DisabledWhitelist.cs");

        SourceFileMerger.CreateSingleSourceFile(CreateOptions(baselineFile));
        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                disabledWhitelistFile,
                CreateWhitelist(
                    false,
                    [new ("MustBeGreaterThan", false)]
                )
            )
        );

        File.ReadAllText(disabledWhitelistFile).Should().Be(File.ReadAllText(baselineFile));
    }

    [Fact]
    public static void WhitelistModeTrimsUnrelatedHelpersAndPullsCrossAssertionDependencies()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustBeGreaterThan.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions: [new ("MustBeGreaterThan", false)]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("MustBeGreaterThan<T>");
        sourceCode.Should().Contain("MustNotBeNullReference<T>");
        sourceCode.Should().Contain("public static void MustBeGreaterThan");
        sourceCode.Should().NotContain("class EnumerableExtensions");
        sourceCode.Should().NotContain("struct Range<T>");
        sourceCode.Should().NotContain("Func<T, T, Exception> exceptionFactory");
    }

    [Fact]
    public static void WhitelistModePrunesBundledThrowMembersAndRetainsExceptionInheritance()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustBeAbsoluteUri.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions: [new ("MustBeAbsoluteUri", false)]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("MustBeAbsoluteUri(");
        sourceCode.Should().Contain("public static void MustBeAbsoluteUri");
        sourceCode.Should().NotContain("public static void MustBeRelativeUri");
        sourceCode.Should().NotContain("public static void UriMustHaveScheme");
        sourceCode.Should().Contain("class RelativeUriException : UriException");
        sourceCode.Should().Contain("class UriException : ArgumentException");
        sourceCode.Should().NotContain("class AbsoluteUriException : UriException");
    }

    [Fact]
    public static void WhitelistModeRetainsSupportClosures()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "SupportClosures.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions:
                    [
                        new ("MustBeIn", false),
                        new ("MustBeValidEnumValue", false),
                        new ("MustBeEmailAddress", false),
                        new ("MustHaveLength", true),
                    ]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("struct Range<T>");
        sourceCode.Should().Contain("class EnumInfo<T>");
        sourceCode.Should().Contain("class Types");
        sourceCode.Should().Contain("class RegularExpressions");
        sourceCode.Should().Contain("delegate Exception SpanExceptionFactory<TItem, in T>");
        sourceCode.Should().Contain("delegate Exception ReadOnlySpanExceptionFactory<TItem, in T>");
        sourceCode.Should().NotContain("GeneratedRegex");
    }

    [Fact]
    public static void PerAssertionExceptionFactoryTrimmingOnlyRemovesSelectedAssertionOverloads()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "ExceptionFactoryTrimming.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions:
                    [
                        new ("MustHaveLength", false),
                        new ("MustBeGreaterThan", true),
                    ]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().NotContain("Func<string?, int, Exception> exceptionFactory");
        sourceCode.Should().NotContain("SpanExceptionFactory<T, int> exceptionFactory");
        sourceCode.Should().Contain("Func<T, T, Exception> exceptionFactory");
        sourceCode.Should().Contain("public static void CustomException<T1, T2>");
    }

    [Fact]
    public static void AdditionalAssertionsRetainPortableSupportClosures()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "AdditionalPortableAssertions.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions:
                    [
                        new ("IsUuidVersion7", false),
                        new ("MustBeUuidVersion7", true),
                        new ("MustHaveCountIn", true),
                        new ("MustBeAscii", true),
                        new ("MustNotBeEmptyOrWhiteSpace", true),
                    ]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("struct GuidLayout");
        sourceCode.Should().Contain("StructLayout(LayoutKind.Explicit");
        sourceCode.Should().Contain("struct Range<T>");
        sourceCode.Should().Contain("class EnumerableExtensions");
        sourceCode.Should().Contain("delegate Exception ReadOnlySpanExceptionFactory<TItem>");
        sourceCode.Should().Contain("MustBeAscii(");
        sourceCode.Should().Contain("MustNotBeEmptyOrWhiteSpace(");
    }

    [Fact]
    public static void MustHaveSameCountAsWhitelistRetainsCountAndExceptionClosuresOnBothTargets()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var portableFile = Path.Combine(temporaryDirectory.DirectoryPath, "SameCountPortable.cs");
        var modernFile = Path.Combine(temporaryDirectory.DirectoryPath, "SameCountModern.cs");
        var whitelist = CreateWhitelist(
            includedAssertions: [new ("MustHaveSameCountAs", true)]
        );

        SourceFileMerger.CreateSingleSourceFile(CreateOptions(portableFile, whitelist));
        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(modernFile, whitelist, SourceTargetFramework.Net10_0)
        );

        foreach (var sourceCode in new[] { File.ReadAllText(portableFile), File.ReadAllText(modernFile) })
        {
            sourceCode.Should().Contain("MustHaveSameCountAs<TCollection, TOtherCollection>(");
            sourceCode.Should().Contain("public static int Count(");
            sourceCode.Should().Contain("public static void CollectionCountsNotEqual(");
            sourceCode.Should().Contain("class InvalidCollectionCountException : CollectionException");
            sourceCode.Should()
                      .Contain("Func<TCollection?, TOtherCollection?, Exception> exceptionFactory");
            sourceCode.Should().Contain("public static void CustomException<T1, T2>(");
            sourceCode.Should().NotContain("MustHaveCountIn<TCollection>(");
        }
    }

    [Fact]
    public static void MustHaveSameCountAsWhitelistTrimsItsExceptionFactoryOverload()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "SameCountWithoutFactory.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(includedAssertions: [new ("MustHaveSameCountAs", false)])
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("MustHaveSameCountAs<TCollection, TOtherCollection>(");
        sourceCode.Should().Contain("public static int Count(");
        sourceCode.Should().Contain("public static void CollectionCountsNotEqual(");
        sourceCode.Should().Contain("class InvalidCollectionCountException : CollectionException");
        sourceCode.Should().NotContain("Func<TCollection?, TOtherCollection?, Exception> exceptionFactory");
    }

    [Fact]
    public static void FiniteWhitelistUsesTargetSpecificSurface()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var portableFile = Path.Combine(temporaryDirectory.DirectoryPath, "FinitePortable.cs");
        var modernFile = Path.Combine(temporaryDirectory.DirectoryPath, "FiniteModern.cs");
        var whitelist = CreateWhitelist(
            includedAssertions:
            [
                new ("IsFinite", true),
                new ("MustBeFinite", true),
            ]
        );

        SourceFileMerger.CreateSingleSourceFile(CreateOptions(portableFile, whitelist));
        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(modernFile, whitelist, SourceTargetFramework.Net10_0)
        );

        File.ReadAllText(portableFile).Should().NotContain("IFloatingPointIeee754<T>");
        File.ReadAllText(modernFile).Should().Contain("IFloatingPointIeee754<T>");
    }

    [Fact]
    public static void SignGuardWhitelistsUseTargetSpecificSurface()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var portableFile = Path.Combine(temporaryDirectory.DirectoryPath, "SignGuardsPortable.cs");
        var modernFile = Path.Combine(temporaryDirectory.DirectoryPath, "SignGuardsModern.cs");
        var portableWithoutFactoriesFile =
            Path.Combine(temporaryDirectory.DirectoryPath, "MustBePositivePortableWithoutFactories.cs");
        var modernWithoutFactoriesFile =
            Path.Combine(temporaryDirectory.DirectoryPath, "MustBePositiveModernWithoutFactories.cs");
        var whitelist = CreateWhitelist(
            includedAssertions:
            [
                new ("MustBePositive", true),
                new ("MustBeNegative", true),
                new ("MustNotBePositive", true),
                new ("MustNotBeNegative", true),
                new ("MustNotBeZero", false),
            ]
        );

        SourceFileMerger.CreateSingleSourceFile(CreateOptions(portableFile, whitelist));
        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(modernFile, whitelist, SourceTargetFramework.Net10_0)
        );
        var withoutFactoriesWhitelist = CreateWhitelist(
            includedAssertions: [new ("MustBePositive", false)]
        );
        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(portableWithoutFactoriesFile, withoutFactoriesWhitelist)
        );
        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                modernWithoutFactoriesFile,
                withoutFactoriesWhitelist,
                SourceTargetFramework.Net10_0
            )
        );
        var portableCode = File.ReadAllText(portableFile);
        var modernCode = File.ReadAllText(modernFile);
        var portableWithoutFactoriesCode = File.ReadAllText(portableWithoutFactoriesFile);
        var modernWithoutFactoriesCode = File.ReadAllText(modernWithoutFactoriesFile);
        var concreteTypesByAssertion = new Dictionary<string, string[]>
        {
            ["MustBePositive"] =
            [
                "sbyte", "byte", "short", "ushort", "int", "uint",
                "long", "ulong", "decimal", "float", "double", "TimeSpan",
            ],
            ["MustBeNegative"] =
            [
                "sbyte", "short", "int", "long", "decimal", "float", "double", "TimeSpan",
            ],
            ["MustNotBePositive"] =
            [
                "sbyte", "byte", "short", "ushort", "int", "uint",
                "long", "ulong", "decimal", "float", "double", "TimeSpan",
            ],
            ["MustNotBeNegative"] =
            [
                "sbyte", "short", "int", "long", "decimal", "float", "double", "TimeSpan",
            ],
            ["MustNotBeZero"] =
            [
                "sbyte", "byte", "short", "ushort", "int", "uint",
                "long", "ulong", "decimal", "float", "double", "TimeSpan",
            ],
        };
        string[] omittedUnsignedTypes = ["byte", "ushort", "uint", "ulong"];

        foreach (var (assertion, concreteTypes) in concreteTypesByAssertion)
        {
            foreach (var type in concreteTypes)
            {
                portableCode.Should().Contain($"public static {type} {assertion}(");
                modernCode.Should().Contain($"public static {type} {assertion}(");
                if (assertion == "MustNotBeZero")
                {
                    portableCode.Should()
                                .NotContain(
                                     $"{assertion}(this {type} parameter, Func<{type}, Exception> exceptionFactory)"
                                 );
                    modernCode.Should()
                              .NotContain(
                                   $"{assertion}(this {type} parameter, Func<{type}, Exception> exceptionFactory)"
                               );
                }
                else
                {
                    portableCode.Should()
                                .Contain(
                                     $"{assertion}(this {type} parameter, Func<{type}, Exception> exceptionFactory)"
                                 );
                    modernCode.Should()
                              .Contain(
                                   $"{assertion}(this {type} parameter, Func<{type}, Exception> exceptionFactory)"
                               );
                }
            }
        }

        foreach (var type in omittedUnsignedTypes)
        {
            portableCode.Should().NotContain($"public static {type} MustBeNegative(");
            modernCode.Should().NotContain($"public static {type} MustBeNegative(");
            portableCode.Should().NotContain($"public static {type} MustNotBeNegative(");
            modernCode.Should().NotContain($"public static {type} MustNotBeNegative(");
        }

        foreach (var type in concreteTypesByAssertion["MustBePositive"])
        {
            portableWithoutFactoriesCode.Should().Contain($"public static {type} MustBePositive(");
            modernWithoutFactoriesCode.Should().Contain($"public static {type} MustBePositive(");
            portableWithoutFactoriesCode.Should()
                                        .NotContain(
                                             $"MustBePositive(this {type} parameter, Func<{type}, Exception> exceptionFactory)"
                                         );
            modernWithoutFactoriesCode.Should()
                                      .NotContain(
                                           $"MustBePositive(this {type} parameter, Func<{type}, Exception> exceptionFactory)"
                                       );
        }

        portableCode.Should().NotContain("public static T MustBePositive<T>(");
        portableCode.Should().NotContain("public static T MustBeNegative<T>(");
        portableCode.Should().NotContain("public static T MustNotBePositive<T>(");
        portableCode.Should().NotContain("public static T MustNotBeNegative<T>(");
        portableCode.Should().NotContain("public static T MustNotBeZero<T>(");
        portableCode.Should().NotContain("INumber<T>");
        modernCode.Should().Contain("MustBePositive<T>");
        modernCode.Should().Contain("MustBeNegative<T>");
        modernCode.Should().Contain("MustNotBePositive<T>");
        modernCode.Should().Contain("MustNotBeNegative<T>");
        modernCode.Should().Contain("MustNotBeZero<T>");
        modernCode.Should().Contain("INumber<T>");
        modernCode.Should()
                  .NotContain("MustNotBeZero<T>(this T parameter, Func<T, Exception> exceptionFactory)");
        portableWithoutFactoriesCode.Should().NotContain("public static T MustBePositive<T>(");
        modernWithoutFactoriesCode.Should().Contain("public static T MustBePositive<T>(");
        modernWithoutFactoriesCode.Should()
                                  .NotContain(
                                       "MustBePositive<T>(this T parameter, Func<T, Exception> exceptionFactory)"
                                   );
    }

    [Fact]
    public static void MustContainKeyWhitelistExportsGuardWithExceptionAndThrowHelper()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustContainKey.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions: [new ("MustContainKey", true)]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("MustContainKey<TKey, TValue>(");
        sourceCode.Should().Contain("this IReadOnlyDictionary<TKey, TValue>? parameter");
        sourceCode.Should().Contain("this Dictionary<TKey, TValue>? parameter");
        sourceCode.Should().Contain("class MissingKeyException : CollectionException");
        sourceCode.Should().Contain("public static void MissingKey<TKey, TValue>(");
        sourceCode.Should()
                  .Contain("Func<IReadOnlyDictionary<TKey, TValue>?, TKey, Exception> exceptionFactory");
        sourceCode.Should().Contain("Func<Dictionary<TKey, TValue>?, TKey, Exception> exceptionFactory");
        sourceCode.Should().NotContain("MustNotContainKey");
        sourceCode.Should().NotContain("class ExistingKeyException");
    }

    [Fact]
    public static void MustNotContainKeyWhitelistExportsGuardAndTrimsExceptionFactoryOverloads()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustNotContainKey.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions: [new ("MustNotContainKey", false)]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("MustNotContainKey<TKey, TValue>(");
        sourceCode.Should().Contain("this IReadOnlyDictionary<TKey, TValue>? parameter");
        sourceCode.Should().Contain("this Dictionary<TKey, TValue>? parameter");
        sourceCode.Should().Contain("class ExistingKeyException : CollectionException");
        sourceCode.Should().Contain("public static void ExistingKey<TKey, TValue>(");
        sourceCode.Should().NotContain("TKey, Exception> exceptionFactory");
        sourceCode.Should().NotContain("MustContainKey<TKey, TValue>(");
        sourceCode.Should().NotContain("class MissingKeyException");
    }

    [Fact]
    public static void MustNotContainNullWhitelistExportsDependenciesAndTrimsExceptionFactories()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustNotContainNull.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions: [new ("MustNotContainNull", false)]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("MustNotContainNull<TCollection>(");
        sourceCode.Should().Contain("where TCollection : class, IEnumerable");
        sourceCode.Should().Contain("ImmutableArray<T> MustNotContainNull<T>(");
        sourceCode.Should().Contain("class ExistingItemException : CollectionException");
        sourceCode.Should().Contain("public static void NullItem(");
        sourceCode.Should().NotContain("Func<TCollection?, Exception> exceptionFactory");
        sourceCode.Should().NotContain("Func<ImmutableArray<T>, Exception> exceptionFactory");
        sourceCode.Should().NotContain("MustNotContainNullOrWhiteSpace");
    }

    [Fact]
    public static void MustNotContainNullOrWhiteSpaceWhitelistExportsDependenciesAndTrimsExceptionFactories()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustNotContainNullOrWhiteSpace.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions: [new ("MustNotContainNullOrWhiteSpace", false)]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("MustNotContainNullOrWhiteSpace<TCollection>(");
        sourceCode.Should().Contain("where TCollection : class, IEnumerable<string?>");
        sourceCode.Should().Contain("ImmutableArray<string?> MustNotContainNullOrWhiteSpace(");
        sourceCode.Should().Contain("class ExistingItemException : CollectionException");
        sourceCode.Should().Contain("public static void NullOrWhiteSpaceItem(");
        sourceCode.Should().Contain("public static bool IsNullOrWhiteSpace(");
        sourceCode.Should().NotContain("Func<TCollection?, Exception> exceptionFactory");
        sourceCode.Should().NotContain("Func<ImmutableArray<string?>, Exception> exceptionFactory");
        sourceCode.Should().NotContain("MustNotContainNull<TCollection>");
    }

    [Fact]
    public static void StringInspectionWhitelistsRetainPredicatesHelpersAndPortableBase64()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "StringInspection.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions:
                    [
                        new ("MustContainOnlyDigits", true),
                        new ("MustContainOnlyLettersOrDigits", true),
                        new ("MustBeUpperCase", true),
                        new ("MustBeLowerCase", true),
                        new ("MustBeBase64", true),
                        new ("MustBeHexadecimal", true),
                        new ("MustNotContainWhiteSpace", true),
                    ]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("ContainsOnlyDigits(this ReadOnlySpan<char> parameter)");
        sourceCode.Should().Contain("ContainsOnlyLettersOrDigits(this ReadOnlySpan<char> parameter)");
        sourceCode.Should().Contain("IsUpperCase(this ReadOnlySpan<char> parameter)");
        sourceCode.Should().Contain("IsLowerCase(this ReadOnlySpan<char> parameter)");
        sourceCode.Should().Contain("IsBase64Portable(ReadOnlySpan<char> parameter)");
        sourceCode.Should().Contain("IsHexadecimal(this ReadOnlySpan<char> parameter)");
        sourceCode.Should().Contain("ContainsWhiteSpace(ReadOnlySpan<char> parameter)");
        sourceCode.Should().Contain("public static void InvalidStringContent(");
        sourceCode.Should().Contain("class StringException : ArgumentException");
        sourceCode.Should().Contain("delegate Exception ReadOnlySpanExceptionFactory<TItem>");
        sourceCode.Should().NotContain("Base64.IsValid(parameter)");
        sourceCode.Should().NotContain("MustBeAscii(");
    }

    [Fact]
    public static void StringInspectionWhitelistTrimsExceptionFactories()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "StringInspectionWithoutFactories.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions: [new ("MustBeBase64", false)]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("MustBeBase64(");
        sourceCode.Should().Contain("IsBase64Portable(ReadOnlySpan<char> parameter)");
        sourceCode.Should().Contain("public static void InvalidStringContent(");
        sourceCode.Should().NotContain("Func<string?, Exception> exceptionFactory");
        sourceCode.Should().NotContain("ReadOnlySpanExceptionFactory<char> exceptionFactory");
        sourceCode.Should().NotContain("public static void CustomSpanException");
    }

    [Fact]
    public static void StreamGuardWhitelistsRetainTheirGuardsThrowHelpersAndExceptionFactories()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "StreamGuards.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions:
                    [
                        new ("MustBeReadable", true),
                        new ("MustBeWritable", true),
                        new ("MustBeSeekable", true),
                    ]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("TStream MustBeReadable<TStream>(");
        sourceCode.Should().Contain("TStream MustBeWritable<TStream>(");
        sourceCode.Should().Contain("TStream MustBeSeekable<TStream>(");
        sourceCode.Should().Contain("public static void MustBeReadable(");
        sourceCode.Should().Contain("public static void MustBeWritable(");
        sourceCode.Should().Contain("public static void MustBeSeekable(");
        sourceCode.Should().Contain("Func<TStream?, Exception> exceptionFactory");
        sourceCode.Should().Contain("public static void CustomException<T>(");
        sourceCode.Should().NotContain("MustBeAbsoluteUri(");
    }

    [Fact]
    public static void StreamGuardWhitelistTrimsFactoryAndUnrelatedStreamThrowHelpers()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "ReadableStreamGuard.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(includedAssertions: [new ("MustBeReadable", false)])
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("TStream MustBeReadable<TStream>(");
        sourceCode.Should().Contain("public static void MustBeReadable(");
        sourceCode.Should().NotContain("Func<TStream?, Exception> exceptionFactory");
        sourceCode.Should().NotContain("public static void MustBeWritable(");
        sourceCode.Should().NotContain("public static void MustBeSeekable(");
    }

    [Fact]
    public static void ObjectDisposedWhitelistExportsGuardThrowHelperAndExceptionFactories()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "ObjectDisposed.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(
                    includedAssertions: [new ("ObjectDisposed", true)]
                )
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain(
            "public static void ObjectDisposed(bool condition, string? objectName = null, string? message = null)"
        );
        sourceCode.Should().Contain(
            "public static void ObjectDisposed(bool condition, Func<Exception> exceptionFactory)"
        );
        sourceCode.Should().Contain(
            "public static void ObjectDisposed<T>(bool condition, T parameter, Func<T, Exception> exceptionFactory)"
        );
        sourceCode.Should().Contain(
            "public static void ObjectDisposed(string? objectName = null, string? message = null) => throw new ObjectDisposedException(objectName, message);"
        );
        sourceCode.Should().Contain("public static void CustomException(Func<Exception> exceptionFactory)");
        sourceCode.Should().NotContain("public static void InvalidOperation(");
    }

    [Fact]
    public static void MustBeAssignableToWhitelistExportsGuardThrowHelperAndFactoryOnBothTargets()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var portableFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustBeAssignableToPortable.cs");
        var modernFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustBeAssignableToModern.cs");
        var whitelist = CreateWhitelist(
            includedAssertions: [new ("MustBeAssignableTo", true)]
        );

        SourceFileMerger.CreateSingleSourceFile(CreateOptions(portableFile, whitelist));
        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(modernFile, whitelist, SourceTargetFramework.Net10_0)
        );

        foreach (var sourceCode in new[] { File.ReadAllText(portableFile), File.ReadAllText(modernFile) })
        {
            sourceCode.Should().Contain("public static Type MustBeAssignableTo(");
            sourceCode.Should().Contain("public static void MustBeAssignableTo(");
            sourceCode.Should().Contain("Func<Type?, Type?, Exception> exceptionFactory");
            sourceCode.Should().Contain("public static void CustomException<T1, T2>(");
            sourceCode.Should().NotContain("public static bool IsEquivalentTypeTo(");
        }
    }

    [Fact]
    public static void MustBeAssignableToWhitelistTrimsExceptionFactoryOverload()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustBeAssignableToWithoutFactory.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(includedAssertions: [new ("MustBeAssignableTo", false)])
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("public static Type MustBeAssignableTo(");
        sourceCode.Should().Contain("public static void MustBeAssignableTo(");
        sourceCode.Should().NotContain("Func<Type?, Type?, Exception> exceptionFactory");
        sourceCode.Should().NotContain("public static void CustomException<T1, T2>(");
    }

    [Fact]
    public static void MustBeConcreteClassWhitelistExportsGuardThrowHelperAndFactoryOnBothTargets()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var portableFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustBeConcreteClassPortable.cs");
        var modernFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustBeConcreteClassModern.cs");
        var whitelist = CreateWhitelist(
            includedAssertions: [new ("MustBeConcreteClass", true)]
        );

        SourceFileMerger.CreateSingleSourceFile(CreateOptions(portableFile, whitelist));
        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(modernFile, whitelist, SourceTargetFramework.Net10_0)
        );

        foreach (var sourceCode in new[] { File.ReadAllText(portableFile), File.ReadAllText(modernFile) })
        {
            sourceCode.Should().Contain("public static Type MustBeConcreteClass(");
            sourceCode.Should().Contain("public static void MustBeConcreteClass(");
            sourceCode.Should().Contain("Func<Type?, Exception> exceptionFactory");
            sourceCode.Should().Contain("public static void CustomException<T>(");
            sourceCode.Should().NotContain("public static Type MustBeAssignableTo(");
        }
    }

    [Fact]
    public static void MustBeConcreteClassWhitelistTrimsExceptionFactoryOverload()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustBeConcreteClassWithoutFactory.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(includedAssertions: [new ("MustBeConcreteClass", false)])
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("public static Type MustBeConcreteClass(");
        sourceCode.Should().Contain("public static void MustBeConcreteClass(");
        sourceCode.Should().NotContain("Func<Type?, Exception> exceptionFactory");
        sourceCode.Should().NotContain("public static void CustomException<T>(");
    }

    [Fact]
    public static void MustBeUriWhitelistExportsGuardThrowHelperExceptionAndFactories()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustBeUri.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(includedAssertions: [new ("MustBeUri", true)])
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("public static string MustBeUri(");
        sourceCode.Should().Contain("public static void MustBeUri(");
        sourceCode.Should().Contain("class InvalidUriException : UriException");
        sourceCode.Should().Contain("class UriException : ArgumentException");
        sourceCode.Should().Contain("Func<string?, Exception> exceptionFactory");
        sourceCode.Should().Contain("Func<string?, UriKind, Exception> exceptionFactory");
        sourceCode.Should().Contain("public static void CustomException<T>(");
        sourceCode.Should().Contain("public static void CustomException<T1, T2>(");
        sourceCode.Should().NotContain("public static void MustBeAbsoluteUri(");
        sourceCode.Should().NotContain("class RelativeUriException : UriException");
    }

    [Fact]
    public static void MustBeUriWhitelistTrimsExceptionFactoryOverloads()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "MustBeUriWithoutFactories.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(includedAssertions: [new ("MustBeUri", false)])
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain("public static string MustBeUri(");
        sourceCode.Should().Contain("public static void MustBeUri(");
        sourceCode.Should().Contain("class InvalidUriException : UriException");
        sourceCode.Should().NotContain("Func<string?, Exception> exceptionFactory");
        sourceCode.Should().NotContain("Func<string?, UriKind, Exception> exceptionFactory");
        sourceCode.Should().NotContain("public static void CustomException<T>(");
        sourceCode.Should().NotContain("public static void CustomException<T1, T2>(");
    }

    [Fact]
    public static void ObjectDisposedWhitelistTrimsExceptionFactoryOverloads()
    {
        using var temporaryDirectory = new TemporaryDirectory();
        var targetFile = Path.Combine(temporaryDirectory.DirectoryPath, "ObjectDisposedWithoutFactories.cs");

        SourceFileMerger.CreateSingleSourceFile(
            CreateOptions(
                targetFile,
                CreateWhitelist(includedAssertions: [new ("ObjectDisposed", false)])
            )
        );
        var sourceCode = File.ReadAllText(targetFile);

        sourceCode.Should().Contain(
            "public static void ObjectDisposed(bool condition, string? objectName = null, string? message = null)"
        );
        sourceCode.Should().Contain(
            "public static void ObjectDisposed(string? objectName = null, string? message = null) => throw new ObjectDisposedException(objectName, message);"
        );
        sourceCode.Should().NotContain("Func<Exception> exceptionFactory");
        sourceCode.Should().NotContain("ObjectDisposed<T>");
        sourceCode.Should().NotContain("public static void CustomException(");
    }

    private static SourceFileMergeOptions CreateOptions(
        string targetFile,
        AssertionWhitelist assertionWhitelist = null,
        SourceTargetFramework targetFramework = SourceTargetFramework.NetStandard2_0
    ) =>
        new ()
        {
            SourceFolder = SourceDirectory.FullName,
            TargetFile = targetFile,
            TargetFramework = targetFramework,
            IncludeVersionComment = false,
            AssertionWhitelist = assertionWhitelist ?? new AssertionWhitelist(),
        };

    private static AssertionWhitelist CreateWhitelist(
        bool isEnabled = true,
        params AssertionSelection[] includedAssertions
    )
    {
        var whitelist = new AssertionWhitelist { IsEnabled = isEnabled };
        var selectedAssertions = includedAssertions.ToDictionary(
            selection => selection.AssertionName,
            StringComparer.Ordinal
        );

        foreach (var property in typeof(AssertionWhitelist).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                                           .Where(
                                                                property => property.PropertyType ==
                                                                            typeof(AssertionWhitelist.AssertionEntry)
                                                            ))
        {
            var entry = selectedAssertions.TryGetValue(property.Name, out var selection) ?
                new AssertionWhitelist.AssertionEntry
                {
                    Include = true,
                    IncludeExceptionFactoryOverload = selection.IncludeExceptionFactoryOverload,
                } :
                new AssertionWhitelist.AssertionEntry { Include = false };
            property.SetValue(whitelist, entry);
        }

        return whitelist;
    }

    private readonly record struct AssertionSelection(
        string AssertionName,
        bool IncludeExceptionFactoryOverload
    );
}
