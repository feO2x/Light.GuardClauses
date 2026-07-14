# Collection Content Guards

## Rationale

Collections received from deserialization, binding, and caller-supplied arrays frequently have invariants about their contents rather than only their own nullability or size. Callers currently express these checks with manual loops or LINQ, losing the intent and the library's standard exception, nullability, caller-expression, and fluent-return behavior. Add `MustNotContainNull` and `MustNotContainNullOrWhiteSpace` as named collection guards.

The additions must work across every package target, enumerate safely and no more than once, preserve collection shapes, remain Native AOT compatible, and participate in the customizable single-file source distribution.

## Acceptance Criteria

- [x] `MustNotContainNull` and `MustNotContainNullOrWhiteSpace` are available with identical semantics on .NET Standard 2.0, .NET Standard 2.1, and .NET 10.
- [x] `MustNotContainNull` accepts nullable reference collections implementing `IEnumerable`, rejects any null item (including an empty nullable value type boxed through non-generic enumeration), and accepts empty collections and collections containing only non-null items.
- [x] `MustNotContainNullOrWhiteSpace` accepts nullable reference collections of strings and rejects null, empty, or whitespace-only items using the same Unicode whitespace semantics as `string.IsNullOrWhiteSpace`; empty collections and collections of non-blank strings are accepted.
- [x] Both guards preserve and return the original reference collection shape, and dedicated overloads preserve `ImmutableArray<T>` without boxing. A default immutable array is treated as having no items and therefore succeeds.
- [x] Each invocation obtains at most one enumerator, disposes it normally, and stops at the first invalid item. Failure-message construction does not enumerate the input again, and validation uses constant additional space.
- [x] Common indexable receivers such as arrays and `List<T>` are inspected by index without allocating an enumerator; arbitrary enumerable receivers retain the single-enumerator fallback.
- [x] A null reference collection passed to a default overload throws `ArgumentNullException`, while invalid null or blank content throws `ExistingItemException`. Default messages identify the guarded expression, the failure category, and the zero-based position of the first offending item without rendering the whole collection.
- [x] Every overload returns the successfully validated input, captures the guarded expression for default exceptions, accepts an optional custom message, and has a custom-exception-factory counterpart consistent with the existing API and the receiver shape.
- [x] Automated tests cover empty, valid, and failing inputs; arrays, `List<T>`, lazy/single-use enumerables, nullable reference and value-type items, and default/empty/non-empty immutable arrays; null, empty, ASCII-whitespace, and Unicode-whitespace strings; null receivers; default exceptions and messages; custom messages and factories; caller argument expressions; fluent return types; early termination; single enumeration; and enumerator disposal.
- [x] Microbenchmarks compare both guards' indexable `List<T>` fast paths with equivalent imperative loops and report allocations.
- [x] The source-export whitelist catalog and committed settings contain both assertion families, and focused source-export tests verify their guards, exception and throw-helper reachability, immutable-array overloads, and exception-factory trimming.
- [x] The committed .NET Standard 2.0 single-file distribution is regenerated and validates with the new portable API surface.
- [x] XML documentation and the assertion overview describe the supported receiver shapes, vacuous success for empty/default immutable collections, the guards' O(n), constant-additional-space, single-pass, short-circuiting enumeration behavior, and the boxing incurred when `MustNotContainNull` enumerates value-type items through non-generic `IEnumerable`.
- [x] The complete solution restores and builds without warnings in Release configuration, and all automated tests pass on the pinned SDK.

## Technical Details

Use one `Check.<Assertion>.cs` file per family. The null-content guard can preserve any reference receiver through non-generic enumeration because it needs no item type; the string guard can likewise infer the receiver while fixing the item type in its constraint (illustrative signatures):

```csharp
public static TCollection MustNotContainNull<TCollection>(
    [NotNull, ValidatedNotNull] this TCollection? parameter,
    [CallerArgumentExpression("parameter")] string? parameterName = null,
    string? message = null
) where TCollection : class, IEnumerable;

public static TCollection MustNotContainNullOrWhiteSpace<TCollection>(
    [NotNull, ValidatedNotNull] this TCollection? parameter,
    [CallerArgumentExpression("parameter")] string? parameterName = null,
    string? message = null
) where TCollection : class, IEnumerable<string?>;
```

Add factory counterparts receiving `TCollection?`, with the established `[ContractAnnotation]` and nullable-flow attributes. Add dedicated `ImmutableArray<T>` / `ImmutableArray<string?>` overloads and factories so the value-type receiver is neither boxed nor shape-erased. Checking each item with `is null` covers reference types and `Nullable<T>` values through `IEnumerable`; this non-generic enumeration boxes non-null value-type items, so call out that cost in the public XML remarks rather than implying the guard is allocation-free for those collections. The string family should use the existing `IsNullOrWhiteSpace` predicate. Do not compose the string guard from multiple guards, because that would enumerate a lazy input more than once.

Inspect indexable receivers without obtaining an enumerator: use non-generic `IList` for the null-content guard and `IList<string?>` / `IReadOnlyList<string?>` for the string guard, and index dedicated immutable-array overloads directly. This covers arrays, `List<T>`, and other common list shapes while retaining the receiver-preserving public signatures. Fall back to a direct `foreach`-style loop for arbitrary enumerable receivers and short-circuit on the first failure. Do not pre-count or call LINQ `Any` or `Count`; a lazy or stateful enumerable must be observed only once. Keep a zero-based position while inspecting and pass the discovered value/category and position to throw helpers; throw helpers must not append collection contents because doing so would re-enumerate a possibly one-shot input.

Reuse `ExistingItemException` for prohibited null and blank items, with focused throw helpers for the two message forms. Custom messages replace the generated message exactly as in existing guards. A null collection follows `MustNotBeNull`; custom factories are invoked for both a null receiver and invalid contents. A default immutable array is handled before enumeration and succeeds, consistent with `MustNotContain`; callers that also need initialization or non-emptiness can compose `MustNotBeDefaultOrEmpty`.

Add both names to `AssertionWhitelist` and `settings.json`, cover dependency reachability and factory removal in `SourceFileMergerWhitelistTests`, and regenerate `Light.GuardClauses.SingleFile.cs` for .NET Standard 2.0. Document the new families in the collections section of `docs/assertion-overview.md`, and put the enumeration behavior and complexity in each public method's XML remarks. Boolean predicate counterparts are out of scope: the requested value is the named throwing precondition, while `Any` already covers non-throwing inspection.
