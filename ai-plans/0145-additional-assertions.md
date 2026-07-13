# Additional Assertions

## Rationale

Light.GuardClauses already covers the common null, equality, comparison, string, collection, date/time, type, enum, and URI preconditions, but several useful invariants and cross-container overloads are still missing. Add focused guards for UUIDv7 identifiers, finite floating-point values, ranged collection counts, canonical UTC `DateTimeOffset` values, and ASCII data, and complete the most useful `Span<T>` and `Memory<T>` length and content checks.

The additions must preserve the library's existing fluent return values, exception-factory overloads, nullable annotations, broad target-framework support, Native AOT compatibility, and customizable single-file source distribution. UUIDv7 validation is performance-sensitive and must inspect the GUID structure without allocations on every supported target.

## Acceptance Criteria

- [x] `Guid` values can be tested with `IsUuidVersion7` and guarded with `MustBeUuidVersion7` on .NET Standard 2.0, .NET Standard 2.1, and .NET 10; a value is accepted only when both its UUID version and RFC/IETF variant bits identify UUIDv7.
- [x] Successful UUIDv7 checks perform no heap allocation and have a microbenchmark covering the Boolean check and successful throwing guard against an equivalent direct structural check.
- [x] The benchmark project targets and executes only .NET 10, with no .NET Framework target or legacy runtime job, so benchmarks can run consistently on every operating system supported by .NET 10 and BenchmarkDotNet.
- [x] `float` and `double` values can be tested and guarded for finiteness on all package targets, while the .NET 10 asset also supports generic IEEE 754 floating-point types such as `Half`.
- [x] General collections can require their count to fall within a `Range<int>` through `MustHaveCountIn`, preserving the original collection shape and the established collection-count behavior.
- [x] Span and memory values have consistent non-empty and ranged-length guards, memory values have exact-length guards, and character span and memory values have a throwing counterpart to `IsEmptyOrWhiteSpace`.
- [x] `MustBeUtc` supports `DateTimeOffset` and accepts only values whose offset is `TimeSpan.Zero`, without changing the instant or returned value.
- [x] Characters, bytes, strings, and character/byte span and memory values can be tested and guarded as ASCII across all package targets; empty inputs are valid ASCII, while null strings are rejected by the throwing guard.
- [x] Every new throwing guard returns the successfully validated input, captures the guarded expression for the default exception, accepts an optional custom message, and provides an exception-factory overload consistent with the existing API for its input shape.
- [x] Automated tests cover successful values, every distinct failure condition, boundary values, default exceptions, custom messages, custom exception factories, caller argument expressions, return values, and target-specific overloads.
- [x] The source-export whitelist catalog and committed settings contain all new assertion families, framework flattening and reachability retain their required helpers, and focused source-export tests cover the new entries and relevant target-specific APIs.
- [x] The committed .NET Standard 2.0 single-file distribution is regenerated and validates with the complete new portable API surface.
- [x] The assertion overview and any affected usage or source-inclusion documentation describe the new families, their supported input shapes, target-specific overloads, and UUIDv7 structural-validation semantics.
- [x] The complete solution restores and builds without warnings in Release configuration, and all automated tests pass on the pinned SDK.

## Technical Details

Follow the existing convention of one `Check.<Assertion>.cs` file per assertion family. Boolean checks are allocation-free predicates; throwing guards use `[CallerArgumentExpression]`, return the original value or collection shape, and offer the established default-exception and custom-factory overloads. Reuse the current exception hierarchy where it accurately describes the failure, preferring standard `ArgumentException` or `ArgumentOutOfRangeException` over adding narrowly useful public exception types. Span factories use the existing span delegate types so no ref struct is captured or boxed.

**UUIDv7.** The exact public names are `IsUuidVersion7` and `MustBeUuidVersion7`. Validation is structural: the version field must be 7 and the variant must be the RFC/IETF `10xx` variant defined by RFC 9562. This inherently rejects `Guid.Empty`; do not add a redundant emptiness check or claim to verify timestamp provenance, entropy, randomness, or monotonic generation.

Use one zero-allocation implementation across all target frameworks. Overlay the GUID with this exact internal type:

```csharp
[StructLayout(LayoutKind.Explicit, Size = 16)]
internal readonly struct GuidLayout
{
    [FieldOffset(0)]
    private readonly Guid _value;

    [FieldOffset(6)]
    public readonly ushort TimeHighAndVersion;

    [FieldOffset(8)]
    public readonly byte ClockSequenceHighAndReserved;

    public GuidLayout(Guid value) => _value = value;
}
```

