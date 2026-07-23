# MustBePositive Parity for Older Target Frameworks

## Rationale

Parent issue #162 (point 5) identifies a target-framework gap in `MustBePositive`: the .NET 10 generic `INumber<T>` overload supports integral types such as `byte` and `ushort`, but the .NET Standard targets and their single-file source export expose only the existing concrete overloads. Consequently, BrilliantMessaging must express seven positive `byte` or `ushort` guards as the less-specific `MustBeGreaterThan(0)`.

Add concrete overloads for the complete missing signed and unsigned integral set so `MustBePositive` has a symmetric API on every supported target framework without changing the modern generic overloads.

## Acceptance Criteria

- [ ] `MustBePositive` has concrete overloads for `sbyte`, `byte`, `short`, `ushort`, `uint`, and `ulong` on .NET Standard 2.0, .NET Standard 2.1, and .NET 10; every overload returns the original value when it is greater than zero and throws `ArgumentOutOfRangeException` for zero and, for signed types, negative values.
- [ ] Each new integral type has a custom-exception-factory overload accepting `Func<T, Exception>`; it passes the original value to the factory, invokes the factory only when validation fails, and a null factory on a failing value throws `ArgumentNullException` via the existing `Throw.CustomException` convention.
- [ ] Default failures preserve the existing `MustBePositive` exception contract, including caller-argument-expression parameter names, optional custom messages, actual values, and the standard generated message.
- [ ] Automated tests cover positive boundary values, zero for every new type, negative boundary values for the signed types, return values, parameter-name and custom-message propagation, custom factory values and exceptions, factories not invoked on success, and null-factory behavior.
- [ ] The existing `MustBePositive` source-export whitelist entry includes all new concrete overloads in portable and modern exports, preserves the generic `INumber<T>` overloads only in the modern export, and continues to trim every custom-exception-factory overload when that option is disabled.
- [ ] The committed .NET Standard 2.0 single-file distribution is regenerated with the new overloads and validates for both supported source-export targets.
- [ ] The comparable/range documentation no longer describes the new concrete integral types as modern-target-only, and the package release notes mention the expanded `MustBePositive` support.
- [ ] The complete solution restores and builds without warnings in Release configuration, and all automated tests pass on the pinned SDK.

## Technical Details

Add the twelve overloads to the existing `Check.MustBePositive.cs` outside the `NET8_0_OR_GREATER` region, following the exact structure, annotations, XML documentation, and comparison semantics of the `int` and `long` overload pairs. For each `T` in `sbyte`, `byte`, `short`, `ushort`, `uint`, and `ulong`, the public shape is:

```csharp
public static T MustBePositive(
    this T parameter,
    [CallerArgumentExpression("parameter")] string? parameterName = null,
    string? message = null
);

public static T MustBePositive(this T parameter, Func<T, Exception> exceptionFactory);
```

Compare each value directly with its type-appropriate zero. Do not widen values to `int` or `long`: the returned value, exception `ActualValue`, and custom-factory argument must retain the receiver's exact type. The existing generic `Throw.MustBePositive<T>` and `Throw.CustomException<T>` helpers already support all six types, so no new exception or throw-helper API is needed.

Keep the `INumber<T>` overloads under `NET8_0_OR_GREATER`. On modern targets, normal calls with the six integral types resolve to the new concrete overloads while explicitly generic calls remain available; both paths must have the same greater-than-zero semantics. This issue does not expand the other sign-guard families or add native-integer overloads.

Extend `MustBePositiveTests` and `NumericCustomFactorySuccessTests` with the concrete integral matrix. Strengthen `SourceFileMergerWhitelistTests.SignGuardWhitelistsUseTargetSpecificSurface` to assert representative signed and unsigned overloads, exact factory signatures, portable omission of `INumber<T>`, modern retention of `INumber<T>`, and factory trimming. Regenerate `tools/source-export/Light.GuardClauses.SourceCodeTransformation/Light.GuardClauses.SingleFile.cs` using the committed source-export settings.
