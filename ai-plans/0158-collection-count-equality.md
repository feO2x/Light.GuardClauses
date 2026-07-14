# Collection Count Equality

## Rationale

Parallel collections such as key/value arrays and correlated batch inputs must often contain the same number of items. Callers currently have to obtain and compare both counts manually, losing Light.GuardClauses' standard null handling, exception contract, caller-expression capture, and fluent return value. Add `MustHaveSameCountAs` as the cross-collection counterpart to `MustHaveCount`.

The guard must support collections with different element and concrete types, preserve the receiver's concrete type, reuse the existing optimized count infrastructure, and remain safe for lazy or single-use enumerables.

## Acceptance Criteria

- [x] `MustHaveSameCountAs` is available with identical semantics on .NET Standard 2.0, .NET Standard 2.1, and .NET 10 for any two reference collections implementing `IEnumerable`, including collections with different element types.
- [x] The guard accepts equal counts, including two empty collections, and returns the original receiver in its concrete type without comparing collection contents.
- [x] A null receiver or comparison collection throws `ArgumentNullException` by default; unequal counts throw `InvalidCollectionCountException` whose parameter name identifies the receiver and whose default message reports both observed counts.
- [x] The default overload captures the receiver expression, accepts an optional custom message, and the custom-exception-factory overload receives both original nullable collections and is used for null inputs as well as unequal counts.
- [x] Count-property fast paths supported by the existing helpers do not enumerate their inputs; each arbitrary enumerable is enumerated at most once per invocation, its enumerator is disposed, and failure-message construction does not enumerate either input again.
- [x] Automated tests cover equal and unequal counts; empty inputs; different collection and element types; null values in both argument positions; default exception details; custom messages and exception factories; caller argument expressions; fluent concrete-type preservation; count-property fast paths; same-instance inputs; and distinct lazy/single-use enumerables.
- [x] The source-export whitelist catalog and committed settings contain `MustHaveSameCountAs`, and focused source-export tests cover its count-helper, throw-helper, exception, and custom-factory reachability and trimming.
- [x] The committed .NET Standard 2.0 single-file distribution is regenerated with the new guard and validates for both supported source-export targets.
- [x] XML documentation and the assertion overview describe the cross-collection semantics, supported receiver shapes, null and failure behavior, fluent return value, and enumeration characteristics.
- [x] The complete solution restores and builds without warnings in Release configuration, and all automated tests pass on the pinned SDK.

## Technical Details

Add the assertion as a new `Check.MustHaveSameCountAs.cs` family. Use separate generic parameters for the receiver and comparison collection so a call such as `keys.MustHaveSameCountAs(values)` works when the arrays have different element types while retaining the exact receiver type. The intended public shape is:

```csharp
public static TCollection MustHaveSameCountAs<TCollection, TOtherCollection>(
    [NotNull, ValidatedNotNull] this TCollection? parameter,
    [NotNull, ValidatedNotNull] TOtherCollection? otherCollection,
    [CallerArgumentExpression("parameter")] string? parameterName = null,
    string? message = null
) where TCollection : class, IEnumerable
  where TOtherCollection : class, IEnumerable;

public static TCollection MustHaveSameCountAs<TCollection, TOtherCollection>(
    [NotNull, ValidatedNotNull] this TCollection? parameter,
    [NotNull, ValidatedNotNull] TOtherCollection? otherCollection,
    Func<TCollection?, TOtherCollection?, Exception> exceptionFactory
) where TCollection : class, IEnumerable
  where TOtherCollection : class, IEnumerable;
```

Apply the established aggressive-inlining, nullable-flow, and JetBrains contract annotations. Validate both references before observing either collection; the receiver retains the captured caller expression, while a null comparison collection follows the repository convention of naming the secondary argument itself. The custom factory is invoked once and receives the original values when either reference is null or the counts differ. Comparing a collection with itself is a tautology and should return immediately after null validation, without enumerating it.

Obtain each count through `FrameworkExtensions.EnumerableExtensions.Count`, not LINQ. This retains the existing O(1), allocation-free paths for non-generic `ICollection` receivers and strings and falls back to enumeration for other `IEnumerable` implementations. Store both counts and pass them to a focused non-returning throw helper that creates `InvalidCollectionCountException`; do not call the existing collection-rendering/counting failure path if it would recount an input. The generated message should state that the receiver must have the same count as the comparison collection and include both precomputed counts without formatting either collection.

Keep this family scoped to reference `IEnumerable` collections, matching `MustHaveCount`. Span, memory, and `ImmutableArray<T>` values use the library's length terminology and are not part of this request; no item-type equality constraint or content comparison should be introduced.

Add the assertion to `AssertionWhitelist` and `settings.json`, verify dependency reachability and removal of its factory overload when configured, and regenerate `Light.GuardClauses.SingleFile.cs` as the portable artifact. Add the guard to the collections table in `docs/assertion-overview.md`; detailed complexity and single-pass behavior belong in the public XML remarks.
