# Stream Guards

## Rationale

Streams are commonly passed between upload, export, and document-generation layers, but an unsupported read, write, or seek operation may fail only after the stream has traveled far from the call site. Add dedicated `MustBeReadable`, `MustBeWritable`, and `MustBeSeekable` guards so callers can state these capability requirements at API boundaries.

The guards must be thin, allocation-free checks of the corresponding `Stream` capability properties, preserve the fluent API and concrete stream type, and participate in the customizable single-file source distribution.

## Acceptance Criteria

- [x] `MustBeReadable`, `MustBeWritable`, and `MustBeSeekable` are available for `Stream` instances and subclasses on .NET Standard 2.0, .NET Standard 2.1, and .NET 10 with identical semantics.
- [x] Each guard accepts a non-null stream exactly when its corresponding `CanRead`, `CanWrite`, or `CanSeek` property returns `true`; it does not inspect other capabilities, probe the stream with an I/O operation, change its position, or otherwise mutate it.
- [x] A null stream throws `ArgumentNullException` by default, while a stream lacking the required capability throws `ArgumentException` with the captured guarded expression and a capability-specific message; no new public exception type is introduced.
- [x] Every guard returns the original successfully validated instance in its concrete stream shape, accepts an optional custom message, and provides a custom-exception-factory overload consistent with the existing nullable reference APIs.
- [x] Successful default and custom-factory guards perform no heap allocation.
- [x] Automated tests cover all supported and unsupported capability states, null values, default exception contracts, custom messages, custom exception factories, caller argument expressions, preservation of the original instance and concrete type, and confirmation that each guard reads only its matching capability property.
- [x] The source-export whitelist catalog and committed settings contain all three assertion families, and focused source-export tests cover their guard, throw-helper, and exception-factory reachability.
- [x] The committed .NET Standard 2.0 single-file distribution is regenerated with the new stream guards, and generated source validates for both .NET Standard 2.0 and .NET 10.
- [x] XML documentation and the assertion overview describe the three guards, their null and failure behavior, fluent return values, and property-only validation semantics.
- [x] The complete solution restores and builds without warnings in Release configuration, and all automated tests pass on the pinned SDK.

## Technical Details

Implement one `Check.<Assertion>.cs` file per guard family. Use a generic stream subtype parameter so fluent chains retain types such as `FileStream` and `MemoryStream`. The following signatures illustrate the shared public shape; the writable and seekable families mirror it:

```csharp
public static TStream MustBeReadable<TStream>(
    [NotNull, ValidatedNotNull] this TStream? parameter,
    [CallerArgumentExpression("parameter")] string? parameterName = null,
    string? message = null
) where TStream : Stream;

public static TStream MustBeReadable<TStream>(
    [NotNull, ValidatedNotNull] this TStream? parameter,
    Func<TStream?, Exception> exceptionFactory
) where TStream : Stream;
```

Apply the repository's established nullable-flow and JetBrains contract annotations. Default overloads should follow the `MustBeAbsoluteUri` null-check pattern: validate null first, inspect the relevant property once, and return the same reference. Factory overloads invoke the factory for either null or capability failure and pass the original nullable stream value to it.

The hot success path is only a null check, one virtual property read, one conditional branch, and the return. Keep it aggressively inlineable and route default failure construction through non-returning `Throw` helpers. Those helpers should create `ArgumentException` messages that name the missing capability without formatting the stream or calling its `ToString`; failures must not attempt an operation to discover why a capability is unavailable. Boolean `IsReadable`, `IsWritable`, and `IsSeekable` predicates are out of scope because `CanRead`, `CanWrite`, and `CanSeek` already expose the predicates directly.

Use small hand-written `Stream` test doubles to represent independent capability combinations and to record property access; do not assume that capabilities imply one another. Include disposed framework streams as representative property-defined failures, but preserve the `Stream` implementation's own behavior if a custom capability getter throws.

Add `MustBeReadable`, `MustBeWritable`, and `MustBeSeekable` to `AssertionWhitelist` and `settings.json`, cover trimming/reachability of their default and factory paths, regenerate `Light.GuardClauses.SingleFile.cs` as the .NET Standard 2.0 artifact, and add a stream assertion section to `docs/assertion-overview.md`.
