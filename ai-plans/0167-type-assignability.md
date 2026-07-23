# Type-Assignability Assertion

## Rationale

Parent issue #162 (point 3) identifies six BrilliantMessaging guards that reject a `Type` unless it can be assigned to a required base type or interface. The existing type-relation predicates do not provide a direct throwing assertion, so callers must translate the relationship into a boolean condition and choose the correct comparison direction themselves.

Add `MustBeAssignableTo` with the exact semantics of `Type.IsAssignableFrom`, expressed in the fluent direction `candidateType.MustBeAssignableTo(requiredType)`, and keep it available on `netstandard2.0`. Do not add `MustImplement`: the assignability guard already covers interfaces, while a second API would duplicate behavior and risk suggesting the open-generic equivalence semantics of the existing `Implements` predicate.

## Acceptance Criteria

- [x] `candidateType.MustBeAssignableTo(requiredType, parameterName, message)` returns the original candidate `Type` when `requiredType.IsAssignableFrom(candidateType)` is true and throws `ArgumentException` when it is false, identically on .NET Standard 2.0, .NET Standard 2.1, and .NET 10; the exception exposes the candidate parameter name and honors an optional custom message.
- [x] The default overload throws `ArgumentNullException` when either the candidate or required type is null, attributing a null candidate to the caller-captured parameter name and a null required type to `requiredType`; no new public exception type is introduced.
- [x] A custom-exception-factory overload accepts `Func<Type?, Type?, Exception>`, passes the original candidate and required types to the factory, invokes it only when either input is null or the assignability check fails, and a null factory on a failing check throws `ArgumentNullException` via the existing `Throw.CustomException` convention.
- [x] Automated tests cover identity, direct and indirect base-class relationships, interface implementation, value types, variant generics, and representative open-generic BCL semantics; reversed and unrelated failure cases; both null inputs; return-value identity; parameter-name and custom-message propagation; the factory arguments and concrete exception; no factory invocation on success; null-factory behavior; and nullable-flow analysis.
- [x] No `MustImplement` convenience assertion is added; interface assignability is documented and tested through `MustBeAssignableTo`.
- [x] The source-export whitelist catalog and committed settings contain `MustBeAssignableTo`, and focused source-export tests cover retention of the guard, its throw helper, and the two-argument custom-exception helper as well as trimming of the exception-factory overload when configured.
- [x] The committed .NET Standard 2.0 single-file distribution is regenerated with the assertion and validates for both supported source-export targets.
- [x] The type-relation assertion documentation lists `MustBeAssignableTo`, and the package release notes mention the new guard.
- [x] The complete solution restores and builds without warnings in Release configuration, and all automated tests pass on the pinned SDK.

## Technical Details

Add `Check.MustBeAssignableTo.cs` using the conventions of the other value-returning guards (aggressive inlining, JetBrains contract annotations, nullable flow annotations, caller-argument-expression capture, and XML documentation). The exact public shape is:

```csharp
public static Type MustBeAssignableTo(
    [NotNull] [ValidatedNotNull] this Type? parameter,
    [NotNull] [ValidatedNotNull] Type? requiredType,
    [CallerArgumentExpression("parameter")] string? parameterName = null,
    string? message = null
);

public static Type MustBeAssignableTo(
    [NotNull] [ValidatedNotNull] this Type? parameter,
    [NotNull] [ValidatedNotNull] Type? requiredType,
    Func<Type?, Type?, Exception> exceptionFactory
);
```

The check must evaluate `requiredType.IsAssignableFrom(parameter)`. This makes the direction explicit: after success, a value whose runtime type is `parameter` can be stored in a variable declared as `requiredType`. Use the BCL behavior verbatim, including equality, inheritance, interface implementation, array compatibility, and generic variance; do not route through `InheritsFrom`, `IsOrInheritsFrom`, or `IsEquivalentTypeTo`, whose constructed/open-generic handling is intentionally different. No generic `MustBeAssignableTo<T>` overload is included because the motivating scenario supplies the required type at runtime.

Route default relation failures through a non-returning `Throw.MustBeAssignableTo` helper that constructs `ArgumentException`. Its default message should state both types and the assignment direction without implying that the `Type` object itself failed a CLR cast. The default overload validates both type arguments before evaluating assignability; the factory overload treats either null as a validation failure and passes both original values to the factory. No trimming annotations or microbenchmarks are required because `Type.IsAssignableFrom` does not enumerate reflected members and this guard is a thin wrapper around the BCL operation.

Register `MustBeAssignableTo` in `AssertionWhitelist` and `settings.json`, add focused `SourceFileMergerWhitelistTests`, add `MustBeAssignableToTests` under the type assertions, extend the value-returning assertion coverage in `Issue72NotNullAttributeTests`, update the type-relation table in `docs/assertion-overview.md`, and regenerate `Light.GuardClauses.SingleFile.cs` through the source-export tool's committed settings.
