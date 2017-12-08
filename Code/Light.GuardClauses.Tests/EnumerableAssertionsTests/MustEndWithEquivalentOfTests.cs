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
    public sealed class MustEndWithEquivalentOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustEndWithEquivalentOf for collections must throw a CollectionException when the collection does not end with the items of the specified set in any order.")]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 42, 87 })]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 3, 2 })]
        [InlineData(new[] { 1500 }, new[] { 3200, 4000 })]
        [InlineData(new[] { -10 }, new[] { -11 })]
        public void CollectionEndsDiffer(IEnumerable<int> collection, IEnumerable<int> set)
        {
            Action act = () => collection.MustEndWithEquivalentOf(set, parameterName: nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must end with the following items in any order:{new StringBuilder().AppendLine().AppendItemsWithNewLine(set).AppendLine()}but it does not.");
        }

        [Theory(DisplayName = "MustEndWithEquivalentOf for collections must not throw an exception when the collection does end with the items of the specified set in any order.")]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 4, 3, 2 })]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 3, 2, 4 })]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 2, 3, 4 })]
        [InlineData(new[] { 1500 }, new[] { 1500 })]
        public void CollectionEndsEqual(int[] collection, int[] set)
        {
            var result = collection.MustEndWithEquivalentOf(set);

            result.Should().BeSameAs(collection);
        }

        [Theory(DisplayName = "MustEndWithEquivalentOf for collections must throw an ArgumentNullException when the parameter or set is null.")]
        [InlineData(new object[] { }, null)]
        [InlineData(null, new object[] { })]
        public void CollectionArgumentNull(IEnumerable<object> collection, IEnumerable<object> set)
        {
            Action act = () => collection.MustEndWithEquivalentOf(set);

            act.ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustEndWithEquivalentOf(new[] { 42 }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 3 }.MustEndWithEquivalentOf(new[] { 42 }, message: message)));
        }
    }
}