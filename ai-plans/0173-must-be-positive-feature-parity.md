# Integral Sign-Guard Feature Parity

## Rationale

Plan 0168 addressed parent issue #162 (point 5) by adding concrete `MustBePositive` overloads for the integral types previously covered only by the modern generic `INumber<T>` API. The other four sign-guard families remain asymmetric: their .NET Standard assets and portable source exports omit smaller signed integral types and, where the predicate remains meaningful, unsigned integral types, although the .NET 10 generic overloads already define their behavior.

Extend the concrete integral surface of `MustBeNegative`, `MustNotBeNegative`, `MustNotBePositive`, and `MustNotBeZero` on every supported target while preserving the existing generic APIs and exception contracts. Omit unsigned overloads whose predicate would be constant rather than adding APIs that either always fail or cannot validate anything.

## Acceptance Criteria

- [ ] `MustBeNegative` and `MustNotBeNegative` each have default and custom-exception-factory overloads for `sbyte` and `short`, but do not add concrete unsigned overloads whose checks would always fail or always succeed.
- [ ] `MustNotBePositive` and `MustNotBeZero` each have default and custom-exception-factory overloads for `sbyte`, `byte`, `short`, `ushort`, `uint`, and `ulong`.
- [ ] Every new overload is available on .NET Standard 2.0, .NET Standard 2.1, and .NET 10, returns the original exactly typed value when its predicate succeeds, and matches the corresponding existing .NET 10 generic semantics.
- [ ] Default failures preserve each guard family's existing `ArgumentOutOfRangeException` contract, including caller-argument-expression parameter names, optional custom messages, and the standard generated message containing the rejected value.
- [ ] Each new factory overload passes the original value with its exact integral type, invokes the factory only when validation fails, propagates the factory's exception, and throws `ArgumentNullException` through the existing `Throw.CustomException` convention when a failing check receives a null factory.
- [ ] Automated tests cover signed and applicable unsigned boundary behavior for every new overload, return values, exception parameter names and messages, factory arguments and exceptions, factories not invoked on success, and null-factory failures.
- [ ] Source-export tests verify the exact per-family concrete type matrix in portable and modern exports, including the deliberate omission of unsigned `MustBeNegative` and `MustNotBeNegative` overloads; generic `INumber<T>` overloads appear only in modern exports; and every custom-exception-factory overload is trimmed when that option is disabled.
- [ ] The committed .NET Standard 2.0 `Light.GuardClauses.SingleFile.cs` distribution is regenerated with the expanded APIs, and generated source validates for both supported source-export targets.
- [ ] The sign-guard documentation describes the per-family concrete integral surface, and the package release notes mention the expanded support across all five sign-guard families.
- [ ] The complete solution restores and builds without warnings in Release configuration, and all automated tests pass on the pinned SDK.

## Technical Details

Add the concrete overload pairs outside the `NET8_0_OR_GREATER` regions according to this matrix:

| Guard | New concrete types | Predicate |
| --- | --- | --- |
| `MustBeNegative` | `sbyte`, `short` | Less than zero |
| `MustNotBeNegative` | `sbyte`, `short` | Greater than or equal to zero |
| `MustNotBePositive` | `sbyte`, `byte`, `short`, `ushort`, `uint`, `ulong` | Less than or equal to zero |
| `MustNotBeZero` | `sbyte`, `byte`, `short`, `ushort`, `uint`, `ulong` | Not equal to zero |

Unsigned overloads are appropriate for `MustNotBePositive`, where zero succeeds and positive values fail, and `MustNotBeZero`, where zero fails and positive values succeed. Do not add concrete unsigned overloads to `MustBeNegative`, which could never succeed, or `MustNotBeNegative`, which could never fail.

For each listed guard and type, follow the annotations, XML documentation, parameter ordering, and implementation conventions of the corresponding existing `int` and `long` pair. The following is an illustrative shape for the concrete APIs, not a new generic API:

```csharp
public static T Guard(
    this T parameter,
    [CallerArgumentExpression("parameter")] string? parameterName = null,
    string? message = null
);

public static T Guard(this T parameter, Func<T, Exception> exceptionFactory);
```

Compare values directly to type-appropriate zero without widening to `int` or `long`, so return values, throw-helper inputs, and factory arguments retain their exact types. The generic throw helpers already support these types, so no new exception or `Throw` API is required.

Keep all `INumber<T>` overloads under `NET8_0_OR_GREATER`. Normal modern calls for types in the matrix should resolve to the new concrete overloads, while explicitly generic calls, deliberately omitted unsigned combinations, and numeric types such as `Half` remain supported by the generic surface with identical predicates. Retain focused tests for that distinction. Do not add `nint` or `nuint` overloads.

Extend the four guard-specific test classes and `NumericCustomFactorySuccessTests` with the new concrete matrix, retaining focused modern tests for explicitly generic calls and types without concrete overloads. Strengthen `SourceFileMergerWhitelistTests.SignGuardWhitelistsUseTargetSpecificSurface` to cover every sign-guard family and both factory settings, update `docs/assertion-overview.md` and `src/Directory.Build.props`, and regenerate the root `Light.GuardClauses.SingleFile.cs` through the committed source-export settings. No microbenchmarks are required because these overloads add only primitive comparisons equivalent to the existing generic behavior.
