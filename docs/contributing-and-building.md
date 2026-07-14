# Contributing and building

[Documentation index](README.md) · **Contributing and building** · [Structuring checks](structuring-precondition-checks.md) · [Source inclusion](source-code-inclusion.md) · [Assertions](assertion-overview.md) · [Historical performance](historical-performance.md) · [Background](guard-clause-background.md)

Start with the contributor entry point in [`CONTRIBUTING.md`](../CONTRIBUTING.md). This page covers repository setup and implementation conventions.

## Prerequisites and build

The repository pins the .NET SDK in [`global.json`](../global.json). Install that .NET 10 SDK feature band or a compatible patch selected by its roll-forward policy. C# 14 is configured centrally.

From the repository root, use the single solution entry point:

```shell
dotnet restore Light.GuardClauses.slnx
dotnet build Light.GuardClauses.slnx -c Release --no-restore
dotnet test --solution Light.GuardClauses.slnx -c Release --no-build --no-restore
```

Tests run on Microsoft Testing Platform. To collect Cobertura coverage locally, add
`--results-directory artifacts/test-results --coverage --coverage-output-format cobertura` to the test command.

The product package targets .NET Standard 2.0, .NET Standard 2.1, and .NET 10. Tests, tools, and benchmarks run on .NET 10. BenchmarkDotNet memory diagnostics are enabled by default; pass `--enable-disassembly` after the benchmark project's `--` separator to opt into platform-dependent disassembly diagnostics.

## Repository layout

| Path | Purpose |
| --- | --- |
| [`src/Light.GuardClauses`](../src/Light.GuardClauses/) | Library source, split primarily into `Check.<Assertion>.cs` and supporting `Throw`, exception, attribute, and helper files |
| [`tests/Light.GuardClauses.Tests`](../tests/Light.GuardClauses.Tests/) | Functional tests for the public library |
| [`tests/Light.GuardClauses.SourceCodeTransformation.Tests`](../tests/Light.GuardClauses.SourceCodeTransformation.Tests/) | Source-export tests |
| [`tests/Light.GuardClauses.InternalRoslynAnalyzers.Tests`](../tests/Light.GuardClauses.InternalRoslynAnalyzers.Tests/) | Tests for the repository's XML-documentation analyzers |
| [`benchmarks/Light.GuardClauses.Performance`](../benchmarks/Light.GuardClauses.Performance/) | BenchmarkDotNet benchmarks |
| [`tools/analyzers`](../tools/analyzers/) | Analyzer and code-fix projects enforcing documentation conventions |
| [`tools/source-export`](../tools/source-export/) | Single-file exporter and its multi-target validation project |

The [`.slnx`](../Light.GuardClauses.slnx) contains all production, test, benchmark, analyzer, and source-export projects.

## Running tests on other frameworks

[`Light.GuardClauses.Tests.csproj`](../tests/Light.GuardClauses.Tests/Light.GuardClauses.Tests.csproj) imports an optional `TargetFrameworks.props` from the same directory. The file is ignored by Git. For example:

```xml
<Project>
  <PropertyGroup>
    <TargetFrameworks>net10.0;net48</TargetFrameworks>
  </PropertyGroup>
</Project>
```

Only select frameworks for which the machine has the required runtime, targeting pack, and test support. Do not commit this local override.

## Adding or changing an assertion

- Keep public assertions in the partial `Check` class and follow the existing file-per-assertion naming convention.
- Throw through the appropriate `Throw` helper so exceptional paths remain out of the hot assertion body.
- A throwing guard normally has a default-exception overload with optional `parameterName` and `message`, plus an exception-factory overload. Preserve the same failure semantics in both.
- Apply `[CallerArgumentExpression("parameter")]` and nullability/code-analysis annotations where the surrounding API pattern calls for them.
- Keep XML documentation complete. Repository analyzers enforce the standard `parameterName` and `message` wording.
- Add functional tests for success and every failure path. Add BenchmarkDotNet coverage when performance is a risk or requirement; measure before introducing performance-specific complexity.
- If the assertion is exportable, add its matching entry to the source exporter's [whitelist catalog](../tools/source-export/Light.GuardClauses.SourceCodeTransformation/AssertionWhitelist.cs) and committed [settings](../tools/source-export/Light.GuardClauses.SourceCodeTransformation/settings.json). Generation intentionally fails when a `Check.<Name>.cs` file lacks an entry.

Run the complete solution after a change. If the single-file distribution or exporter behavior is affected, also follow the validation workflow in the [source inclusion guide](source-code-inclusion.md).
