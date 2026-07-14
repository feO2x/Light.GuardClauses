# AGENTS.md for Testing

## How to structure tests

- Do not use mocking frameworks like Moq or NSubstitute for test doubles, use hand-written test doubles instead.
- Use FluentAssertions instead of xunit's `Assert` class. The library is pinned to 7.x.x to avoid licensing issues.
- Keep Test Coverage at least at 93%. Microsoft.Testing.Extensions.CodeCoverage is available to get test coverage metrics.
- Prefer Sociable Tests as proposed by Martin Fowler. Only use Solitary Tests as a last resort.

# How to run tests

- `dotnet test` for usual test runs.
- `dotnet test -- --coverage --coverage-output-format cobertura` for test coverage metrics.
