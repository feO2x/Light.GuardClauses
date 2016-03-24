using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class SingleItemMustNotContainTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotContain must throw a CollectionException when the specified item is part of the collection.")]
        [MemberData(nameof(CollectionContainsItemData))]
        public void CollectionContainsItem<T>(T[] collection, T item)
        {
            Action act = () => collection.MustNotContain(item, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must not contain value \"{item.ToStringOrNull()}\", but it does.");
        }

        public static readonly IEnumerable<object[]> CollectionContainsItemData =
            new[]
            {
                new object[] { new[] { 1, 2, 3 }, 2 },
                new object[] { new[] { "Hello", "World" }, "Hello" },
                new object[] { new[] { "There is something", null }, null }
            };

        [Theory(DisplayName = "MustNotContain must not throw an exception when the specified item is not part of the collection.")]
        [MemberData(nameof(CollectionDoesNotContainItemData))]
        public void CollectionDoesNotContainItem<T>(T[] collection, T item)
        {
            Action act = () => collection.MustNotContain(item);

            act.ShouldNotThrow();
        }

        public static readonly IEnumerable<object[]> CollectionDoesNotContainItemData =
            new[]
            {
                new object[] { new[] { 'a', 'b', 'c' }, 'd' },
                new object[] { new[] { 42, 87, -188 }, 10555 },
                new object[] { new[] { true }, false }
            };

        [Fact(DisplayName = "MustNotContain must throw an ArgumentNullException when the specified collection is null.")]
        public void CollectionNull()
        {
            object[] collection = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => collection.MustNotContain("foo");

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustNotContain(2, exception: exception)));
            testData.Add(new CustomExceptionTest(exception => ((string[]) null).MustNotContain("Foo", exception: exception)));

            testData.Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 3 }.MustNotContain(2, message: message)));
            testData.Add(new CustomMessageTest<ArgumentNullException>(message => ((string[]) null).MustNotContain("Foo", message: message)));
        }
    }
}