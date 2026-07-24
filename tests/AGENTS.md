# AGENTS.md for Testing

## How to structure tests

- Do not use mocking frameworks like Moq or NSubstitute for test doubles, use hand-written test doubles instead.
- Use FluentAssertions instead of xunit's `Assert` class. The library is pinned to 7.x.x to avoid licensing issues.
- Keep Test Coverage at least at 93%. Microsoft.Testing.Extensions.CodeCoverage is available to get test coverage metrics.
- Prefer Sociable Tests as proposed by Martin Fowler. Only use Solitary Tests as a last resort.

# How to run tests

- `dotnet test` for usual test runs.
- `dotnet test -- --coverage --coverage-output-format cobertura --coverage-settings CodeCoverage.config` for test coverage metrics (run from tests/Light.GuardClauses.Tests). The settings file excludes JetBrains.Annotations and Regex source generator output from the report.
- `dotnet test <project> --nologo` fails with "Zero tests ran" (exit code 5) because the Microsoft.Testing.Platform runner forwards `--nologo` to the test app, which rejects it. Run `dotnet test` without `--nologo`.
- Line coverage of 100% is not achievable: the closing brace after a call to a `Throw.*` helper is an unreachable sequence point because these helpers never return (285 such lines as of 2026-07-15).
