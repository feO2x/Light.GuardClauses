# Parsable URI String Assertion

## Rationale

Parent issue #162 (point 2) identifies URI validation on strings as a recurring need: BrilliantMessaging validates a URI supplied as a string and accepts `UriKind.RelativeOrAbsolute`, but the existing URI assertions operate on `Uri` instances, forcing callers to write `Uri.TryCreate` logic themselves. Add `Check.MustBeUri` for strings with a configurable `UriKind`, so URI string validation reads explicitly and remains usable on `netstandard2.0`.

## Acceptance Criteria

- [ ] `Check.MustBeUri(parameter, uriKind, parameterName, message)` returns the original string when `Uri.TryCreate` accepts it under the supplied `UriKind` (default `RelativeOrAbsolute`), throws the new `InvalidUriException` (deriving from `UriException`) when parsing fails, and throws `ArgumentNullException` when the parameter is null — identically on .NET Standard 2.0, .NET Standard 2.1, and .NET 10. The exception carries the parameter name and optional custom message like the existing URI exceptions.
- [ ] Two custom-exception-factory overloads mirror the `MustHaveScheme` convention (`Func<string?, Exception>` and `Func<string?, UriKind, Exception>`); they throw the factory's exception only when validation fails (a null parameter counts as failure), never invoke the factory otherwise, and a null factory on failure throws `ArgumentNullException` via the existing `Throw.CustomException` convention.
- [ ] Automated tests cover: parsable and unparsable strings for each `UriKind` (including relative-only and absolute-only acceptance), null parameter handling for all overloads, return-value identity with the input string, parameter-name and custom-message propagation, both factory overloads (including the passed parameter and `UriKind`), factories not invoked on success, and null-factory behavior.
- [ ] The source-export whitelist catalog and committed settings contain `MustBeUri`, and focused source-export tests cover retention of the guard, its `Throw.MustBeUri` helper, and the `InvalidUriException` type as well as trimming of the exception-factory overloads when configured.
- [ ] The committed .NET Standard 2.0 single-file distribution is regenerated with the new guard and validates for both supported source-export targets.
- [ ] The "URI assertions" table in `docs/assertion-overview.md` lists `MustBeUri`, and the 15.0.0 package release notes in `src/Directory.Build.props` mention the new guard.
- [ ] The complete solution restores and builds without warnings in Release configuration, and all automated tests pass on the pinned SDK.

## Technical Details

Add the guard as a new `Check.MustBeUri.cs` modeled on `Check.MustBeEmailAddress.cs` (aggressive inlining, JetBrains contract annotations, XML documentation style). The exact public shape is:

```csharp
public static string MustBeUri(
    this string? parameter,
    UriKind uriKind = UriKind.RelativeOrAbsolute,
    [CallerArgumentExpression("parameter")] string? parameterName = null,
    string? message = null
);

public static string MustBeUri(
    this string? parameter,
    UriKind uriKind,
    Func<string?, Exception> exceptionFactory
);

public static string MustBeUri(
    this string? parameter,
    UriKind uriKind,
    Func<string?, UriKind, Exception> exceptionFactory
);
```

The guard delegates to `Uri.TryCreate(parameter, uriKind, out _)`, which exists on all library targets; BCL semantics apply verbatim (for example, the empty string parses as a valid relative reference and passes with `RelativeOrAbsolute`). The return-type question left open in #162 is resolved in favor of library convention: the assertion returns the original string for fluent argument validation, and callers that need a `Uri` can safely construct one after the guard. No span or memory overloads are added because `Uri.TryCreate` is string-based only, and no microbenchmarks are needed for this thin wrapper.

Add `InvalidUriException : UriException` in `Exceptions/InvalidUriException.cs`, modeled on `RelativeUriException` (`[Serializable]`, protected serialization constructor under `#if !NET8_0_OR_GREATER`). Deriving from `UriException` groups string parsing failures with the other URI assertion failures under a common `ArgumentException` base. Add a non-returning `Throw.MustBeUri` helper to the existing `ExceptionFactory/Throw.Uri.cs` (`[ContractAnnotation("=> halt")]`, `[DoesNotReturn]`) whose default message follows the established URI pattern, e.g. `{parameterName ?? "The string"} must be a valid URI ({uriKind}), but it actually is "{parameter}".`

The default overload validates null via `MustNotBeNull` (like `MustBeEmailAddress`); the factory overloads treat null as a validation failure and pass the null value to the factory, matching the existing string-assertion convention.

Add a `MustBeUri` entry to `AssertionWhitelist` and `settings.json` between `MustBeRelativeUri` and `MustContain` (preserving alphabetical position), add focused facts to `SourceFileMergerWhitelistTests` modeled on the `ObjectDisposed` facts for dependency retention and factory-overload trimming, add `MustBeUriTests` under `tests/Light.GuardClauses.Tests/UriAssertions/` following `MustBeAbsoluteUriTests` conventions (FluentAssertions, `Test.CustomMessage`/`Test.CustomException` helpers, `DefaultVariablesData` where useful), extend the per-assertion nullability flow-analysis facts in `tests/Light.GuardClauses.Tests/Issues/Issue72NotNullAttributeTests.cs` (which exhaustively cover value-returning assertions such as `MustBeAbsoluteUri`), and regenerate `Light.GuardClauses.SingleFile.cs` via the source-export project's committed settings.
