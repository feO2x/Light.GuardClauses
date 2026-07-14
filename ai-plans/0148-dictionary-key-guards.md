# Dictionary Key Guards

## Rationale

Line-of-Business code constantly guards lookup structures — settings dictionaries, translation tables, header maps — but `MustContain` and `MustNotContain` operate on collection items and substrings, so there is no assertion that targets dictionary keys. Versions before the v4 rewrite shipped `MustContainKey` and `MustNotContainKey` in the removed `DictionaryAssertions` class; reintroduce these two guards with the library's modern conventions. The broader legacy family (`MustContainValue`, `MustContainPair`, `MustBeKeyOf`, plural-keys overloads) is deliberately not resurrected: value and pair lookups are O(n) and rarely precondition-shaped, and they can be added later on demand.

The additions must preserve the library's fluent return values, exception-factory overloads, nullable annotations, broad target-framework support, Native AOT compatibility, and the customizable single-file source distribution. Key lookups must use `ContainsKey` so they never enumerate and honor the dictionary's own key comparer.

## Acceptance Criteria

- [x] `MustContainKey` and `MustNotContainKey` are available for `IReadOnlyDictionary<TKey, TValue>` receivers on .NET Standard 2.0, .NET Standard 2.1, and .NET 10 with identical semantics, and both type arguments are inferred at the call site without explicit specification.
- [x] Additional overloads for `Dictionary<TKey, TValue>` preserve and return the concrete dictionary shape, while receivers of other dictionary types implementing `IReadOnlyDictionary<TKey, TValue>` (such as `ConcurrentDictionary`, `SortedDictionary`, `ReadOnlyDictionary`, `ImmutableDictionary`, and `FrozenDictionary` on .NET 10) bind to the interface overloads without overload-resolution ambiguity.
- [x] The guards check key presence exclusively through `ContainsKey`, so they never enumerate the dictionary and respect its configured key comparer.
- [x] A null dictionary thrown into the default overloads produces the established `ArgumentNullException` null behavior; the exception-factory overloads pass the dictionary and the key to the factory.
- [x] A failed `MustContainKey` throws the new `MissingKeyException` and a failed `MustNotContainKey` throws the new `ExistingKeyException`; both derive from `CollectionException`, report the offending key, and follow the existing serialization conventions.
- [x] Every new guard returns the successfully validated input, captures the guarded expression for the default exception, accepts an optional custom message, and provides an exception-factory overload consistent with the existing API.
- [x] Automated tests cover present and absent keys, null dictionaries, comparer-sensitive lookups (for example an `OrdinalIgnoreCase` dictionary), default exceptions, custom messages, custom exception factories, caller argument expressions, returned values including the preserved `Dictionary<TKey, TValue>` shape, and successful binding for several dictionary types.
- [x] The source-export whitelist catalog and committed settings contain the two new assertion families with their exceptions and throw helpers, and focused source-export tests cover the new entries.
- [x] The committed .NET Standard 2.0 single-file distribution is regenerated and validates with the new portable API surface.
- [x] The assertion overview documents the new guards, including the shapes they accept and the `IDictionary<TKey, TValue>`-only limitation described below.
- [x] The complete solution restores and builds without warnings in Release configuration, and all automated tests pass on the pinned SDK.

## Technical Details

One `Check.<Assertion>.cs` file per family. The `MustContain`-style shape preservation via a `TCollection` type parameter is impossible here: C# type inference does not consult generic constraints, so a `TDictionary : class, IReadOnlyDictionary<TKey, TValue>` constraint leaves `TValue` non-inferable and would force explicit type arguments at every call site. The receivers are therefore interface-typed, with `TKey` and `TValue` inferred from the receiver conversion (exact signatures):

```csharp
public static IReadOnlyDictionary<TKey, TValue> MustContainKey<TKey, TValue>(
    [NotNull, ValidatedNotNull] this IReadOnlyDictionary<TKey, TValue>? parameter,
    TKey key,
    [CallerArgumentExpression("parameter")] string? parameterName = null,
    string? message = null
);

public static IReadOnlyDictionary<TKey, TValue> MustContainKey<TKey, TValue>(
    this IReadOnlyDictionary<TKey, TValue>? parameter,
    TKey key,
    Func<IReadOnlyDictionary<TKey, TValue>?, TKey, Exception> exceptionFactory
);

public static Dictionary<TKey, TValue> MustContainKey<TKey, TValue>(
    [NotNull, ValidatedNotNull] this Dictionary<TKey, TValue>? parameter,
    TKey key,
    [CallerArgumentExpression("parameter")] string? parameterName = null,
    string? message = null
) where TKey : notnull;
```

`MustNotContainKey` mirrors these shapes, and the `Dictionary<TKey, TValue>` overloads also get factory variants. The concrete overloads exist because the interface overloads erase the shape in fluent chains (`_map = map.MustNotBeNull().MustContainKey("endpoint");` must keep compiling when `map` and `_map` are `Dictionary<TKey, TValue>`); they coexist safely with the interface overloads because an identity conversion beats an interface conversion in overload resolution. The `notnull` constraint matches the BCL's `Dictionary<TKey, TValue>` annotation and avoids nullability warnings in the implementation.

**Why `IReadOnlyDictionary` and not `IDictionary` (or both).** A guard only reads, and every current BCL dictionary type implements `IReadOnlyDictionary<TKey, TValue>`. Offering overloads for both interfaces would make every call on a concrete dictionary type other than `Dictionary<TKey, TValue>` ambiguous (CS0121), because such types convert to both interfaces and neither conversion is better. Accepted limitation: a receiver statically typed as `IDictionary<TKey, TValue>` cannot use the guards; document `dictionary.Keys.MustContain(key)` as the workaround (the key collections of the BCL dictionaries implement `ICollection<TKey>.Contains` via `ContainsKey`, so this stays O(1)).

The key is passed to `ContainsKey` unmodified; dictionaries that reject null keys surface their own `ArgumentNullException`, and the guards add no redundant key-null check. Follow `MustContain`'s null handling for the dictionary itself: `[NotNull, ValidatedNotNull]`, `[ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]`, and the `MustNotBeNull` path in the default overloads.

**Exceptions.** Add `MissingKeyException` and `ExistingKeyException` deriving from `CollectionException`, mirroring `MissingItemException`/`ExistingItemException` including the `#if !NET8_0_OR_GREATER` serialization constructor. Add `Throw.MissingKey` and `Throw.ExistingKey` helpers following the `Throw.MissingItem` message conventions — for example `"{parameterName ?? "The dictionary"} must contain key {key.ToStringOrNull()}, but it actually does not."` — appending the dictionary's keys (not its key-value pairs) via the existing collection-content helper for the missing-key case.

Update `AssertionWhitelist`, `settings.json`, and the source-export whitelist tests for the two new families, ensure the new exceptions and throw helpers are reachable in the export, regenerate `Light.GuardClauses.SingleFile.cs` as the .NET Standard 2.0 output, and verify both portable and .NET 10 generated-source validation. Document the new guards in the collections section of `docs/assertion-overview.md`. No microbenchmarks are needed: the guards perform a single hash or tree lookup.
