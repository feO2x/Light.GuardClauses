# Light.GuardClauses
**A lightweight library for expressive Guard Clauses with conditional compilation in C#.**  

## Why do I need it?

When you write methods with parameters in C# (including constructors), you for sure are used to Guard Clauses that check the validity of parameter values. The most common one is probably the null check where you throw an ArgumentNullException if an object reference does not point to an actual object. Light.GuardClauses provides a set of extension methods simplifying this task for you:

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

Light.GuardClauses provides extension methods for a lot of scenarios, e.g. for GUIDs and numeric values:

```csharp
public void SetMovieRating(Guid movieId, int numberOfStars)
{
    movieId.MustNotBeEmpty();
    numberOfStars.MustBeIn(Range<int>.FromInclusive(0).ToInclusive(5));
    
    var movie = _movieRepo.GetById(movieId);
    movie.AddRating(numberOfStars);
}
```

Inspired by [FluentAssertions](https://github.com/dennisdoomen/FluentAssertions), there are many more methods tailored for strings, IComparable<T>, IEnumerable<T>, IEquatable<T>, and IDictionary<T>. See a list of all of them [in the release notes](https://github.com/feO2x/Light.GuardClauses/releases) or discover them on the fly through IntelliSense - all the methods are fully documented. Just be sure to add the following `using` statement at the top of your code files to see the extension methods: `using Light.GuardClauses;`.

## Where do I get it?

[Download the assembly via NuGet](https://www.nuget.org/packages/Light.GuardClauses/): `Install-Package Light.GuardClauses` - Or use the code from this repo.

Light.GuardClauses is a portable library compatible for .NET 4.5 or later, Windows 8 / 8.1 / 10 Store Apps, Windows Phone 8.1 / Windows Phone 8 Silverlight, and .NET Core 1.0 (profile 259).

## And what's the difference to other assertion libraries?

Light.GuardClauses is specifically tailored for the scenario of creating precondition checks in production code. While the purpose of many other libraries is to provide a fluent syntax for assertions in automated tests, Light.GuardClauses does not provide these method-chaining capabilities, mainly for two reasons: to be high-performant as all checks involve only static method calls which create as less objects as possible (to keep the pressure on the Garbage Collector low), and to provide conditional compilation.

## Conditional Compilation

The methods of Light.GuardClauses are marked with the [ConditionalAttribute](https://msdn.microsoft.com/en-us/library/system.diagnostics.conditionalattribute(v=vs.110).aspx) so that you can include or exclude the calls to these methods when you build your project. To do that, go to the Build tab in the project properties and define the compilation symbol "COMPILE_ASSERTIONS" so that calls to Light.GuardClauses methods are included.

![Activating assertion compilation](/Images/compile_assertions.png)

Although you cannot use method chaining (because methods marked with the [ConditionalAttribute](https://msdn.microsoft.com/en-us/library/system.diagnostics.conditionalattribute(v=vs.110).aspx) cannot have return values), the ability to selectively include or exclude these precondition checks gives you a lot of flexibility regarding performance: during development, you can enable them to fail fast, and if you absolutely need the performance squeeze, you can disable them for your production deployment - this is perfectly in line with Bertrand Meyer's Design by Contract where you can also enable or disable assertions (by the way, read his book "Object-Oriented Software Construction" if you haven't - it's a necessary read for any O-O dev in my opinion).

## Customizing messages and exceptions

Every extension method of Light.GuardClauses has three optional parameters: **parameterName**, **message** and **exception**. With these, you can customize the outcome of an assertion:

* **parameterName** lets you inject the name of the actual parameter into the resulting exception message. By default, the standard exception messages use e.g. "The value" or "The string" to talk about the subject - these values are exchanged when you specify the name of the parameter.
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
* Create a static (extension) method that should have `void` as return type. Mark this method with the ConditionalAttribute and specify `Check.CompileAssertionsSymbol` to it.
* Apart from the parameters you need, add the three optional parameters **parameterName**, **message**, and **exception**. They should behave as mentioned above in the "Customizing messages and exceptions" section.
* Using the Conditional Operator (?:) and the Null-Coalescing-Operator (??) is recommended to check if the optional parameters are specified.

Check out the existing methods and the following template:

```csharp
[Conditional(Check.CompileAssertionsSymbol)
public static void *YourMethodName*(this *YourType* parameter, *Your other necessary parameters*, string parameterName = null, string message = null, Func<Exception> exception = null)
{
    if (*check something here*)
        throw exception != null ? exception() : new *YourExceptionType*(message ?? $"{parameterName ?? "The value"} must not be ...");
}
```

Your extension method is cool and you think other developers can benefit from it? Then send me a pull request and I'll check if it is useful to incorporate your method. Please provide tests and XML comments for your method, too.

## Writing tests for new extension methods

Light.GuardClauses uses [xunit.net](https://github.com/xunit/xunit) as the framework for automated tests and [FluentAssertions](https://github.com/dennisdoomen/FluentAssertions) for assertions within test methods. Please do not use any other frameworks when you want to integrate your own extension methods within the framework. Also, I would advise you to create one test class per assertion method.

Furthermore, the test project contains two checks that test all assertions methods using reflection:
* In the test class `CheckConditionalAttributeAppliance`, there is a Fact that ensures that all static methods in the Light.GuardClauses namespace are tagged with `Conditional(Check.CompileAssertionsSymbol)`. This way you cannot forget to mark your extension method with that attribute.
* Because writing tests for custom messages and custom exceptions for all extension methods is tedious, I created a little bit of infrastructure that does most of the heavy lifting for you. This infrastructure is implemented in `CustomMessagesAndCustomExceptionsTests` test class and the types in the sub-namespace `CustomMessagesAndExceptions`. All you have to do is implement the `ICustomMessageAndExceptionTestDataProvider` interface in your test class. In the only method `PopulateTestDataForCustomExceptionAndCustomMessageTests`of this interface, you get a `CustomMessageAndExceptionTestData` object as a parameter that you can use to populate as many `CustomExceptionTest` and `CustomMessageTest` instances as you like. Check the existing test classes or the example below to see how it's done. Just be aware that `CustomMessagesAndCustomExceptionsTests` also checks if your test class implements the said interface, and if not will tell you so in the test runs. If you do not implement an assertion extension method, you can blacklist your test class in the `OmmitedTestClasses` field.

A example of a test class looks like this:
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

To be frank, this library is still young, but the code base is thoroughly tested and I actively use it in my main work, the iRescYou research project. Please note that I might introduce breaking changes since we have not reached the status of a v1.0 yet, but honestely, I do think that these are just minor changes that you can solve in a blink using search and replace in your favorite IDE.

## In the end, what do I get?

Light.GuardClauses is a lightweight .NET solution for precondition checks, providing you with default exceptions for the most common cases and conditional compilation so that you can easily include or exclude your assertion calls when you build your project. This removes the clutter at the beginning of your parameterized methods.

If you need to, you can also customize every assertion by providing custom messages or even your own exceptions - this way you have full control over your assertions. And you can easily extend Light.GuardClauses with your own methods.

### Acknowledgements

Light.GuardClauses is developed as part of the "iRescYou" research project, conducted at the [University Hospital of Regensburg](http://www.uniklinikum-regensburg.de/e/index.php) and the [University of Applied Sciences Regensburg](https://www.oth-regensburg.de/en.html), funded by the [Bavarian State Ministry of Health Care](http://www.stmgp.bayern.de/).
