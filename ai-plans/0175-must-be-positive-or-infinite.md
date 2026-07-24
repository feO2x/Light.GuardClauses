# Positive Timeout or Infinite Sentinel

## Rationale

Parent issue #162 (point 6) identifies two BrilliantMessaging guards that accept either a positive `TimeSpan` or the standard .NET infinite-timeout sentinel. Callers can express this with a generic predicate, but doing so obscures the `Timeout.InfiniteTimeSpan` convention and produces a less-specific failure contract.

Add `MustBePositiveOrInfinite` as a fluent `TimeSpan` assertion on every supported target framework. The guard will make timeout validation explicit while retaining the return-value, exception, custom-factory, and source-export conventions of the existing sign guards.

## Acceptance Criteria

- [x] `MustBePositiveOrInfinite` returns the original `TimeSpan` when it is greater than `TimeSpan.Zero` or exactly equals `Timeout.InfiniteTimeSpan`, and rejects zero and every other negative value identically on .NET Standard 2.0, .NET Standard 2.1, and .NET 10.
- [x] Default failures throw `ArgumentOutOfRangeException` and preserve the existing sign-guard contract: caller-argument-expression parameter names, optional custom messages, and a generated message that identifies the rejected value and the two accepted forms.
- [x] A custom-exception-factory overload accepts `Func<TimeSpan, Exception>`, passes the original value to the factory, invokes it only for an invalid value, and throws `ArgumentNullException` through the existing `Throw.CustomException` convention when a failing check receives a null factory.
- [x] Automated tests cover positive boundary values, the exact infinite sentinel, zero, representative negative values on both sides of the sentinel, `TimeSpan.MinValue`, return values, default exception details, custom factory behavior, factories not invoked on success, and null-factory behavior.
- [x] The source-export whitelist catalog and committed settings contain `MustBePositiveOrInfinite`; focused source-export tests verify the assertion and its throw-helper dependency in portable and modern output, as well as trimming of the custom-exception-factory overload when configured.
- [x] The committed .NET Standard 2.0 single-file distribution is regenerated with the assertion and validates for both supported source-export targets.
- [x] The comparable/range assertion documentation lists `MustBePositiveOrInfinite` and defines “infinite” as exact equality with `Timeout.InfiniteTimeSpan`; the package release notes mention the new assertion.
- [x] The complete solution restores and builds without warnings in Release configuration, and all automated tests pass on the pinned SDK.

## Technical Details

Add a dedicated `TimeSpan` assertion following the structure, annotations, XML documentation, parameter ordering, and aggressive-inlining convention of the existing `MustBePositive(TimeSpan)` overloads. The exact public shape is:

```csharp
public static TimeSpan MustBePositiveOrInfinite(
    this TimeSpan parameter,
    [CallerArgumentExpression("parameter")] string? parameterName = null,
    string? message = null
);

public static TimeSpan MustBePositiveOrInfinite(
    this TimeSpan parameter,
    Func<TimeSpan, Exception> exceptionFactory
);
```

Use the predicate `parameter > TimeSpan.Zero || parameter == Timeout.InfiniteTimeSpan`. “Infinite” is deliberately limited to the BCL sentinel from `System.Threading`; it does not mean `TimeSpan.MaxValue` as a separate category (that value already succeeds because it is positive), nor does it admit arbitrary negative durations. Because equality is value-based, any `TimeSpan` equal to negative one millisecond is the sentinel regardless of how it was constructed. Values one tick above or below it remain negative and must fail.

Route default failures through a new non-returning `Throw.MustBePositiveOrInfinite` helper that constructs `ArgumentOutOfRangeException` consistently with `Throw.MustBePositive`; no new public exception type is needed. The factory overload should evaluate the same predicate and delegate failures to `Throw.CustomException`.

Register the assertion in `AssertionWhitelist` and the committed source-export settings, add focused source-export coverage for dependency retention and factory trimming, and regenerate the root `Light.GuardClauses.SingleFile.cs`. Add guard tests alongside the comparable assertions, and update `docs/assertion-overview.md` plus the current package release notes. No microbenchmarks are required because the guard adds only a comparison and an equality check.
