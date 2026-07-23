# Object-Disposed Condition Guard

## Rationale

Parent issue #162 identifies disposal guards as a recurring need (five call sites in BrilliantMessaging alone) that currently must be expressed as a generic boolean assertion with an exception factory, obscuring intent. Add `Check.ObjectDisposed` as a condition guard analogous to `Check.InvalidArgument`/`Check.InvalidOperation` that throws `ObjectDisposedException` when the condition is true, so disposal checks read explicitly and remain usable on `netstandard2.0`.

## Acceptance Criteria

- [x] `Check.ObjectDisposed(condition, objectName, message)` throws `ObjectDisposedException` only when `condition` is true, identically on .NET Standard 2.0, .NET Standard 2.1, and .NET 10; the supplied object name is exposed via `ObjectName`, the optional message via `Message`, and omitted arguments fall back to the BCL default message.
- [x] Two custom-exception-factory overloads follow the `InvalidArgument` convention (plain `Func<Exception>` and generic `Func<T, Exception>` receiving a caller-supplied parameter); they throw the factory's exception only when the condition is true, never invoke the factory otherwise, and a null factory on a true condition throws `ArgumentNullException` via the existing `Throw.CustomException` convention.
- [x] Automated tests cover: condition true/false, object-name and message propagation, the BCL default message, both factory overloads (including concrete custom exception instances and the passed parameter), factories not invoked on a false condition, and null-factory behavior.
- [x] The source-export whitelist catalog and committed settings contain `ObjectDisposed`, and focused source-export tests cover retention of the guard and its `Throw.ObjectDisposed` helper as well as trimming of the exception-factory overloads when configured.
- [x] The committed .NET Standard 2.0 single-file distribution is regenerated with the new guard and validates for both supported source-export targets.
- [x] The "Condition and state assertions" table in `docs/assertion-overview.md` lists `ObjectDisposed`, and the 15.0.0 package release notes in `src/Directory.Build.props` mention the new guard.
- [x] The complete solution restores and builds without warnings in Release configuration, and all automated tests pass on the pinned SDK.

## Technical Details

Add the guard as a new `Check.ObjectDisposed.cs` modeled directly on `Check.InvalidArgument.cs` (aggressive inlining, JetBrains contract annotations, XML documentation style), plus a non-returning `Throw.ObjectDisposed` helper in `ExceptionFactory/Throw.ObjectDisposed.cs` modeled on `Throw.InvalidOperation.cs` (`[ContractAnnotation("=> halt")]`, `[DoesNotReturn]`) whose single expression throws `new ObjectDisposedException(objectName, message)`. The exact public shape is:

```csharp
public static void ObjectDisposed(bool condition, string? objectName = null, string? message = null);

public static void ObjectDisposed(bool condition, Func<Exception> exceptionFactory);

public static void ObjectDisposed<T>(bool condition, T parameter, Func<T, Exception> exceptionFactory);
```

The generic factory overload mirrors `InvalidArgument`'s `Func<T, Exception>` form: callers pass arbitrary failure context (typically the disposed object or its name) without closure allocation. No caller-argument-expression capture applies because condition guards take a pre-evaluated `bool`, matching the other condition checks.

`ObjectDisposedException(string, string)` exists on all library targets and supplies the "Cannot access a disposed object." default when the message is null, so no custom exception type or message composition is introduced. No microbenchmarks: the guard is a trivial inlined condition check like `InvalidOperation`.

Append an `ObjectDisposed` entry to `AssertionWhitelist` and `settings.json` (after `MustStartWith`, preserving alphabetical position), add focused facts to `SourceFileMergerWhitelistTests` for dependency retention and factory-overload trimming, add `ObjectDisposedTests` under `tests/Light.GuardClauses.Tests/CommonAssertions/` following `InvalidOperationTests`/`InvalidArgumentTests` conventions (FluentAssertions, `DefaultVariablesData` where useful), and regenerate `Light.GuardClauses.SingleFile.cs` via the source-export project's committed settings.
