# Structuring precondition checks

[Documentation index](README.md) · [Contributing and building](contributing-and-building.md) · **Structuring checks** · [Source inclusion](source-code-inclusion.md) · [Assertions](assertion-overview.md) · [Historical performance](historical-performance.md) · [Background](guard-clause-background.md)

## Default exceptions, parameter names, and messages

Most guards whose names start with `Must` have one overload that throws the library's default exception and another that accepts an `exceptionFactory` for a custom exception. Default guard exceptions generally derive from `ArgumentException`, directly or indirectly; the precise contract is documented on each overload.

The default overload usually accepts optional `parameterName` and `message` arguments:

```csharp
public sealed class Entity
{
    public Guid Id { get; }

    public Entity(Guid id) =>
        Id = id.MustNotBeEmpty(
            message: "An entity cannot be created with an empty identifier."
        );
}
```

With a C# 10 or newer compiler, `[CallerArgumentExpression]` captures the expression passed as `parameter`, so an ordinary call such as `id.MustNotBeEmpty()` reports `id` automatically. The feature depends on the consuming compiler, not the target framework. When using an older compiler, pass `parameterName` explicitly:

```csharp
id.MustNotBeEmpty(nameof(id));
```

## Custom exceptions

Pass an exception factory when the surrounding application needs a domain-specific exception:

```csharp
public CustomerService(ICustomerRepository? repository)
{
    _repository = repository.MustNotBeNull(
        () => new InvalidOperationException("The customer repository is not configured.")
    );
}
```

Some factories receive the invalid value or other comparison inputs. This makes it possible to construct a useful exception without a closure:

```csharp
public void RequestCustomerInformation(Uri? uri)
{
    uri.MustBeHttpsUrl(
        invalidUri => new InvalidOperationException(
            $"The endpoint '{invalidUri}' must use HTTPS."
        )
    );
}
```

When a guard performs several checks, its custom factory is used for every failure path. For example, `MustBeHttpsUrl` uses the supplied factory whether the URI is null, relative, or has the wrong scheme. Consult the XML documentation to see which arguments a particular factory receives.

## Extension or static-call style

Every assertion is a member of the static `Check` class and is normally exposed as an extension method:

```csharp
_bar = bar.MustNotBeNull();
```

The equivalent static call is available when it reads more clearly:

```csharp
_bar = Check.MustNotBeNull(bar);
```

Throwing guards return the successfully validated value, which allows validation and assignment in one expression. They can also be used as statements:

```csharp
Check.MustNotBeNull(bar);
_bar = bar;
```

Use the form that makes the calling code easiest to understand. A direct `if` statement remains appropriate when a library guard would obscure the intent.
