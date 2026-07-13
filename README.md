# Light.GuardClauses

**A lightweight .NET library for expressive guard clauses.**

[![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Light.GuardClauses.svg?style=for-the-badge&label=NuGet)](https://www.nuget.org/packages/Light.GuardClauses/)
[![Source Code](https://img.shields.io/badge/Source%20Code-Single%20File-blue.svg?style=for-the-badge)](Light.GuardClauses.SingleFile.cs)
[![Documentation](https://img.shields.io/badge/Docs-Repository-yellowgreen.svg?style=for-the-badge)](docs/README.md)
[![Changelog](https://img.shields.io/badge/Docs-Changelog-yellowgreen.svg?style=for-the-badge)](https://github.com/feO2x/Light.GuardClauses/releases)

Light.GuardClauses replaces repetitive parameter checks with expressive extension methods:

```csharp
public class Foo
{
    private readonly IBar _bar;

    public Foo(IBar? bar)
    {
        _bar = bar.MustNotBeNull();
    }
}
```

The guard returns the validated value, so validation and assignment can stay together. Throwing guards start with `Must`; Boolean checks such as `IsNullOrEmpty`, `IsValidEnumValue`, and `IsFileExtension` can be used in branching logic.

```csharp
public void SetMovieRating(Guid movieId, int numberOfStars)
{
    movieId.MustNotBeEmpty();
    numberOfStars.MustBeIn(Range.InclusiveBetween(0, 5));

    var movie = _movieRepo.GetById(movieId);
    movie.AddRating(numberOfStars);
}
```

See the [documentation index](docs/README.md), [assertion overview](docs/assertion-overview.md), and [usage guide](docs/structuring-precondition-checks.md) for more.

## Supported platforms

The NuGet package contains these assets:

| Target | Purpose |
| --- | --- |
| .NET Standard 2.0 | Broad compatibility, including implementations of .NET Standard 2.0 |
| .NET Standard 2.1 | Implementations of .NET Standard 2.1 |
| .NET 10 | The current .NET asset, including modern generic-math, span, and memory overloads and Native AOT compatibility |

Caller argument expressions are understood by C# 10 and newer compilers and automatically capture expressions such as `bar` for the exception parameter name. This is independent of the target framework. With an older C# compiler, pass the name explicitly:

```csharp
bar.MustNotBeNull(nameof(bar));
```

## Installation

Light.GuardClauses is available from [NuGet](https://www.nuget.org/packages/Light.GuardClauses/).

- .NET CLI: `dotnet add package Light.GuardClauses`
- Package Manager Console: `Install-Package Light.GuardClauses`
- Project file: `<PackageReference Include="Light.GuardClauses" />`

To embed the library without a DLL dependency, use the committed [.NET Standard 2.0 single-file distribution](Light.GuardClauses.SingleFile.cs) or create a tailored file with the [source exporter](docs/source-code-inclusion.md).

## Design and quality

The library supports nullable reference types, .NET code-analysis attributes, JetBrains contract annotations, and Native AOT. Its functional behavior is covered by the test suite, and performance-sensitive assertions can be measured with the current [benchmark project](benchmarks/Light.GuardClauses.Performance/).

For the design history behind guard clauses and design by contract, see [Guard clause background](docs/guard-clause-background.md).

![Light Libraries logo](Images/light_logo.png)
