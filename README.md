# Light.GuardClauses
**A lightweight .NET library for expressive Guard Clauses.** 

[![License](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)](https://github.com/feO2x/Light.GuardClauses/blob/master/LICENSE)
[![NuGet](https://img.shields.io/badge/NuGet-10.2.0-blue.svg?style=for-the-badge)](https://www.nuget.org/packages/Light.GuardClauses/)
[![Source Code](https://img.shields.io/badge/Source%20Code-10.2.0-blue.svg?style=for-the-badge)](https://github.com/feO2x/Light.GuardClauses/blob/master/Light.GuardClauses.SingleFile.cs)
[![Documentation](https://img.shields.io/badge/Docs-Wiki-yellowgreen.svg?style=for-the-badge)](https://github.com/feO2x/Light.GuardClauses/wiki)
[![Documentation](https://img.shields.io/badge/Docs-Changelog-yellowgreen.svg?style=for-the-badge)](https://github.com/feO2x/Light.GuardClauses/releases)

## Light.GuardClauses - easy precondition checks in C# / .NET

[Read the full docs in the Wiki](https://github.com/feO2x/Light.GuardClauses/wiki)

As a software developer, you're used to writing `if` statements at the beginning of your methods which validate the parameters that are passed in. Most often you'll probably check for null:

```csharp
public class Foo
{
    private readonly IBar _bar;
    
    public Foo(IBar? bar)
    {
        if (bar == null)
            throw new ArgumentNullException(nameof(bar));
        
        _bar = bar;
    }
}
```

**Light.GuardClauses** simplifies these precondition checks for you by providing extension methods that you can directly call on your parameters:

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

> As of version 10, Light.GuardClauses supports the [CallerArgumentExpressionAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callerargumentexpressionattribute) of C# 10. If your C# compiler is on a lower version, then the `CallerArgumentExpressionAttribute` might not be respected - you should then call all assertions with an explicit parameter name, e.g. `bar.MustNotBeNull(nameof(bar))`. You can use the C# 10 compiler by e.g. installing .NET 6 and/or Visual Studio 2022. The `CallerArgumentExpressionAttribute` is backwards-compatible, so it can be used with projects targeting .NET Standard 2, .NET Framework, or .NET Core.

By using **Light.GuardClauses**, you'll gain access to assertions for a vast amount of scenarios like checking strings, collections, enums, URIs, `DateTime`, `Type`, `IComparable<T>`, `IEnumerable`, `IEnumerable<T>`, and `Span<T>`. Just have a look at these examples:

```csharp
public class ConsoleWriter
{
    private readonly ConsoleColor _foregroundColor;

    public ConsoleWriter(ConsoleColor foregroundColor = ConsoleColor.Black) =>
        _foregroundColor = foregroundColor.MustBeValidEnumValue();
}
```

```csharp
public void SetMovieRating(Guid movieId, int numberOfStars)
{
    movieId.MustNotBeEmpty();
    numberOfStars.MustBeIn(Range.FromInclusive(0).ToInclusive(5));
    
    var movie = _movieRepo.GetById(movieId);
    movie.AddRating(numberOfStars);
}
```

```csharp
public class WebGateway
{
    private readonly HttpClient _httpClient;
    private readonly Uri _targetUrl;

    public WebGateway(HttpClient? httpClient, Uri? targetUrl)
    {
        _httpClient = httpClient.MustNotBeNull();
        _targetUrl = targetUrl.MustBeHttpOrHttpsUrl();
    }
}
```

In addition to assertions that throw exceptions (all these start with `Must`), **Light.GuardClauses** provides assertions that return a Boolean. Some examples are:
- `string.IsNullOrWhitespace()`
- `collection.IsNullOrEmpty()`
- `enum.IsValidEnumValue()`

You can use these in your branching logic to easily check if an assertion is true or false. 

Every assertion is well-documented - explore them using IntelliSense or check out [this overview](https://github.com/feO2x/Light.GuardClauses/wiki/Overview-of-All-Assertions).

## Light.GuardClauses is optimized

Since version 4.x, **Light.GuardClauses** is optimized for performance (measured in .NET 4.8 and .NET 6). With the incredible help of [@redknightlois](https://github.com/redknightlois) and the awesome tool [Benchmark.NET](https://github.com/dotnet/BenchmarkDotNet), most assertions are as fast as your imperative code would be.

**Light.GuardClauses** has support for [.NET analyzers / FxCopAnalyzers](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview) with the `ValidatedNotNullAttribute`. Analyzers will know when an assertion validated that a parameters is not null and consequently, CA1062 will not be raised. 

Furthermore, **Light.GuardClauses** has support for ReSharper since version 4.x. Via [Contract Annotations](https://www.jetbrains.com/help/resharper/Contract_Annotations.html), R# knows when assertions do not return a null value and thus removes squiggly lines indicating a possible `NullReferenceException`. Since version 10.1.0, Light.GuardClauses also annotates assertions that do not iterate over an `IEnumerable<T>` with ReSharper's `NoEnumerationAttribute` - ReSharper will then not indicate a "Possible multiple enumeration" (thanks to [cdonnellytx](https://github.com/cdonnellytx) for this contribution).

**Light.GuardClauses** supports C#8 Nullable Reference Types since version 8.0.

And, of course, the functional correctness of **Light.GuardClauses** is covered by a vast suite of automated tests.

## Supported Platforms

**Light.GuardClauses** is built against [.NET Standard 2.0 and 2.1](https://docs.microsoft.com/en-us/dotnet/standard/net-standard), thus it can be used in frameworks like .NET 6, .NET 7, .NET Framework 4.6.1 or newer, Unity, Mono, or UWP.

## How to Install

Light.GuardClauses is available as a [NuGet package](https://www.nuget.org/packages/Light.GuardClauses/).

- **dotnet CLI**: `dotnet add package Light.GuardClauses`
- **Visual Studio Package Manager Console**: `Install-Package Light.GuardClauses`
- **Package Reference in csproj**: `<PackageReference Include="Light.GuardClauses" Version="10.2.0" />`

Also, you can incorporate Light.GuardClauses as a **single source file** where the API is changed to `internal`. This is especially interesting for framework / library developers that do not want to have a dependency on the Light.GuardClauses DLL. You can grab the default .NET Standard 2.0 version in [Light.GuardClauses.SingleFile.cs](https://github.com/feO2x/Light.GuardClauses/blob/master/Light.GuardClauses.SingleFile.cs) or you can use the [Light.GuardClauses.SourceCodeTransformation](https://github.com/feO2x/Light.GuardClauses/tree/master/Code/Light.GuardClauses.SourceCodeTransformation) project to create your custom file. You can learn more about it [here](https://github.com/feO2x/Light.GuardClauses/wiki/Including-Light.GuardClauses-as-source-code).

## Let there be... Light
![Light Libraries Logo](https://cdn.githubraw.com/feO2x/Light.GuardClauses/main/Images/light_logo.png)
