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
    public sealed class MustNotEndWithEquivalentOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotEndWithEquivalentOf for collections must throw a CollectionException when the collection does end with the specified items in any order.")]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 4, 3, 2 })]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 3, 2, 4 })]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 3, 4 })]
        [InlineData(new[] { -1255 }, new[] { -1255 })]
        public void CollectionEndsEqual(IEnumerable<int> collection, IEnumerable<int> set)
        {
            Action act = () => collection.MustNotEndWithEquivalentOf(set, parameterName: nameof(collection));

            act.Should().Throw<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must not end with the following items in any order:{new StringBuilder().AppendLine().AppendItemsWithNewLine(set).AppendLine()}but it does.");
        }

        [Theory(DisplayName = "MustNotEndWithEquivalentOf for collections must not throw an exception when the collection does not end with the specified items in any order.")]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 42, 87 })]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 2, 3 })]
        [InlineData(new[] { 603 }, new[] { 499, 717 })]
        public void CollectionEndsDiffer(int[] collection, int[] set)
        {
            var result = collection.MustNotEndWithEquivalentOf(set);

            result.Should().BeSameAs(collection);
        }

        [Theory(DisplayName = "MustNotEndWithEquivalentOf for collections must throw an ArgumentNullException when parameter or set is null.")]
        [InlineData(new object[] { }, null)]
        [InlineData(null, new object[] { })]
        public void CollectionArgumentNull(IEnumerable<object> collection, IEnumerable<object> set)
        {
            Action act = () => collection.MustNotEndWithEquivalentOf(set);

            act.Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustNotEndWithEquivalentOf(new[] { 3, 2 }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 3 }.MustNotEndWithEquivalentOf(new[] { 3, 2 }, message: message)));
        }
    }
}