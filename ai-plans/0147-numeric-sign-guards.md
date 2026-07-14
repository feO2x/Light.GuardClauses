# Numeric Sign Guards

## Rationale

Guarding a numeric parameter against zero or a wrong sign is one of the most frequent preconditions in Line-of-Business code — quantities, monetary amounts, page sizes, retry counts, timeouts, and durations. Today callers must express these checks as `MustBeGreaterThan(0)` or `MustNotBeLessThan(TimeSpan.Zero)`, which works but states the boundary instead of the intent. Add dedicated sign guards that name the invariant directly: `MustBePositive`, `MustBeNegative`, `MustNotBePositive`, `MustNotBeNegative`, and `MustNotBeZero`.

The additions must preserve the library's fluent return values, exception-factory overloads, nullable annotations, broad target-framework support, Native AOT compatibility, and the customizable single-file source distribution.

## Acceptance Criteria

- [ ] The five throwing guard families `MustBePositive`, `MustBeNegative`, `MustNotBePositive`, `MustNotBeNegative`, and `MustNotBeZero` are available for `int`, `long`, `decimal`, `float`, `double`, and `TimeSpan` on .NET Standard 2.0, .NET Standard 2.1, and .NET 10.
- [ ] The .NET 10 asset additionally provides generic overloads of all five families constrained to `INumber<T>`, so types without concrete overloads (such as `short`, `byte`, or `Half`) are covered on the modern target.
- [ ] Sign semantics are defined by comparing the value against zero with the type's comparison operators: `MustBePositive` accepts only values greater than zero, `MustBeNegative` only values less than zero, `MustNotBePositive` only values less than or equal to zero, `MustNotBeNegative` only values greater than or equal to zero, and `MustNotBeZero` only values not equal to zero.
- [ ] For IEEE 754 floating-point inputs, `NaN` is rejected by all four sign guards and accepted by `MustNotBeZero`, positive and negative infinity satisfy the guards that match their sign, and negative zero — including `decimal`'s signed zero representations — is treated exactly like zero; these outcomes are identical on all targets and for both concrete and generic overloads.
- [ ] A failed guard throws `ArgumentOutOfRangeException` by default and reports the violated sign requirement and the actual value.
- [ ] Every new guard returns the successfully validated input, captures the guarded expression for the default exception, accepts an optional custom message, and provides an exception-factory overload consistent with the existing API.
- [ ] Automated tests cover successful values, failing values, zero and negative-zero boundaries (floating-point and `decimal`), `NaN`, infinity, and subnormal handling, `TimeSpan.Zero`, default exceptions, custom messages, custom exception factories, caller argument expressions, return values, and the .NET 10 generic overloads including at least one type without a concrete overload.
- [ ] The source-export whitelist catalog and committed settings contain the five new assertion families, and focused source-export tests cover the new entries.
- [ ] The committed .NET Standard 2.0 single-file distribution is regenerated and validates with the new portable API surface.
- [ ] The assertion overview documents the new families, including their zero, negative-zero, and `NaN` semantics and the .NET 10-only generic overloads.
- [ ] The complete solution restores and builds without warnings in Release configuration, and all automated tests pass on the pinned SDK.

## Technical Details

Follow the `IsFinite`/`MustBeFinite` pattern: one `Check.<Assertion>.cs` file per family containing concrete overloads for all targets, with the generic overloads added under the repository's existing modern-framework conditional (`NET8_0_OR_GREATER`). Illustrative shape of one family:

```csharp
public static int MustBePositive(
    this int parameter,
    [CallerArgumentExpression("parameter")] string? parameterName = null,
    string? message = null
);

public static int MustBePositive(this int parameter, Func<int, Exception> exceptionFactory);

#if NET8_0_OR_GREATER
public static T MustBePositive<T>(
    this T parameter,
    [CallerArgumentExpression("parameter")] string? parameterName = null,
    string? message = null
) where T : INumber<T>;
#endif
```

On the modern target the concrete overloads coexist with the generic ones; overload resolution prefers the concrete overload for its exact type, matching the established `IsFinite` and `IsApproximately` behavior. `TimeSpan` keeps concrete overloads on every target because it does not implement `INumber<TimeSpan>`; compare it against `TimeSpan.Zero`.

**Semantics.** Implement every check with the type's comparison operators against zero (`T.Zero` in the generic overloads). Do not use `IComparable<T>.CompareTo`, whose total order classifies `NaN` as less than zero and would make `MustBeNegative(double.NaN)` pass. Do not use `INumberBase<T>.IsPositive`/`IsNegative`, which use sign-bit semantics and disagree with the operator-based definition for negative zero and `NaN`. With operators, all four sign guards naturally reject `NaN` because every IEEE 754 comparison with `NaN` is false, and `-0.0` compares equal to zero. `MustNotBeZero` uses exact equality; document that tolerance-based comparisons remain the domain of the approximation guards. Positive and negative infinity are valid inputs that satisfy the guards matching their sign — rejecting non-finite values remains the job of `MustBeFinite`, and the two guards compose (`value.MustBeFinite().MustBePositive()`). `decimal`'s signed zero representations compare equal to zero and therefore behave exactly like zero. Unsigned types are only reachable through the generic overloads and need no special handling, even though `MustBeNegative` can never succeed for them.

**Boolean predicates are intentionally omitted.** The comparison operator itself is the predicate, and extension methods named `IsPositive`/`IsNegative` would collide with the framework's static `double.IsPositive`/`INumberBase<T>.IsNegative`, which follow the diverging sign-bit semantics — same name with different results for `-0.0` invites subtle bugs.

**Exceptions.** Add `Throw` helpers following the `Throw.NotFinite<T>` convention: generic, `[DoesNotReturn]`, throwing `ArgumentOutOfRangeException` with a message of the form `"{parameterName ?? "The value"} must be positive, but it actually is {parameter}."` (adjusted per family). No new public exception types are required.

**Portable scope.** The concrete overload set (`int`, `long`, `decimal`, `float`, `double`, `TimeSpan`) is deliberate: extension-method receivers do not apply implicit numeric conversions, so smaller integer types are simply not covered on the portable targets. This is accepted; the .NET 10 generic overloads close that gap on the modern target.

Update `AssertionWhitelist`, `settings.json`, and the source-export whitelist tests for the five new families with their exception-factory overloads, regenerate `Light.GuardClauses.SingleFile.cs` as the .NET Standard 2.0 output, and verify both portable and .NET 10 generated-source validation. Add the families to the comparable/range section of `docs/assertion-overview.md` and list the generic `INumber<T>` overloads under the target-specific API section.
