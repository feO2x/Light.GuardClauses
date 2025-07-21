# Issue 120 - MustHaveMaximumLength for ImmutableArray

## Context

The .NET library Light.GuardClauses already has several assertions for collections. They often rely on `IEnumerable<T>` or `IEnumerable`. However, these assertions would result in `ImmutableArray<T>` being boxed - we want to avoid that by providing dedicated assertions for this type which avoids boxing. For this issue, we implement the `MustHaveMaximumLength` assertion for `ImmutableArray<T>`.

## Tasks for this issue

- [ ] The production code should be placed in the Light.GuardClauses project. There is no existing `Check.MustHaveMaximumLength.cs` file, but there is a `Check.MustHaveMaximumCount.cs` file. Create a new file called `Check.MustHaveMaximumLength.cs` in the root folder of the project.
- [ ] In this file, create several extension method overloads called `MustHaveMaximumLength` for `ImmutableArray<T>`. It should be placed in the class `Check` which is marked as `partial`.
- [ ] Each assertion in Light.GuardClauses has two overloads - the first one takes the optional `parameterName` and `message` arguments and throw the default exception. The actual exception is thrown in the `Throw` class - use the existing `Throw.InvalidMaximumCollectionCount` method which is located in `ExceptionFactory/Throw.InvalidMaximumCollectionCount.cs`.
- [ ] The other overload takes a delegate which allows the caller to provide their own custom exceptions. Use the existing `Throw.CustomException` method and pass the delegate, the erroneous `ImmutableArray<T>` instance and the maximum length.
- [ ] Use the `Length` property of `ImmutableArray<T>` instead of `Count` for performance and correctness.
- [ ] Create unit tests for both overloads. The corresponding tests should be placed in Light.GuardClauses.Tests project. There is an existing file 'CollectionAssertions/MustHaveMaximumCountTests.cs' but you need to create a new file 'CollectionAssertions/MustHaveMaximumLengthTests.cs' for length-related tests. Please follow conventions of the existing tests (e.g. use FluentAssertions' `Should()` for assertions).

## Notes

- There are already plenty of other assertions and tests in this library. All overloads are placed in the same file in the production code project. The test projects has top-level folders for different groups of assertions, like `CollectionAssertions`, `StringAssertions`, `DateTimeAssertions` and so on. Please take a look at them to follow a similar structure and code style.
- This assertion specifically targets `ImmutableArray<T>` to avoid boxing that would occur with generic `IEnumerable<T>` extensions.
- Use the `Length` property instead of `Count` as this is the appropriate property for `ImmutableArray<T>`.
- The assertion should verify that the `ImmutableArray<T>` does not exceed the specified maximum length.
- If you have any questions or suggestions, please ask me about them.
