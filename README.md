# Light.GuardClauses

**A lightweight .NET library for expressive guard clauses.**

[![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Light.GuardClauses.svg?style=for-the-badge&label=NuGet)](https://www.nuget.org/packages/Light.GuardClauses/)
[![Source Code](https://img.shields.io/badge/Source%20Code-Single%20File-blue.svg?style=for-the-badge)](Light.GuardClauses.SingleFile.cs)
[![Documentation](https://img.shields.io/badge/Docs-Repository-yellowgreen.svg?style=for-the-badge)](docs/README.md)
[![Changelog](https://img.shields.io/badge/Docs-Changelog-yellowgreen.svg?style=for-the-badge)](https://github.com/feO2x/Light.GuardClauses/releases)

## Why Light.GuardClauses?

- 🧰 **130+ assertions** cover nullability, collections, text, numbers, ranges, dates, URIs, types, streams, and more.
- ⚡ **As fast as handwritten guards** — most assertions are optimized and benchmarked against equivalent imperative checks using a dedicated BenchmarkDotNet suite.
- 🏷️ **Automatic parameter names** - with C# 10 and newer, `CallerArgumentExpression` produce clear exceptions without repetitive `nameof` calls.
- 🔄 **Validation and assignment in one statement** works because throwing guards return the successfully validated value.
- 🧩 **Custom exception factories** let you control exception construction when the built-in exception does not fit your application.
- 🧠 **Tooling-aware contracts** support Nullable Reference Types for Roslyn, .NET code analysis, and JetBrains annotations.
- 🚀 **Flexible deployment** spans broad .NET compatibility, Native AOT, and optional single-file source inclusion.

Light.GuardClauses replaces repetitive parameter checks with expressive extension methods:

```csharp
public class Foo
{
    private readonly IBar _bar;

    public Foo(IBar? bar) => _bar = bar.MustNotBeNull();
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

Custom exception factories let you replace a guard's default exception with one that fits your application. Some factories receive the values involved in the validation:

```csharp
numberOfStars.MustBeIn(
    Range.InclusiveBetween(0, 5),
    (rating, range) => new InvalidOperationException(
        $"The rating {rating} is outside the allowed {range}."
    )
);
```

With C# 10 or newer, caller argument expressions automatically capture expressions such as `movieId` or `numberOfStars` for the exception parameter name. When using an older compiler, pass the name explicitly:

```csharp
movieId.MustNotBeEmpty(nameof(movieId));
```

See the [documentation index](docs/README.md), [assertion overview](docs/assertion-overview.md), and [usage guide](docs/structuring-precondition-checks.md) for more.

## Target frameworks

The NuGet package contains three target-framework assets. NuGet automatically selects the best compatible asset for the consuming project:

- **.NET Standard 2.0** provides the portable Light.GuardClauses API with the broadest runtime compatibility. It is selected for .NET Framework and other implementations that support .NET Standard 2.0 but not 2.1.
- **.NET Standard 2.1** provides the portable Light.GuardClauses API for newer .NET implementations. It is selected for .NET Core 3.0+, .NET 5–9, and other implementations that support .NET Standard 2.1.
- **.NET 10** provides the full API for .NET 10 and later, including generic-math overloads, additional span and memory overloads, trimming annotations, framework-optimized implementations, and declared Native AOT compatibility.

## Installation

Light.GuardClauses is available from [NuGet](https://www.nuget.org/packages/Light.GuardClauses/).

- .NET CLI: `dotnet add package Light.GuardClauses`
- Package Manager Console: `Install-Package Light.GuardClauses`
- Project file: `<PackageReference Include="Light.GuardClauses" />`

To embed the library without a DLL dependency, use the committed [.NET Standard 2.0 single-file distribution](Light.GuardClauses.SingleFile.cs) or create a tailored file with the [Light.GuardClauses.SourceCodeTransformation project](docs/source-code-inclusion.md).

## Design and quality

The library supports nullable reference types, .NET code-analysis attributes, JetBrains contract annotations, and Native AOT. Its functional behavior is covered by the test suite, and performance-sensitive assertions can be measured with the current [benchmark project](benchmarks/Light.GuardClauses.Performance/).

For the design history behind guard clauses and design by contract, see [Guard clause background](docs/guard-clause-background.md).

## Let there be Light!

![Light Libraries logo](Images/light_logo.png)
