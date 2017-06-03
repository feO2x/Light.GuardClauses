# Light.GuardClauses
**A lightweight .NET library for expressive Guard Clauses.** 

[![Video introduction to Light.GuardClauses](https://raw.githubusercontent.com/feO2x/Light.GuardClauses/master/Images/version2-video-logo.png)](https://youtu.be/wTDY_Gt46vU) 

## Why do I need it?

When you write methods with parameters in C# (including constructors), you for sure are used to Guard Clauses that check the validity of parameter values. The most common one is probably the null check where you throw an `ArgumentNullException` if an object reference does not point to an actual object. Light.GuardClauses provides a set of extension methods simplifying this task for you:

```csharp
public class Foo
{
    private readonly IBar _bar;
    
    public Foo(IBar bar)
    {
        // Perform a simple not-null-check using the MustNotBeNull extension method
        bar.MustNotBeNull();
        
        _bar = bar;
    }
}
```

Since v2.0.0, you can also directly use the return value of assertion methods to chain multiple assertions or, as in the following example, directly set the parameter to a field:

```csharp
public class Foo
{
    private readonly IBar _bar;
    
    public Foo(IBar bar)
    {
        // You can also use the return value of the assertion methods to e.g. directly set a field
        _bar = bar.MustNotBeNull();
    }
}
```

There is a vast variety of scenarios where Light.GuardClauses can support you, e.g. with GUIDs and numeric values:

```csharp
public void SetMovieRating(Guid movieId, int numberOfStars)
{
    movieId.MustNotBeEmpty();
    numberOfStars.MustBeIn(Range<int>.FromInclusive(0).ToInclusive(5));
    
    var movie = _movieRepo.GetById(movieId);
    movie.AddRating(numberOfStars);
}
```

Inspired by [FluentAssertions](https://github.com/dennisdoomen/FluentAssertions), there are many more methods tailored for strings, URIs, `DateTime`, `IComparable<T>`, `IEnumerable<T>`, `IEquatable<T>`, and `IDictionary<T>`. See a list of all of them [in the release notes](https://github.com/feO2x/Light.GuardClauses/releases) or discover them on the fly via IntelliSense - all the methods are fully documented. Just be sure to add the following `using` statement at the top of your code files to see the extension methods: `using Light.GuardClauses;`.

## Where do I get it?

[Download the assembly via NuGet](https://www.nuget.org/packages/Light.GuardClauses/): `Install-Package Light.GuardClauses` - Or use the code from this repo.

Light.GuardClauses is a .NET Standard 1.0 library since v2.0.0 (before it was a PCL with profile 259). This means it is compatible with e.g. .NET 4.5 or later, .NET Core 1.0 and UWP, Mono 4.6, Xamarin.iOS 10.0, Xamarin.Android 7.0, Windows 8 / 8.1 Store Apps, and Windows Phone 8.1 / Windows Phone 8 Silverlight.

## And what's the difference to other assertion libraries?

Light.GuardClauses is specifically tailored for the scenario of creating precondition checks in production code. While the purpose of many other libraries is to provide a fluent syntax for assertions in automated tests, Light.GuardClauses achieves the following two goals: 

1) Light.GuardClauses is high-performant as all checks involve only static method calls which create as less objects as possible (to keep the pressure on the Garbage Collector low)
2) Light.GuardClauses provides meaningful exception messages that you would expect from production code. Additionally, you can easily customize the message or the expception being thrown for every assertion call.

Check out the following section to learn how you can customize the resulting exceptions when precondition checks fail.

## Customizing messages and exceptions

Every extension method of Light.GuardClauses has three optional parameters: **parameterName**, **message** and **exception**. With these, you can customize the outcome of an assertion:

* **parameterName** lets you inject the name of the parameter being checked into the resulting exception message. By default, the standard exception messages use e.g. "The value", "The string", or "The URI" to talk about the subject - these parts are exchanged when you specify the name of the parameter. My advise is to not use `paramterName` when developing application-specific code, but if you create framework, library, or any other type of reusable code, it is better to use `parameterName` so that exception messages immediately point to the erroneous parameter.
* **message** lets you exchange the entire exception message if you are not satisfied with the default message in your current context.
* **exception** lets you specify a delegate creating an exception object that is thrown instead of the default exception.

```csharp
public class ConsoleWriter
{
    private readonly ConsoleColor _foregroundColor;

    public ConsoleWriter(ConsoleColor foregroundColor = ConsoleColor.Black)
    {
        foregroundColor.MustBeValidEnumValue(parameterName: nameof(foregroundColor));
        
        _foregroundColor = foregroundColor;
    }
}
```

```csharp
public class Entity
{
    public Guid Id { get; }
    
    public Entity(Guid id)
    {
        id.MustNotBeEmpty(message: "You cannot create an entity with an empty GUID.");
        
        Id = id;
    }
}
```

```csharp
public class CustomerController : ApiController
{
    private readonly ICustomerRepository _repo;
    
    public CustomerController(ICustomerRepository repo)
    {
        repo.MustNotBeNull(exception: () => new StupidTeamMembersException("Who is the idiot that forgot to register ICustomerRepository with the DI container?"));
    
        _repo = repo;
    }
}
```

## I can't find a suitable assertion method

If you can't find an assertion method that suits your current needs, you can always fall back to the `Check.That` and `Check.Against` methods that can be used in all circumstances:

```csharp
public void CompleteOrder()
{
    Check.That(_customerInfo.IsComlete,
               () => new InvalidOperationException("You cannot complete the order because some customer information is missing.");
               
    // Implemetation omitted to keep the example small
}
```
Of course, you can write your own extension methods, too.

## I want to extend Light.GuardClauses

If you want to write your own assertion method, you should follow these recommendations (which of course can be ignored when you only want to use them in your own solution):
* Create an extension method that should return the value that it is checking, i.e. the `this` parameter type should also be the return type to provide a fluent API.
* Apart from the parameters you need, add the three optional parameters **parameterName**, **message**, and **exception**. They should behave as mentioned above in the [Customizing messages and exceptions](https://github.com/feO2x/Light.GuardClauses#customizing-messages-and-exceptions) section.
* Using the Conditional Operator (?:) and the Null-Coalescing-Operator (??) is recommended to check if the optional parameters are specified.

Check out the existing methods in the source code of this repository and the following template:

```csharp
[Conditional(Check.CompileAssertionsSymbol)]
public static void *YourMethodName*(this *YourType* parameter, *Your other necessary parameters*, string parameterName = null, string message = null, Func<Exception> exception = null)
{
    if (*check something here*)
        throw exception != null ? exception() : new *YourExceptionType*(message ?? $"{parameterName ?? "The value"} must not be ...");
}
```

Your extension method is cool and you think other developers can benefit from it? Then send me a pull request and I'll check if it is useful to incorporate your method. Please provide tests and XML comments for your method, too.

## Writing tests for new extension methods

Light.GuardClauses uses [xunit.net](https://github.com/xunit/xunit) as the framework for automated tests and [FluentAssertions](https://github.com/dennisdoomen/FluentAssertions) for assertions within test methods. Please do not use any other frameworks when you want to integrate your own extension methods within Light.GuardClauses.Tests. Also, I would advise you to create one test class per assertion method.

Furthermore, the test project contains two checks that test all assertion methods using reflection:
* In the test class `FluentApiTests`, there is a Theory that ensures that all extension method that start with "Must\*" in the Light.GuardClauses namespace return the value that they check. This way you cannot forget to provide a fluent API with your new extension method. If for some reason your method cannot provide a fluent API, you can update the `omittedMethods` variable in the constructor of that class.
* Because writing tests for custom messages and custom exceptions for all extension methods is tedious, I created a little bit of infrastructure that does most of the heavy lifting for you. This infrastructure is implemented in  `CustomMessagesAndCustomExceptionsTests` and the types in the sub-namespace `CustomMessagesAndExceptions`. All you have to do is implement the `ICustomMessageAndExceptionTestDataProvider` interface in your own test class. In the only method `PopulateTestDataForCustomExceptionAndCustomMessageTests`of this interface, you get a `CustomMessageAndExceptionTestData` object as a parameter that you can use to populate as many `CustomExceptionTest` and `CustomMessageTest` instances as you like. Check the existing test classes or the example below to see how it's done. Just be aware that `CustomMessagesAndCustomExceptionsTests` also checks if your test class implements the said interface, and if not will tell you so in the test runs. If you do not implement an assertion extension method, you can whitelist your test class in the `OmmitedTestClasses` field.

An example of a test class looks like this:
```csharp
public sealed class MustBeSameAsTests : ICustomMessageAndExceptionTestDataProvider
{
    [Theory(DisplayName = "MustBeSameAs must throw an ArgumentException when the two specified references do not point to the same instance.")]
    [InlineData("Hello", "World")]
    [InlineData("1", "2")]
    [InlineData(new object[] { }, new object[] { "Foo" })]
    public void ReferencesDifferent<T>(T first, T second) where T : class
    {
        Action act = () => first.MustBeSameAs(second, nameof(first));

        act.ShouldThrow<ArgumentException>()
            .And.Message.Should().Contain($"{nameof(first)} must point to the object instance \"{second}\", but it does not.");
    }

    [Theory(DisplayName = "MustBeSameAs must not throw an exception when the two specified references point to the same instance.")]
    [InlineData("Foo")]
    [InlineData("Bar")]
    public void ReferencesEqual(string reference)
    {
        Action act = () => reference.MustBeSameAs(reference);

        act.ShouldNotThrow();
    }

    public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
    {
        testData.Add(new CustomExceptionTest(exception => "foo".MustBeSameAs("bar", exception: exception)));

        testData.Add(new CustomMessageTest<ArgumentException>(message => "foo".MustBeSameAs("bar", message: message)));
    }
}
```

## Is it ready for production?

Since the beginning of June 2016, the library is in v1.x and stable. Light.GuardClauses is thoroughly covered with automated tests and I actively use it in my daily work.

## In the end, what do I get?

Light.GuardClauses is a lightweight, high-performance .NET solution for precondition checks, providing you with sensible default exception messages and the ability to easily customize them. This removes the clutter at the beginning of your parameterized methods. Additionally, you can easily extend Light.GuardClauses with your own assertions.

### Acknowledgements

Light.GuardClauses was initially developed as part of the [iRescYou research project](http://www.irescyou.de/), conducted at the [University Hospital of Regensburg](http://www.uniklinikum-regensburg.de/e/index.php) and the [University of Applied Sciences Regensburg](https://www.oth-regensburg.de/en.html), funded by the [Bavarian State Ministry of Health and Care](http://www.stmgp.bayern.de/).

Thanks to [xunit.net](https://github.com/xunit/xunit) and [FluentAssertions](https://github.com/dennisdoomen/FluentAssertions) - I use these frameworks every day!
And [Visual Studio](https://www.visualstudio.com/) in combination with [R#](https://www.jetbrains.com/dotnet/) is awesome!

### Let there be... Light
![Light Libraries Logo](/Images/light_logo.png)
