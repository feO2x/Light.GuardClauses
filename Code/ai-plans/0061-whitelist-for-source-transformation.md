# Issue 61 - Whitelist for Source Transformation

## Rationale

`Light.GuardClauses.SourceCodeTransformation` merges the library sources into a single file for consumers who want to embed Light.GuardClauses as source code instead of referencing the DLL. Today, the export always includes the complete source-export surface. Some consumers only need a small subset of assertions, so we introduce an opt-in assertion whitelist on `SourceFileMergeOptions`.

When the whitelist is enabled, the export is assertion-only: selected `Check.*` methods are the roots, dependency declarations are retained, and unrelated public helper APIs may be omitted. When the whitelist is disabled, merge output remains unchanged from today.

File-name trimming is not sufficient: only 14 of the 100 current assertions have a same-named `Throw.<Name>.cs` file, and several `Throw.*.cs` files contain methods for unrelated assertions. A Roslyn semantic reachability analysis, targeting the `netstandard2.0` source-export shape used by `Light.GuardClauses.Source`, should compute the minimal safe dependency closure. The implementation must prefer conservative over-inclusion over broken output, with a post-generation project build as a safety net.

## Acceptance Criteria

- [x] `SourceFileMergeOptions` gets exactly one new top-level property: `public AssertionWhitelist AssertionWhitelist { get; init; } = new();`.
- [x] A new `AssertionWhitelist` record exposes `bool IsEnabled { get; init; } = false;` plus one `AssertionEntry` property per assertion, named after the `<Name>` portion of its `Check.<Name>.cs` file. `AssertionEntry` exposes `bool Include { get; init; } = true;` and `bool IncludeExceptionFactoryOverload { get; init; } = true;`.
- [x] When `AssertionWhitelist.IsEnabled` is `false`, all whitelist entries are ignored: no assertion filtering runs, per-assertion overload flags do not affect output, and the generated file remains equivalent to today's output for the same existing options.
- [x] When `AssertionWhitelist.IsEnabled` is `true`, the generated file is assertion-only: selected active `Check` methods are roots, required source dependencies are retained, unrelated public helper APIs may be omitted, and analysis uses the `netstandard2.0` source shape without `NET8_0` / `NET8_0_OR_GREATER` code.
- [x] Reachability includes symbols referenced from signatures, bodies, initializers, inheritance, attributes, generic constraints, delegates, nested types, and expressions. If a source dependency cannot be resolved precisely, the implementation keeps the relevant containing declaration conservatively.
- [x] `Check` and `Throw` are pruned by member where possible; support declarations may be retained at whole-type granularity when that is the smallest safe unit. Cross-assertion dependencies are pulled in automatically, and bundled `Throw.*.cs` files are trimmed by reachable member rather than by file name.
- [x] The existing global `RemoveOverloadsWithExceptionFactory` behavior remains unchanged. In whitelist mode, an assertion entry with `IncludeExceptionFactoryOverload == false` removes only that assertion's `Check` overloads with an `exceptionFactory` parameter before reachability runs; shared `Throw.CustomException` members and span delegate types are retained only if still reachable.
- [x] `Check.cs` / `Throw.cs` partial class shells and attribute files remain governed by their existing options.
- [x] After writing the target file, the tool builds the `.csproj` found in the same directory as `options.TargetFile`; if none is found, validation is skipped. If multiple project files are found, the tool prints a warning and skips validation. On build failure, the tool prints captured output, returns a non-zero code, and leaves the generated file on disk.
- [x] `settings.json` is committed for `Light.GuardClauses.SourceCodeTransformation` with whitelist mode enabled and every assertion entry set to include both normal and exception-factory overloads, producing the assertion-only export for all assertions. `settings.local.json` is ignored, loaded after `settings.json` and before command-line arguments, and both settings files are copied to the output directory when present.
- [x] Automated tests need to be written.

## Technical Details

- **Options and config**: add `AssertionWhitelist.cs` with a sealed `AssertionWhitelist` record and nested `AssertionEntry` record. Use reflection to map assertion names to whitelist properties so assertion names are not maintained twice. Per-entry flags are consulted only when `IsEnabled` is `true`. Update `.gitignore`, `Program.cs`, and the source transformation project file for `settings.json` / `settings.local.json` layering and copy-to-output behavior.

- **Source catalog and compilation**: parse every source file with `CSharpSyntaxTree.ParseText(text, parseOptions, path: file.FullName)` using `CSharpParseOptions(LanguageVersion.CSharp12, preprocessorSymbols: ["NETSTANDARD", "NETSTANDARD2_0"])`; do not define `NET8_0` or `NET8_0_OR_GREATER`. Build a catalog of source declarations and a `CSharpCompilation` with trusted platform assemblies plus required package references such as `System.Collections.Immutable`. This compilation supports reachability only; the generated project build is the source of truth for final preprocessor and compile correctness.

- **Root selection**: in whitelist mode, roots are active `Check` methods from assertion files whose entries have `Include == true`. Remove an assertion's `Check` methods with an `exceptionFactory` parameter before root selection when that entry has `IncludeExceptionFactoryOverload == false`; also apply the existing global `RemoveOverloadsWithExceptionFactory` option when set.

- **Reachability traversal**: resolve source symbols with Roslyn and compare them via `SymbolEqualityComparer.Default`, normalizing with `OriginalDefinition` where appropriate. Inspect declaration shape and executable code, including type syntax, attributes, base lists, constraints, parameters, return types, field/property initializers, accessors, constructor initializers, operators, expression-bodied members, invocations, object creations, identifiers, and member access. Treat candidate source symbols as reachable when binding is ambiguous.

- **Retention policy**: retain `Check` and `Throw` by reachable member where possible. For exceptions, delegates, enums, structs, nested types, comparers, `Range`, `EnumInfo`, `RegularExpressions`, and framework extension helper types, prefer correctness over perfect minimality; retaining the whole containing type is acceptable once any part is reachable.

- **Merger integration**: refactor `SourceFileMerger` enough to apply reachability before inserting members into the final namespaces. In whitelist mode, do not add root-namespace helpers, exception types, framework extension types, or `Throw` members just because their files exist. In non-whitelist mode, preserve the current merge behavior.

- **Build validation**: add `GeneratedFileBuildValidator` and call it from `Program.Main` after `SourceFileMerger.CreateSingleSourceFile` writes `options.TargetFile`. It should run `dotnet build` with captured stdout / stderr when exactly one project file exists next to the target file, skip when none exists, warn and skip when multiple exist, and report build failure through `Program.Main`'s exit code.

- **Tests**: add tests in `Light.GuardClauses.SourceCodeTransformation.Tests` using real files from `Light.GuardClauses`. Cover disabled-mode output equivalence and ignored per-entry flags; assertion-only trimming of unrelated public helpers; cross-assertion reachability (`MustBeGreaterThan` retains `MustNotBeNullReference`); bundled `Throw` pruning (`MustBeAbsoluteUri` omits unrelated URI throw members); exception inheritance (`RelativeUriException` retains `UriException`); support closures for `Range`, `EnumInfo` / `Types`, `RegularExpressions`, and span delegate dependencies under `netstandard2.0`; per-assertion exception-factory trimming; and build validation success, failure, non-whitelist execution, and skip behavior.

- No benchmarks needed. This feature runs once per source export, not in a hot path.
