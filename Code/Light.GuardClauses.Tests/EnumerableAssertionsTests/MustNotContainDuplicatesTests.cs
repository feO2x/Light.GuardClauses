using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    public sealed class MustNotContainDuplicatesTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotContainDuplicates must throw an exception when the collection has duplicates.")]
        [InlineData(new[] { "1", "2", "3", "1" }, 3)]
        [InlineData(new[] { "1", null, "1" }, 2)]
        public void DuplicateItems(string[] collection, int duplicateIndex)
        {
            Action act = () => collection.MustNotContainDuplicates(parameterName: nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must be a collection with unique items, but there is a duplicate at index {duplicateIndex}.");
        }

        [Fact(DisplayName = "MustNotContainDuplicates must not throw an exception when the items are unique.")]
        public void UniqueItems()
        {
            var collection = new[] { 1, 2, 3, 4, 5 };

            var result = collection.MustNotContainDuplicates(parameterName: nameof(collection));

            result.Should().BeSameAs(collection);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 1 }.MustNotContainDuplicates(exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 1 }.MustNotContainDuplicates(message: message)));
        }
    }
}