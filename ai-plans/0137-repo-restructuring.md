# Repository Restructuring

## Rationale

The repository still groups solutions, projects, build configuration, development tools, and AI instructions under a generic `Code` directory. Restructure it around conventional repository-level entry points and purpose-specific source, test, benchmark, and tooling directories so that the project is easier to navigate, build, and maintain.

## Acceptance Criteria

- [x] The `ai-plans` directory and its instructions reside at the repository root.
- [x] `AGENTS.md` and `CLAUDE.md` reside at the repository root and apply to the intended repository scope.
- [x] The repository contains no `Code` directory; product source, tests, benchmarks, and internal tools use the agreed purpose-specific directories.
- [x] A single root-level `Light.GuardClauses.sln` contains all projects and presents the repository structure through corresponding solution folders.
- [x] The shared `Light.GuardClauses.sln.DotSettings` file resides beside the solution, contains the intended team settings, and contains no machine-specific paths or references to the removed solution.
- [x] Shared MSBuild and package configuration is discovered from the repository root, with project-type defaults inherited through hierarchical `Directory.Build.props` files rather than duplicated across project files.
- [x] Project references and repository-path discovery work with the new layout without relying on a directory named `Code` or on prebuilt project outputs.
- [x] The root-level `.idea` metadata integrates `ai-plans` into Rider's Solution Explorer without individual solution-file entries and contains no references to the old solution name or location.
- [x] Operational documentation, ignore rules, and automation contain no stale actionable paths to the old layout; completed historical plans remain unchanged.
- [x] Continuous integration restores, builds, and tests the complete solution, including all three test projects.
- [x] The release workflow packs and publishes `Light.GuardClauses` from its new location without changing the package identity, metadata, symbols, or packaged documentation and icon.
- [x] The source-code transformation can locate the product sources and validation project, generate the root-level single-file distribution, and validate its supported target frameworks from the new layout.
- [x] The complete solution builds successfully in Release configuration and all automated tests pass on the supported .NET SDK.

## Technical Details

The following target layout is exact at the directory and project level; files within project directories retain their existing organization:

```text
/
|-- Light.GuardClauses.sln
|-- Light.GuardClauses.sln.DotSettings
|-- Directory.Build.props
|-- Directory.Packages.props
|-- global.json
|-- AGENTS.md
|-- CLAUDE.md
|-- .idea/
|-- ai-plans/
|-- benchmarks/
|   `-- Light.GuardClauses.Performance/
|-- src/
|   |-- Directory.Build.props
|   `-- Light.GuardClauses/
|-- tests/
|   |-- Directory.Build.props
|   |-- Light.GuardClauses.Tests/
|   |-- Light.GuardClauses.InternalRoslynAnalyzers.Tests/
|   `-- Light.GuardClauses.SourceCodeTransformation.Tests/
`-- tools/
    |-- analyzers/
    |   `-- Light.GuardClauses.InternalRoslynAnalyzers/
    `-- source-export/
        |-- Light.GuardClauses.SourceCodeTransformation/
        `-- Light.GuardClauses.SourceValidation/
```

Retain the conventional `Light.GuardClauses.sln` name and classic solution format while replacing its limited project set with all eight projects from `Light.GuardClauses.AllProjects.sln`. Use solution folders matching the physical top-level project directories and include relevant root configuration and documentation as solution items. Retain only the `Debug|Any CPU` and `Release|Any CPU` solution configurations; the existing `x86` and `x64` configurations only map back to `Any CPU`. Remove the AllProjects solution afterward. Base the surviving DotSettings file on the settings used by the complete solution, reconcile intentional differences between the two existing files, and remove the stale injected layer containing an absolute Windows path.

Move `Directory.Packages.props` to the root so central package management covers every project. Replace `Version.props` and duplicated stable properties with a root `Directory.Build.props` containing the version, C# language version, author, company, copyright, and a non-packable default. Add child props for `src` and `tests`; each child must explicitly import the nearest parent props because MSBuild automatically imports only one `Directory.Build.props`. Product packaging, SourceLink settings, package content, and `IsPackable=true` belong under `src`, while test defaults and common xUnit, test SDK, runner, and assertion references belong under `tests`. Keep target frameworks, nullable settings, warning policies, and exceptional build behavior in the individual project files.

Replace the product project's conditional reference to the analyzer DLL under `bin` with the following exact analyzer reference shape, adjusted only if the final relative project path requires it:

```xml
<ProjectReference
    Include="../../tools/analyzers/Light.GuardClauses.InternalRoslynAnalyzers/Light.GuardClauses.InternalRoslynAnalyzers.csproj"
    OutputItemType="Analyzer"
    ReferenceOutputAssembly="false" />
```

This makes the build graph explicit and removes the need for a separate analyzer build step or solution-only project dependency. Preserve the existing `RS1038` behavior during this restructuring; resolving the analyzer assembly's dependency on Roslyn Workspaces is separate work.

Update source-export defaults, validation-project lookup, and test environment discovery to locate the repository through a stable root marker and the exact new paths. They must work when invoked from the repository root and from project output directories. Preserve the generated file at `Light.GuardClauses.SingleFile.cs` in the repository root and the current validation behavior for `netstandard2.0` and `net8.0`.

Update workflow path filters and all restore, build, test, pack, signing-key, artifact, and cleanup paths. CI should test the complete solution rather than only `Light.GuardClauses.Tests`. Add a root `global.json` that pins an appropriate .NET 8 SDK feature band consistent with CI; adopting `.slnx` or raising the SDK baseline is outside this restructuring.

Move the shared `.idea` project metadata to the repository root, rename its project directory for the canonical `Light.GuardClauses` solution, and retain the index-layout attachment that exposes `ai-plans` in Rider's Solution Explorer. Continue ignoring user-specific Rider workspace state and configure `.gitignore` so that only the intentionally shared metadata is tracked. Update the remaining ignore entries for benchmark artifacts, local source-export settings, and obsolete generated-source paths. Update actionable paths in the README and other operational documentation without rewriting completed plans that describe the historical layout.
