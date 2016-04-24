using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustHaveMaximumCountTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustHaveMaximumCount must throw a CollectionException when the number of items in the collection exceeds the specified count.")]
        [MemberData(nameof(CountExceededData))]
        public void CountExceeded(IEnumerable<string> invalidCollection, int count)
        {
            Action act = () => invalidCollection.MustHaveMaximumCount(count, nameof(invalidCollection));

            // ReSharper disable PossibleMultipleEnumeration
            var collectionCount = invalidCollection.Count();
            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(invalidCollection)} must have no more than {count} {Items(count)}, but it actually has {collectionCount} {Items(collectionCount)}.");
            // ReSharper restore PossibleMultipleEnumeration
        }

        public static readonly TestData CountExceededData =
            new[]
            {
                new object[] { new[] { "Foo", "Bar" }, 1 },
                new object[] { new[] { "1", "2", "3", "4" }, 2 },
                new object[] { new[] { "Baz" }, 0 }
            };

        [Theory(DisplayName = "MustHaveMaximumCount must not throw an exception when the number of items does not exceed the specified count.")]
        [MemberData(nameof(CountNotExceededData))]
        public void CountNotExceeded(IEnumerable<int> collection, int count)
        {
            Action act = () => collection.MustHaveMaximumCount(count);

            act.ShouldNotThrow();
        }

        public static readonly TestData CountNotExceededData =
            new[]
            {
                new object[] { new[] { 88, 2, -5691, 21 }, 5 },
                new object[] { new[] { 102, 3 }, 2 },
                new object[] { new int[0], 0 }
            };

        private static string Items(int count)
        {
            return count == 1 ? "item" : "items";
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 3 }.MustHaveMaximumCount(0, exception: exception)));

            testData.Add(new CustomMessageTest<CollectionException>(message => new[] { 3 }.MustHaveMaximumCount(0, message: message)));
        }
    }
}