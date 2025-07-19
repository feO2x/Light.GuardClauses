# Issue 114 - MustNotBeDefaultOrEmpty for ImmutableArray

## Context

The .NET library Light.GuardClauses already has several assertions for collections. They often rely on `IEnumerable<T>` or `IEnumerable`. However, these assertions would result in `ImmutableArray<T>` being boxed - we want to avoid that by providing dedicated assertions for this type which avoids boxing. For this issue, we start out with a new assertion called `MustNotBeDefaultOrEmpty`.

## Tasks for this issue

- [ ] The production code should be placed in the Light.GuardClauses project. Create a file called `Check.MustNotBeDefaultOrEmpty.cs` in the root folder of the project.
- [ ] In this file, create several extension method overloads called `MustNotBeDefaultOrEmpty` for `ImmutableArray<T>`. It should be placed in the class `Check` which is marked as `partial`.
- [ ] Each assertion in Light.GuardClauses has two overloads - the first one takes the optional `parameterName` and `message` arguments and throw the default exception (in this case the existing `EmptyCollectionException`). The actual exception in thrown in the `Throw` class, you need to create a corresponding method for it in this class.
- [ ] The other overload takes a delegate which allows the caller to provide their own custom exceptions. Pass the erroneous `ImmutableArray<T>` instance to the delegate and throw the returned exception.
- [ ] Create unit tests for both overloads. The corresponding file should be placed in Light.GuardClauses.Tests project, in the existing subfolder 'CollectionAssertions'. Please follow conventions of the existing tests (e.g. use FluentAssertions' `Should()` for assertions).

## Notes

- There are already plenty of other assertions and tests in this library. All overloads are placed in the same file in the production code project. The test projects has top-level folders for different groups of assertions, like `CollectionAssertions`, `StringAssertions`, `DateTimeAssertions` and so on. Please take a look at them to follow a similar structure and code style.
- If you have any questions or suggestions, please ask me about them.
