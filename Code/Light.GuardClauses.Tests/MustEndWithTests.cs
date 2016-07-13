using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustEndWithTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustEndWith for strings must throw a StringException when the string does not end with the specified text (case-sensitivity respected).")]
        [InlineData("Wello", "Horld!")]
        [InlineData("This should end", "End")]
        [InlineData("A mind needs books as a sword needs a whetstone, if it is to keep its edge.", "keep its ledge.")]
        public void StringEndsDiffer(string @string, string endText)
        {
            Action act = () => @string.MustEndWith(endText, nameof(@string));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must end with \"{endText}\", but you specified {@string}.");
        }

        [Theory(DisplayName = "MustEndWith for strings must not throw an exception when the string ends with the specified text (case-sensitivity respected).")]
        [InlineData("This is the end", "is the end")]
        [InlineData("Hello", "lo")]
        [InlineData("Can a man still be brave if he's afraid?", "he's afraid?")]
        public void StringEndsEqual(string @string, string endText)
        {
            Action act = () => @string.MustEndWith(endText);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustEndWith for strings must throw an ArgumentNullException when either paramter or text is null.")]
        [InlineData(null, "Foo")]
        [InlineData("Foo", null)]
        public void StringArgumentNull(string @string, string endText)
        {
            Action act = () => @string.MustEndWith(endText);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory(DisplayName = "MustEndWith for collections must throw a CollectionException when the collection does not end with the given set (order respected).")]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 7, 2 })]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 4, 3 })]
        [InlineData(new[] { 877, -4431, 15988732 }, new[] { -4431 })]
        [InlineData(new[] { 1 }, new[] { -4431, 1 })]
        public void CollectionEndsDiffer(IEnumerable<int> collection, IEnumerable<int> set)
        {
            Action act = () => collection.MustEndWith(set, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must end with the following items:{new StringBuilder().AppendLine().AppendItemsWithNewLine(set).AppendLine()}but it does not.");
        }

        [Theory(DisplayName = "MustEndWith for collections must not throw an exception when the collection ends with the given set (order respected).")]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 2, 3, 4 })]
        [InlineData(new[] { 10, 15, 20 }, new[] { 20 })]
        [InlineData(new[] { -8000 }, new[] { -8000 })]
        public void CollectionEndsSame(IEnumerable<int> collection, IEnumerable<int> set)
        {
            Action act = () => collection.MustEndWith(set);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustEndWith must throw an ArgumentNullException when the collection or the set are null.")]
        [InlineData(null, new object[] { })]
        [InlineData(new object[] { }, null)]
        public void CollectionArgumentNull(IEnumerable<object> collection, IEnumerable<object> set)
        {
            Action act = () => collection.MustEndWith(set);

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "That is the only time a man can be brave".MustEndWith("afraid", exception: exception)))
                    .Add(new CustomMessageTest<StringException>(message => "Foo".MustEndWith("Bar", message: message)));

            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustEndWith(new[] { 4 }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 3 }.MustEndWith(new[] { 4 }, message: message)));
        }
    }
}