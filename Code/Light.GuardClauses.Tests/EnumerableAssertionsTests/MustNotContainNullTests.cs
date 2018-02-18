using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    public sealed class MustNotContainNullTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotContainNull must throw an exception when the specified collection contains at least one item that is null.")]
        [InlineData(new[] { "Here", "Is", null }, 2)]
        [InlineData(new[] { null, "1", "2", "3", "4" }, 0)]
        public void CollectionWithNull(string[] collection, int erroneousIndex)
        {
            Action act = () => collection.MustNotContainNull(nameof(collection));

            act.Should().Throw<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must be a collection not containing null, but you specified null at index {erroneousIndex}.");
        }

        [Fact(DisplayName = "MustNotContainNull must not thrown when the specified collection does not contain items that are null.")]
        public void CollectionsWithoutNull()
        {
            var collection = new[] { "a", "b", "c" };

            var result = collection.MustNotContainNull();

            result.Should().BeSameAs(collection);
        }

        [Fact(DisplayName = "MustNotContainNull must throw an ArgumentNullException when the specified collection is null.")]
        public void CollectionNull()
        {
            object[] collection = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => collection.MustNotContainNull(nameof(collection));

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be(nameof(collection));
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new object[] { null }.MustNotContainNull(exception: exception)))
                    .Add(new CustomExceptionTest(exception => new[] { "This collection contains null", null }.MustNotContainNull(exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { "This collection contains null", null }.MustNotContainNull(message: message)));
        }
    }
}