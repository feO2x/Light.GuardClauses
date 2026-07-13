# Upgrade the Repository to .NET 10

## Rationale

The repository is pinned to the .NET 8 SDK and C# 12, its runnable projects and automation use .NET 8, and the source-export pipeline models .NET 8 as its modern target. Upgrade the development and execution baseline to the .NET 10 LTS toolchain, replace the `net8.0` product asset with `net10.0`, and retain the existing .NET Standard compatibility floor.

The upgrade must also keep the compiler-loaded analyzer safe after the RS1038 assembly split, align Roslyn with the pinned SDK, and accommodate .NET 10 package pruning without suppressing warnings or changing guard-clause behavior.

## Acceptance Criteria

- [ ] `global.json` selects the .NET SDK 10.0.300 feature band with `latestPatch`, and local builds, CI, and release packaging use that SDK selection as their single source of truth.
- [ ] The repository uses C# 14; runnable tools and test projects target `net10.0`, benchmarks target `net10.0` and `net48`, and the product package contains exactly `netstandard2.0`, `netstandard2.1`, and `net10.0` assets with no `net8.0` asset.
- [ ] The `net10.0` product asset is Native AOT-compatible and exposes the existing modern API surface without behavioral or source-level regressions, including the span, memory, and generic-math overloads.
- [ ] The analyzer, code-fix, and source-export projects use Roslyn 5.6.0 consistently; Release builds produce no RS1038, CS8032, or new Roslyn analyzer warnings, and analyzer and code-fix behavior remains covered by automated tests.
- [ ] Source export replaces `Net8_0` with `Net10_0`, emits and validates the correct .NET 10 source, and preserves `NetStandard2_0` as the default for the committed single-file distribution.
- [ ] Restore and build produce no NU1510 package-pruning warning, and the packed NuGet dependency groups contain only dependencies required by each target framework.
- [ ] User-facing framework documentation describes the resulting .NET 10 and .NET Standard support accurately and contains no stale .NET 8 SDK, target-framework, or support claims.
- [ ] The complete solution restores and builds without warnings in Release configuration on the pinned SDK, all automated tests pass, and a signed-equivalent package verification confirms the expected assemblies, symbols, Source Link data, documentation, icon, and target-framework dependency groups.

## Technical Details

Use this target matrix:

| Component | Target frameworks |
| --- | --- |
| `Light.GuardClauses` | `netstandard2.0;netstandard2.1;net10.0` |
| Main, analyzer, and source-export test projects | `net10.0` by default |
| Source-export tool | `net10.0` |
| Source-validation harness | `netstandard2.0;net10.0` |
| Benchmarks | `net10.0;net48` |
| Analyzer and code-fix assemblies | `netstandard2.0` |

Retain the main test project's optional `TargetFrameworks.props` override and enable `IsAotCompatible` for the `net10.0` product asset. Existing `NET8_0_OR_GREATER` branches express the API capability boundary and are defined when targeting .NET 10; retain them rather than performing a mechanical rename to .NET 10 guards. Compile the repository with an explicit `<LangVersion>14</LangVersion>`. C# 14 changes overload resolution involving `Span<T>` conversions, so the existing string, span, memory, and generic-math tests must compile and pass without changing the public overload set unless a genuine ambiguity is found.

Pin `Microsoft.CodeAnalysis.Analyzers`, `Microsoft.CodeAnalysis.CSharp`, and `Microsoft.CodeAnalysis.CSharp.Workspaces` to 5.6.0, matching the compiler line in the .NET SDK 10.0.300 feature band. Remove the code-fix project's 4.11.0 `VersionOverride` and update the central explanatory comment, while preserving the compiler-only dependency boundary of `Light.GuardClauses.InternalRoslynAnalyzers` and the Workspaces dependency in `Light.GuardClauses.InternalRoslynAnalyzers.CodeFixes`. The source-export tool and its tests use the same Workspaces version, so syntax parsing, semantic analysis, analyzers, and code fixes all share one Roslyn line.

Account for .NET 10's framework-package pruning rather than disabling it. `System.Collections.Immutable` remains a package dependency only where it is required for the .NET Standard product and validation assets; `net10.0` uses the framework assembly, and the source-export tool does not carry a redundant direct reference. Update the centrally managed `Microsoft.Extensions.Configuration.*` and `System.Collections.Immutable` packages to 10.0.9. Remove the explicit `Microsoft.SourceLink.GitHub` package because Source Link for GitHub is included and enabled by the .NET 10 SDK; retain the existing repository, symbol-package, and untracked-source properties and verify their packed output.

Replace `SourceTargetFramework.Net8_0` with `Net10_0`. The parse-options factory uses C# 14 and defines the symbols required by the current source to select its .NET 10 branches: `NET`, `NETCOREAPP`, `NET10_0`, `NET10_0_OR_GREATER`, `NET9_0_OR_GREATER`, and `NET8_0_OR_GREATER`, but not the exact-version symbol `NET8_0`. Update modern-only imports, polyfill suppression, the validation project, validator mapping, configuration, and tests for the replacement target. Cover .NET 10 flattening, conditional branch selection, polyfill omission, and build validation. The committed configuration and `Light.GuardClauses.SingleFile.cs` remain the portable `NetStandard2_0` output.

Update both GitHub Actions workflows to install the SDK through `global-json-file: global.json`. Remove the release workflow's independently configurable SDK-version input so an older compiler cannot load analyzers built against Roslyn 5.6.0. Preserve the existing restore/build/test and signed package flow otherwise.
