using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotContainNullTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotContainNull must throw an exception when the specified collection contains at least one item that is null.")]
        [MemberData(nameof(CollectionWithNullData))]
        public void CollectionWithNull<T>(T[] collection, int erroneousIndex) where T : class
        {
            Action act = () => collection.MustNotContainNull(nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must be a collection not containing null, but you specified null at index {erroneousIndex}.");
        }

        public static readonly TestData CollectionWithNullData =
            new[]
            {
                new object[] { new[] { "Here", "Is", null }, 2 },
                new object[] { new[] { null, "1", "2", "3", "4" }, 0 },
                new object[] { new[] { new object(), null, new object() }, 1 }
            };

        [Theory(DisplayName = "MustNotContainNull must not thrown when the specified collection does not contain items that are null.")]
        [MemberData(nameof(CollectionsWithoutNullData))]
        public void CollectionsWithoutNull<T>(T[] collection) where T : class
        {
            Action act = () => collection.MustNotContainNull(nameof(collection));

            act.ShouldNotThrow();
        }

        public static readonly TestData CollectionsWithoutNullData =
            new[]
            {
                new object[] { new[] { "a", "b", "c" } },
                new object[] { new[] { new object(), new object() } }
            };

        [Fact(DisplayName = "MustNotContainNull must throw an ArgumentNullException when the specified collection is null.")]
        public void CollectionNull()
        {
            object[] collection = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => collection.MustNotContainNull(nameof(collection));

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(collection));
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new object[] { null }.MustNotContainNull(exception: exception)));
            testData.Add(new CustomExceptionTest(exception => new[] { "This collection contains null", null }.MustNotContainNull(exception: exception)));

            testData.Add(new CustomMessageTest<CollectionException>(message => new[] { "This collection contains null", null }.MustNotContainNull(message: message)));
        }
    }
}