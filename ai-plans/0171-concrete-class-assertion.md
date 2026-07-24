# Concrete-Class Assertion

## Rationale

Parent issue #162 (point 4) identifies three BrilliantMessaging guards that require a handler type to be a class and not abstract. Callers currently have to combine reflection predicates with a generic assertion, which obscures the reusable intent and makes the failure contract inconsistent.

Add `MustBeConcreteClass` as a fluent `Type` assertion that uses the reflection definition `Type.IsClass && !Type.IsAbstract`, remains available on `netstandard2.0`, and composes with `MustBeAssignableTo` when a handler must also satisfy a base-type or interface constraint.

## Acceptance Criteria

- [x] `type.MustBeConcreteClass(parameterName, message)` returns the original `Type` when `type.IsClass` is true and `type.IsAbstract` is false, and throws `ArgumentException` otherwise, identically on .NET Standard 2.0, .NET Standard 2.1, and .NET 10; the exception exposes the caller-captured parameter name and honors an optional custom message.
- [x] The default overload throws `ArgumentNullException` for a null type, attributing the failure to the caller-captured parameter name; no new public exception type is introduced.
- [x] A custom-exception-factory overload accepts `Func<Type?, Exception>`, passes the original type to the factory, invokes it only when the type is null or is not a concrete class, and a null factory on a failing check throws `ArgumentNullException` via the existing `Throw.CustomException` convention.
- [x] Automated tests cover representative concrete classes and reflection edge cases accepted by the exact predicate; interfaces, abstract and static classes, value types, enums, and `void`; null input; return-value identity; parameter-name and custom-message propagation; the factory argument and concrete exception; no factory invocation on success; null-factory behavior; nullable-flow analysis; and composition with `MustBeAssignableTo`.
- [x] The source-export whitelist catalog and committed settings contain `MustBeConcreteClass`, and focused source-export tests cover retention of the assertion, its throw helper, and the one-argument custom-exception helper as well as trimming of the exception-factory overload when configured.
- [x] The committed .NET Standard 2.0 single-file distribution is regenerated with the assertion and validates for both supported source-export targets.
- [x] The type-relation assertion documentation lists `MustBeConcreteClass`, and the package release notes mention the new guard.
- [x] The complete solution restores and builds without warnings in Release configuration, and all automated tests pass on the pinned SDK.

## Technical Details

Add `Check.MustBeConcreteClass.cs` using the conventions of the other value-returning `Type` guards (aggressive inlining, JetBrains contract annotations, nullable-flow annotations, caller-argument-expression capture, and XML documentation). The exact public shape is:

```csharp
public static Type MustBeConcreteClass(
    [NotNull] [ValidatedNotNull] this Type? parameter,
    [CallerArgumentExpression("parameter")] string? parameterName = null,
    string? message = null
);

public static Type MustBeConcreteClass(
    [NotNull] [ValidatedNotNull] this Type? parameter,
    Func<Type?, Exception> exceptionFactory
);
```

After validating null in the default overload, the assertion must test `parameter.IsClass && !parameter.IsAbstract`. Treat those reflection properties as the complete contract: do not inspect constructors, `ContainsGenericParameters`, visibility, or other instantiability concerns. Consequently, ordinary sealed and unsealed classes, delegates, arrays, and open or closed generic classes follow the BCL reflection flags, while static classes fail because their emitted type is abstract.

Route default classification failures through a non-returning `Throw.MustBeConcreteClass` helper that constructs `ArgumentException` with the optional parameter name and a default message identifying the rejected type. The factory overload treats null and either failed predicate as validation failures and passes the original nullable value to `Throw.CustomException`. No microbenchmarks are required because the guard performs two constant-time reflection-property checks.

Register `MustBeConcreteClass` in `AssertionWhitelist` and `settings.json`, add focused `SourceFileMergerWhitelistTests`, add `MustBeConcreteClassTests` under the type assertions, extend the value-returning assertion coverage in `Issue72NotNullAttributeTests`, update the type-relation table in `docs/assertion-overview.md`, update the package release notes, and regenerate `tools/source-export/Light.GuardClauses.SourceCodeTransformation/Light.GuardClauses.SingleFile.cs` through the committed source-export settings.
