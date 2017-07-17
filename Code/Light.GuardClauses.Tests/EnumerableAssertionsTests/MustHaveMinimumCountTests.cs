using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustHaveMinimumCountTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustHaveMinimumCount must throw a CollectionException when the collection does not have the specified count.")]
        [MemberData(nameof(LessItemsData))]
        public void LessItems(IEnumerable<string> invalidCollection, int count)
        {
            Action act = () => invalidCollection.MustHaveMinimumCount(count, nameof(invalidCollection));

            var collectionCount = invalidCollection.Count();
            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(invalidCollection)} must have at least {count} {Items(count)}, but it actually has {collectionCount} {Items(collectionCount)}.");
        }

        public static readonly TestData LessItemsData =
            new[]
            {
                new object[] { new List<string> { "Foo", "Bar" }, 3 },
                new object[] { new List<string>(), 1 },
                new object[] { new List<string> { "Hey", "There", "What's up?" }, 50 }
            };

        [Theory(DisplayName = "MustHaveMinimumCount must not throw an exception when the collection has equal or more items than the specified count.")]
        [MemberData(nameof(EnoughItemsData))]
        public void EnoughItems(IEnumerable<int> validCollection, int count)
        {
            Action act = () => validCollection.MustHaveMinimumCount(count);

            act.ShouldNotThrow();
        }

        public static readonly TestData EnoughItemsData =
            new[]
            {
                new object[] { new[] { 1, 2, 3, 4 }, 2 },
                new object[] { new int[0], 0 },
                new object[] { new List<int> { 42, 145, 3000 }, 3 }
            };

        private static string Items(int count)
        {
            return count == 1 ? "item" : "items";
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new List<bool>().MustHaveMinimumCount(42, exception: exception)));

            testData.Add(new CustomMessageTest<CollectionException>(message => new List<object>().MustHaveMinimumCount(1, message: message)));
        }
    }
}