The explicit overlay avoids array or string creation, unsafe compilation, and a dependency on `System.Runtime.CompilerServices.Unsafe`. Determine UUIDv7 with `(layout.TimeHighAndVersion & 0xF000) == 0x7000` and `(layout.ClockSequenceHighAndReserved & 0xC0) == 0x80`. Reading the version as the overlaid numeric field rather than indexing its component bytes keeps the check independent of machine endianness. Keep the layout helper reachable by the source exporter, document its dependency on the stable sequential GUID field layout, and cover it with tests using RFC UUIDv7 examples, `Guid.CreateVersion7()` on .NET 10, UUIDs of other versions, `Guid.Empty`, and a forged value with a version-7 nibble but a non-RFC variant. Add BenchmarkDotNet cases with memory diagnostics for valid and invalid Boolean checks and the successful guard path; the successful cases must report `0 B/op`.

Make the benchmark project .NET 10-only by replacing its target-framework list with `net10.0` and removing the .NET Framework 4.8 runtime job. Replace the stale explicit .NET 8 job with one .NET 10 job. Enable the cross-platform memory diagnoser by default and make disassembly diagnostics opt-in through a benchmark configuration or command-line option so an unsupported disassembler cannot prevent ordinary benchmark execution. The default configuration must not depend on Windows, an installed .NET Framework runtime, or platform-specific diagnostic tooling.

**Finite floating-point values.** Add `IsFinite` and `MustBeFinite` overloads for `float` and `double` to every target. The modern asset additionally exposes generic overloads constrained to `IFloatingPointIeee754<T>` under the repository's existing modern-framework conditional, thereby covering `Half` without treating `decimal` as an IEEE 754 type. Finite values include positive and negative zero, subnormal values, and normal values; reject `NaN`, positive infinity, and negative infinity. The portable overloads must not depend on APIs absent from .NET Standard.

**Collection count ranges.** Add `MustHaveCountIn<TCollection>(this TCollection? parameter, Range<int> range, ...) where TCollection : class, IEnumerable`, plus its factory overload. Determine the count through the existing optimized collection-count helpers and enumerate a lazy enumerable no more than once per guard invocation. Null uses the existing null behavior; an out-of-range count uses `InvalidCollectionCountException` and reports the actual count and requested range. Preserve inclusive and exclusive endpoints exactly as modeled by `Range<int>`.

**Span and memory parity.** Extend the existing families rather than introducing alternate length terminology:

- `MustNotBeEmpty` for `Span<T>`, `ReadOnlySpan<T>`, `Memory<T>`, and `ReadOnlyMemory<T>`;
- `MustHaveLengthIn` for those same four shapes;
- `MustHaveLength` for `Memory<T>` and `ReadOnlyMemory<T>`; and
- `MustNotBeEmptyOrWhiteSpace` for `Span<char>`, `ReadOnlySpan<char>`, `Memory<char>`, and `ReadOnlyMemory<char>`, implemented as the throwing counterpart to `IsEmptyOrWhiteSpace`.

Mutable overloads should delegate validation to the read-only span implementation and return the original mutable value. Memory overloads must not allocate or copy their contents. Use `InvalidCollectionCountException` or the existing empty/string exception contracts consistently with the corresponding collection, length, and whitespace guards.

**UTC `DateTimeOffset`.** Extend `MustBeUtc` with `DateTimeOffset` default and factory overloads. UTC means `parameter.Offset == TimeSpan.Zero`; do not convert a non-zero-offset value with `ToUniversalTime`, because a guard validates rather than normalizes. Continue using `InvalidDateTimeException`, updating its documentation and throw helpers to cover both `DateTime` and `DateTimeOffset` accurately.

**ASCII.** Add `IsAscii` and `MustBeAscii` for `char`, `byte`, `string`, `Span<char>`, `ReadOnlySpan<char>`, `Memory<char>`, `ReadOnlyMemory<char>`, and the corresponding byte span and memory shapes. ASCII means every code unit or byte is at most `0x7F`; empty strings and buffers satisfy the predicate, `IsAscii(null)` is false, and `MustBeAscii` throws `ArgumentNullException` for a null string before checking content. Use the optimized framework implementation where appropriate on .NET 10 and a simple allocation-free portable loop elsewhere, with identical semantics across targets. Mutable and memory guards delegate to read-only span validation and return their original shape.

Update `AssertionWhitelist`, `settings.json`, and source-export reachability tests for `IsUuidVersion7`, `MustBeUuidVersion7`, `IsFinite`, `MustBeFinite`, `MustHaveCountIn`, `MustNotBeEmptyOrWhiteSpace`, `IsAscii`, and `MustBeAscii`; additions that only extend existing files use their existing whitelist entries. Regenerate `Light.GuardClauses.SingleFile.cs` as the .NET Standard 2.0 output and verify both portable and .NET 10 generated-source validation. Update `docs/assertion-overview.md` as the primary user-facing catalog, and adjust other documentation where the overload matrix or source-export examples would otherwise become incomplete.
