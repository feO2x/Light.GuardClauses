using System;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    public sealed class MustNotContainTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotContain must throw a CollectionException when the specified item is part of the collection.")]
        [InlineData(new[] { "Hello", "World" }, "Hello")]
        [InlineData(new[] { "There is something", null }, null)]
        public void CollectionContainsItem(string[] collection, string item)
        {
            Action act = () => collection.MustNotContain(item, nameof(collection));

            act.Should().Throw<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must not contain value \"{item.ToStringOrNull()}\", but it does.");
        }

        [Fact(DisplayName = "MustNotContain must not throw an exception when the specified item is not part of the collection.")]
        public void CollectionDoesNotContainItem()
        {
            const char item = 'd';
            var collection = new[] { 'a', 'b', 'c' };

            var result = collection.MustNotContain(item);

            result.Should().BeSameAs(collection);
        }

        [Fact(DisplayName = "MustNotContain must throw an ArgumentNullException when the specified collection is null.")]
        public void CollectionNull()
        {
            object[] collection = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => collection.MustNotContain("foo");

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory(DisplayName = "MustNotContain must throw a CollectionException when the collection contains any of the items of the specified set.")]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2 })]
        [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
        [InlineData(new[] { 1, 42, 3 }, new[] { 42 })]
        public void SetContained(int[] collection, int[] set)
        {
            Action act = () => collection.MustNotContain(set, nameof(collection));

            act.Should().Throw<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must not contain any of the following values:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(set)}");
        }

        [Fact(DisplayName = "MustNotContain must not throw an exception when the collection does not contain any items of the specified set.")]
        public void SetNotContained()
        {
            var collection = new[] { "Hello", "There" };
            var set = new[] { "What's", "Up?" };

            var result = collection.MustNotContain(set);

            result.Should().BeSameAs(collection);
        }

        [Theory(DisplayName = "MustNotContain must throw an ArgumentNullException when the specified parameter or set is null.")]
        [InlineData(new object[0], null)]
        [InlineData(null, new object[0])]
        public void SetArgumentsNull(object[] collection, object[] set)
        {
            Action act = () => collection.MustNotContain(set);

            act.Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustNotContain(2, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 3 }.MustNotContain(2, message: message)));

            testData.Add(new CustomExceptionTest(exception => new[] { 'a', 'b' }.MustNotContain(new[] { 'a' }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 'a', 'b' }.MustNotContain(new[] { 'a' }, message: message)));
        }
    }
}