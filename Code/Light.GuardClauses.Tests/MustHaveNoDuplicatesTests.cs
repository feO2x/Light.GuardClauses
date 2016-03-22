using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustHaveNoDuplicatesTests
    {
        [Theory(DisplayName = "MustHaveUniqueItems must throw an exception when the collection has duplicates.")]
        [MemberData(nameof(DuplicateItemsTestData))]
        public void DuplicateItems<T>(T[] collection, int duplicateIndex)
        {
            Action act = () => collection.MustHaveNoDuplicates(nameof(collection));

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

        [Theory(DisplayName = "MustHaveUniqueItems must not throw an exception when the items are unique.")]
        [MemberData(nameof(UniqueItemsTestData))]
        public void UniqueItems<T>(T[] collection)
        {
            Action act = () => collection.MustHaveNoDuplicates(nameof(collection));

            act.ShouldNotThrow();
        }

        public static readonly TestData UniqueItemsTestData =
            new[]
            {
                new object[] { new[] { "41", "42", "43" } },
                new object[] { new object[] { 1, 2, 3, 4, 5 } }
            };

        [Fact(DisplayName = "The caller can specify a custom message that MustHaveUniqueItems must inject instead of the default one.")]
        public void CustomMessage()
        {
            const string message = "Thou shall have unique items!";

            Action act = () => new[] { 1, 2, 1 }.MustHaveNoDuplicates(message: message);

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain(message);
        }

        [Fact(DisplayName = "The caller can specify a custom exception that MustHaveUniqueItems must raise instead of the default one.")]
        public void CustomException()
        {
            var exception = new Exception();

            Action act = () => new[] { 1, 2, 1 }.MustHaveNoDuplicates(exception: exception);

            act.ShouldThrow<Exception>().Which.Should().BeSameAs(exception);
        }
    }
}