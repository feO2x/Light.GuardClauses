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
    public sealed class MustNotEndWithTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotEndWith for collections must throw a CollectionException when the specified collection ends with the specified set (order respected).")]
        [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
        [InlineData(new[] { -44453, 233123, 555 }, new[] { 555 })]
        [InlineData(new[] { 5050, 8776, -233232, 445, 43597 }, new[] { 445, 43597 })]
        public void CollectionEndsEqual(IEnumerable<int> collection, IEnumerable<int> set)
        {
            Action act = () => collection.MustNotEndWith(set, parameterName: nameof(collection));

            act.Should().Throw<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must not end with the following items:{new StringBuilder().AppendLine().AppendItemsWithNewLine(set).AppendLine()}but it does.");
        }

        [Theory(DisplayName = "MustNotEndWith for collections must not throw an exception when the specified collection does not end with the given set (order respected).")]
        [InlineData(new[] { 1, 2, 3 }, new[] { 4, 5 })]
        [InlineData(new[] { 1, 2, 3 }, new[] { 2, 3, 1 })]
        [InlineData(new[] { -144, 155, 166 }, new[] { -144 })]
        [InlineData(new int[0], new[] { 42 })]
        public void CollectionEndsDiffer(int[] collection, int[] set)
        {
            var result = collection.MustNotEndWith(set);

            result.Should().BeSameAs(collection);
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustNotEndWith(new[] { 1, 2, 3 }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 42 }.MustNotEndWith(new[] { 42 }, message: message)));
        }
    }
}