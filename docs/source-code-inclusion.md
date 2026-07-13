# Including Light.GuardClauses as source code

[Documentation index](README.md) · [Contributing and building](contributing-and-building.md) · [Structuring checks](structuring-precondition-checks.md) · **Source inclusion** · [Assertions](assertion-overview.md) · [Historical performance](historical-performance.md) · [Background](guard-clause-background.md)

Light.GuardClauses can be embedded as C# source when taking a dependency on the NuGet assembly is undesirable. Use either the committed portable file or the custom exporter.

## Ready-made .NET Standard 2.0 file

Copy [`Light.GuardClauses.SingleFile.cs`](../Light.GuardClauses.SingleFile.cs) into the consuming project. It is the committed .NET Standard 2.0 distribution generated from the current sources. Its default configuration:

- changes public Light.GuardClauses types to `internal`, avoiding additions to the consuming assembly's public API;
- enables nullable annotations;
- retains JetBrains contract annotations and the code-analysis, validated-null, and caller-argument-expression support needed by the portable source shape;
- includes the full selected-framework assertion surface; and
- contains one flattened source shape with no conditional-compilation directives.

The generated source uses `System.Collections.Immutable` and span-related framework types. The consuming project must reference assemblies or packages that provide the APIs required by its target.

## Custom source export

The exporter project can be found at [`tools/source-export/Light.GuardClauses.SourceCodeTransformation`](../tools/source-export/Light.GuardClauses.SourceCodeTransformation/). It reads configuration in this order, with each later source overriding earlier values:

1. built-in `SourceFileMergeOptions` defaults;
2. optional `settings.json` from the application's base directory;
3. optional `settings.local.json` from the application's base directory;
4. command-line arguments.

The project copies both settings files from the project directory to its output directory when they exist, so `dotnet run` uses them regardless of the shell's working directory. The committed [`settings.json`](../tools/source-export/Light.GuardClauses.SourceCodeTransformation/settings.json) is the canonical complete option and assertion catalog. The optional `settings.local.json` is ignored by Git and is intended for local overrides.

From the repository root, a command-line override can select the modern source shape and a different output file:

```shell
dotnet run --project tools/source-export/Light.GuardClauses.SourceCodeTransformation -- \
  --TargetFramework Net10_0 \
  --TargetFile Light.GuardClauses.Net10.cs
```

Configuration keys are case-insensitive. Nested command-line keys use `:`, for example `--AssertionWhitelist:IsEnabled true`.

### Core options

| Option | Default | Effect |
| --- | --- | --- |
| `SourceFolder` | Repository `src/Light.GuardClauses` | Directory containing the source files |
| `TargetFile` | Root `Light.GuardClauses.SingleFile.cs` | Generated output path |
| `TargetFramework` | `NetStandard2_0` | Selects `NetStandard2_0` or `Net10_0` |
| `ChangePublicTypesToInternalTypes` | `true` | Makes exported public types internal |
| `BaseNamespace` | `Light.GuardClauses` | Rewrites the base namespace |
| `RemoveContractAnnotations` | `false` | Removes JetBrains contract annotations from members |
| `IncludeJetBrainsAnnotations` / `IncludeJetBrainsAnnotationsUsing` | `true` | Controls the bundled annotation and using directive |
| `IncludeVersionComment` | `true` | Adds the generated version header |
| `RemoveOverloadsWithExceptionFactory` | `false` | Globally removes assertion overloads that accept exception factories |
| Nullable/validated-null options | varies; see settings | Include supporting attributes or remove their usages |
| Caller-argument-expression options | included/not removed | Include the portable polyfill or remove annotation usages |
| `AssertionWhitelist` | disabled | Optionally limits the exported assertion roots |

Use the committed settings file for the exact names and defaults of all nullable, validated-null, and caller-expression switches.

## Target-framework flattening

`TargetFramework` selects exactly one source shape:

- `NetStandard2_0` is the portable shape used by the committed single-file distribution.
- `Net10_0` includes modern generic-math overloads and the .NET 10 span/memory email APIs. It suppresses code-analysis and caller-argument-expression polyfills already supplied by the framework.

The parser selects the appropriate conditional branches and the merger emits no `#if`, `#else`, `#endif`, or other conditional-compilation directives. The result is therefore for the selected target only, not a multi-target source file.

## Assertion whitelisting and dependency retention

When `AssertionWhitelist.IsEnabled` is `false`, all entry settings are ignored and the full surface for the selected framework is exported.

When enabled, entries whose `Include` value is `true` become reachability roots. The exporter retains their transitive dependencies, including required members of `Check` and `Throw`, exception types, attributes, exception-factory delegates, and supporting helpers. Unrelated public helpers and assertion APIs may be omitted.

Every entry defaults to `Include: true`. To create a genuinely small subset, start with the complete catalog in the committed `settings.json`, enable the whitelist, set every unwanted entry to `false`, and keep only the required roots. A shortened illustration of the resulting local file is:

```json
{
  "TargetFramework": "Net10_0",
  "AssertionWhitelist": {
    "IsEnabled": true,
    "MustNotBeNull": {
      "Include": true,
      "IncludeExceptionFactoryOverload": false
    },
    "MustNotBeEmpty": {
      "Include": true,
      "IncludeExceptionFactoryOverload": true
    }
  }
}
```

For this example to select only those two roots, all other catalog entries must be present with `"Include": false`; omitted entries remain enabled by default. This default is intentionally fail-safe.

For example, [`jq`](https://jqlang.org/) can turn the committed full catalog into that exact local subset without listing every exclusion by hand:

```shell
jq '
  .TargetFramework = "Net10_0" |
  .AssertionWhitelist.IsEnabled = true |
  (.AssertionWhitelist[] | select(type == "object") | .Include) = false |
  .AssertionWhitelist.MustNotBeNull = {
    "Include": true,
    "IncludeExceptionFactoryOverload": false
  } |
  .AssertionWhitelist.MustNotBeEmpty = {
    "Include": true,
    "IncludeExceptionFactoryOverload": true
  }
' tools/source-export/Light.GuardClauses.SourceCodeTransformation/settings.json \
  > tools/source-export/Light.GuardClauses.SourceCodeTransformation/settings.local.json

dotnet run --project tools/source-export/Light.GuardClauses.SourceCodeTransformation
```

`RemoveOverloadsWithExceptionFactory` is the global switch. When it is `false`, an included whitelist entry can still remove its factory overload with `IncludeExceptionFactoryOverload: false`. The per-entry value cannot restore factory overloads removed globally.

The exporter fails loudly if a `Check.<Name>.cs` assertion file has no corresponding whitelist property. This keeps the catalog synchronized even while whitelist mode is disabled.

## Validation and failures

After writing the target file, the tool builds it through [`Light.GuardClauses.SourceValidation`](../tools/source-export/Light.GuardClauses.SourceValidation/) using `netstandard2.0` or `net10.0` to match the selected source target.

If generation throws, the tool prints the exception and returns a non-zero exit code. If validation fails, it prints the captured build output and returns the build's non-zero exit code. In either case, a file already generated is left in place for diagnosis. Do not publish or commit custom output unless the command and matching validation complete successfully.
