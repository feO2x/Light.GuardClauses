# Issue 119 - MustContain for ImmutableArray

## Context

The .NET library Light.GuardClauses already has several assertions for collections. They often rely on `IEnumerable<T>` or `IEnumerable`. However, these assertions would result in `ImmutableArray<T>` being boxed - we want to avoid that by providing dedicated assertions for this type which avoids boxing. For this issue, we implement the `MustContain` assertion for `ImmutableArray<T>`.

## Tasks for this issue

- [ ] The production code should be placed in the Light.GuardClauses project. Extend the existing `Check.MustContain.cs` in the root folder of the project.
- [ ] In this file, add several extension method overloads called `MustContain` for `ImmutableArray<T>`. They should be placed in the existing class `Check` which is marked as `partial`.
- [ ] Each assertion in Light.GuardClauses has two overloads - the first one takes the optional `parameterName` and `message` arguments and throws the default exception (in this case the existing `MissingItemException`). The actual exception is thrown in the `Throw` class, you need to call the existing method for it in this class.
- [ ] The other overload takes a delegate which allows the caller to provide their own custom exceptions. Use the existing `Throw.CustomException` method and pass the delegate, the erroneous `ImmutableArray<T>` instance and the missing item.
- [ ] Create unit tests for both overloads. The corresponding tests should be placed in Light.GuardClauses.Tests project, in the existing file 'CollectionAssertions/MustContainTests.cs'. Please follow conventions of the existing tests (e.g. use FluentAssertions' `Should()` for assertions).

## Notes

- There are already plenty of other assertions and tests in this library. All overloads are placed in the same file in the production code project. The test projects has top-level folders for different groups of assertions, like `CollectionAssertions`, `StringAssertions`, `DateTimeAssertions` and so on. Please take a look at them to follow a similar structure and code style.
- This assertion specifically targets `ImmutableArray<T>` to avoid boxing that would occur with generic `IEnumerable<T>` extensions.
- There are already plenty of other assertions and tests in this library. All overloads are placed in the same file in the production code project. The test projects has top-level folders for different groups of assertions, like `CollectionAssertions`, `StringAssertions`, `DateTimeAssertions` and so on. Please take a look at them to follow a similar structure and code style.
- The assertion should verify that the `ImmutableArray<T>` contains the specified item.
- If you have any questions or suggestions, please ask me about them.
