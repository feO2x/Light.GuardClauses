# Resolve RS1038 by Splitting the Internal Roslyn Analyzers Assembly

## Rationale

Building the solution produces warning RS1038 because `Light.GuardClauses.InternalRoslynAnalyzers` contains a `DiagnosticAnalyzer` — an extension loaded by the compiler during every command-line build of the product — while also referencing `Microsoft.CodeAnalysis.CSharp.Workspaces`, which the compiler does not provide. The build only works today because the analyzer's executed code paths never touch a Workspaces type; any future change that makes one reachable would cause the analyzer to fail loading (CS8032), silently disabling it and breaking Release builds via `TreatWarningsAsErrors`.

Split the assembly following the standard Roslyn pattern: the compiler-loaded analyzer must reference only compiler-layer Roslyn assemblies, while the IDE-only code fix providers move to a separate assembly that may reference Workspaces.

## Acceptance Criteria

- [ ] Building the complete solution in Release configuration produces no RS1038 warning and no new warnings.
- [ ] The `Light.GuardClauses.InternalRoslynAnalyzers` assembly references only compiler-layer Roslyn packages and contains no code fix providers; the two code fix providers reside in a new `Light.GuardClauses.InternalRoslynAnalyzers.CodeFixes` assembly.
- [ ] The product project receives both assemblies as analyzer references, so command-line builds still run the XML-comment analyzer and IDEs can still load the code fixes.
- [ ] Introducing a non-default `parameterName` or `message` XML comment on a standard exception assertion in the product project still produces the corresponding `LightGuardClauses_*` diagnostic during a command-line build.
- [ ] The existing analyzer and code-fix tests pass against the split assemblies, and the complete solution's tests all pass.
- [ ] `Directory.Packages.props` contains no project-name-conditional version override; the Roslyn version constraint for compiler-loaded assemblies is documented where the version is pinned.
- [ ] The new project appears in `Light.GuardClauses.sln` under the `tools/analyzers` solution folder hierarchy.

## Technical Details

Create `tools/analyzers/Light.GuardClauses.InternalRoslynAnalyzers.CodeFixes/` and move `MessageXmlCommentFix.cs` and `ParameterNameXmlCommentFix.cs` into it, keeping the existing `Light.GuardClauses.InternalRoslynAnalyzers` namespace to avoid churn. The shared types (`Descriptors`, `ParameterNameConstants`, `MessageConstants`) are already public and stay in the analyzer project; the CodeFixes project references it via a regular `ProjectReference`. Both projects target `netstandard2.0` and reference `Microsoft.CodeAnalysis.Analyzers` with `PrivateAssets="all"`; only the analyzer project keeps `EnforceExtendedAnalyzerRules`.

The analyzer project replaces its `Microsoft.CodeAnalysis.CSharp.Workspaces` reference with `Microsoft.CodeAnalysis.CSharp`; the CodeFixes project references `Microsoft.CodeAnalysis.CSharp.Workspaces`. Both must stay at 4.11.0: a compiler-loaded extension must not be built against a newer Microsoft.CodeAnalysis than the Roslyn version shipped with the pinned .NET SDK 8.0.400 (Roslyn 4.11), and the code fixes must match the analyzer's Roslyn version. Express this via a central `Microsoft.CodeAnalysis.CSharp` entry at 4.11.0 (the analyzer is its only consumer) and a `VersionOverride="4.11.0"` on the Workspaces reference in the CodeFixes project, removing the existing `MSBuildProjectName`-conditional `PackageVersion Update` group. Record the SDK constraint as a comment next to the pinned versions. The central `Microsoft.CodeAnalysis.CSharp.Workspaces` version remains 4.13.0 for the source-export tooling.

The product project adds a second analyzer reference alongside the existing one (illustrative — mirrors the existing reference):

```xml
<ProjectReference
    Include="../../tools/analyzers/Light.GuardClauses.InternalRoslynAnalyzers.CodeFixes/Light.GuardClauses.InternalRoslynAnalyzers.CodeFixes.csproj"
    OutputItemType="Analyzer"
    ReferenceOutputAssembly="false" />
```

The compiler finds no analyzer types in the CodeFixes assembly and ignores it; IDEs load the fix providers from it with Workspaces available at runtime. This is the same layout analyzer NuGet packages such as StyleCop.Analyzers use.

`Light.GuardClauses.InternalRoslynAnalyzers.Tests` currently references only the analyzer project but tests both the analyzer and the fixes, and its helpers use Workspaces types that today arrive transitively through the analyzer. Repoint it to reference the CodeFixes project (the analyzer project arrives transitively); no test logic changes are expected.

Behavior does not change; the existing test suite covering the analyzer and both code fixes constitutes the required coverage.
