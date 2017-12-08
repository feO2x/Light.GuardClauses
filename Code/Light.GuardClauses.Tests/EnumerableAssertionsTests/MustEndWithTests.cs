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
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustEndWithTests : ICustomMessageAndExceptionTestDataProvider

    {
        [Theory(DisplayName = "MustEndWith for collections must throw a CollectionException when the collection does not end with the given set (order respected).")]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 7, 2 })]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 4, 3 })]
        [InlineData(new[] { 877, -4431, 15988732 }, new[] { -4431 })]
        [InlineData(new[] { 1 }, new[] { -4431, 1 })]
        public void CollectionEndsDiffer(IEnumerable<int> collection, IEnumerable<int> set)
        {
            Action act = () => collection.MustEndWith(set, parameterName: nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must end with the following items:{new StringBuilder().AppendLine().AppendItemsWithNewLine(set).AppendLine()}but it does not.");
        }

        [Theory(DisplayName = "MustEndWith for collections must not throw an exception when the collection ends with the given set (order respected).")]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 2, 3, 4 })]
        [InlineData(new[] { 10, 15, 20 }, new[] { 20 })]
        [InlineData(new[] { -8000 }, new[] { -8000 })]
        public void CollectionEndsSame(int[] collection, int[] set)
        {
            var result = collection.MustEndWith(set);

            result.Should().BeSameAs(collection);
        }

        [Theory(DisplayName = "MustEndWith must throw an ArgumentNullException when the collection or the set are null.")]
        [InlineData(null, new object[] { })]
        [InlineData(new object[] { }, null)]
        public void CollectionArgumentNull(IEnumerable<object> collection, IEnumerable<object> set)
        {
            Action act = () => collection.MustEndWith(set);

            act.ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustEndWith(new[] { 4 }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 3 }.MustEndWith(new[] { 4 }, message: message)));
        }
    }
}