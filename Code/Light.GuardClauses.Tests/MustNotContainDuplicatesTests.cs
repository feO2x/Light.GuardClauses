using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustNotContainDuplicatesTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotContainDuplicates must throw an exception when the collection has duplicates.")]
        [MemberData(nameof(DuplicateItemsTestData))]
        public void DuplicateItems<T>(T[] collection, int duplicateIndex)
        {
            Action act = () => collection.MustNotContainDuplicates(nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must be a collection with unique items, but there is a duplicate at index {duplicateIndex}.");
        }

        public static readonly TestData DuplicateItemsTestData =
            new[]
            {
                new object[] { new[] { "1", "2", "3", "1" }, 3 },
                new object[] { new object[] { 1, 42, 42, 87 }, 2 },
                new object[] { new[] { "1", null, "1" }, 2 }
            };

        [Theory(DisplayName = "MustNotContainDuplicates must not throw an exception when the items are unique.")]
        [MemberData(nameof(UniqueItemsTestData))]
        public void UniqueItems<T>(T[] collection)
        {
            Action act = () => collection.MustNotContainDuplicates(nameof(collection));

            act.ShouldNotThrow();
        }

        public static readonly TestData UniqueItemsTestData =
            new[]
            {
                new object[] { new[] { "41", "42", "43" } },
                new object[] { new object[] { 1, 2, 3, 4, 5 } }
            };

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 1 }.MustNotContainDuplicates(exception: exception)));
            testData.Add(new CustomExceptionTest(exception =>
                                                 {
                                                     string[] array = null;
                                                     // ReSharper disable once ExpressionIsAlwaysNull
                                                     array.MustNotContainDuplicates(exception: exception);
                                                 }));

            testData.Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 1 }.MustNotContainDuplicates(message: message)));
            testData.Add(new CustomMessageTest<ArgumentNullException>(message =>
                                                                      {
                                                                          string[] array = null;
                                                                          // ReSharper disable once ExpressionIsAlwaysNull
                                                                          array.MustNotContainDuplicates(message: message);
                                                                      }));
        }
    }
}