using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    public sealed class MustContainTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustContain must throw a CollectionException when the specified value is not part of the collection.")]
        public void CollectionDoesNotContainValue()
        {
            var collection = new[] { 1, 2, 3 };
            const int value = 42;

            Action act = () => collection.MustContain(value, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must contain value \"{value}\", but does not.");
        }

        [Fact(DisplayName = "MustContain must throw an ArgumentNullException when the specified collection is null.")]
        public void CollectionNull()
        {
            IReadOnlyCollection<string> collection = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => collection.MustContain("foo");

            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact(DisplayName = "MustContain must not throw an exception when the specified value is part of the collection.")]
        public void CollectionContainsValue()
        {
            var collection = new[] { "How", "Are", "You" };
            const string value = "Are";

            var result = collection.MustContain(value);

            result.Should().BeSameAs(collection);
        }

        [Fact(DisplayName = "MustContain must throw a CollectionException when the specified subset is not part of the collection.")]
        public void IsNotSuperset()
        {
            var collection = new[] { 'x', 'z', 'y', 'b' };
            var invalidSubset = new[] { 'z', 'y', 'f' };

            Action act = () => collection.MustContain(invalidSubset, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must contain the following values:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(invalidSubset)}");
        }

        [Fact(DisplayName = "MustContain must not throw an exception when every item in the specified subset is contained in the collection.")]
        public void IsSuperset()
        {
            var collection = new[] { "a", null };
            var subset = new string[] { null };

            var result = collection.MustContain(subset);

            result.Should().BeSameAs(collection);
        }

        [Theory(DisplayName = "MustContain must throw an ArgumentNullException when either the collection or the subset are null.")]
        [InlineData(new object[] { }, null)]
        [InlineData(null, new object[] { })]
        public void SupersetOrCollectionNull(object[] collection, object[] subset)
        {
            Action act = () => collection.MustContain(subset);

            act.ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustContain(42, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 3 }.MustContain(42, message: message)));

            testData.Add(new CustomExceptionTest(exception => new[] { 'a', 'b' }.MustContain(new[] { 'c', 'd' }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 'a', 'b' }.MustContain(new[] { 'c', 'd' }, message: message)));
        }
    }
}