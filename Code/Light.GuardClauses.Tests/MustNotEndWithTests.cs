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
    public sealed class MustNotEndWithTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotEndWith for strings must throw a StringException when the string ends with the specified text (case-sensitivity respected).")]
        [InlineData("Foo", "Foo")]
        [InlineData("Most men would rather deny a hard truth than face it", "face it")]
        public void StringEndsEqual(string @string, string endText)
        {
            Action act = () => @string.MustNotEndWith(endText, nameof(@string));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must not end with \"{endText}\", but you specified {@string}.");
        }

        [Theory(DisplayName = "MustNotEndWith for strings must not throw an exception when the string does not end with the specified text (case-sensitivity respected).")]
        [InlineData("Foo", "Bar")]
        [InlineData("Foo", "foo")]
        [InlineData("The things we love destroy us every time, lad. Remember that.", "Consider that.")]
        public void StringEndsDiffer(string @string, string endText)
        {
            Action act = () => @string.MustNotEndWith(endText);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustNotEndWith for strings must throw an ArgumentNullException when either paramter or text is null.")]
        [InlineData(null, "Foo")]
        [InlineData("Foo", null)]
        public void StringArgumentNull(string @string, string endText)
        {
            Action act = () => @string.MustNotEndWith(endText);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory(DisplayName = "MustNotEndWith for collections must throw a CollectionException when the specified collection ends with the specified set (order respected).")]
        [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2, 3 })]
        [InlineData(new[] { -44453, 233123, 555 }, new[] { 555 })]
        [InlineData(new[] { 5050, 8776, -233232, 445, 43597 }, new[] { 445, 43597 })]
        public void CollectionEndsEqual(IEnumerable<int> collection, IEnumerable<int> set)
        {
            Action act = () => collection.MustNotEndWith(set, parameterName: nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must not end with the following items:{new StringBuilder().AppendLine().AppendItemsWithNewLine(set).AppendLine()}but it does.");
        }

        [Theory(DisplayName = "MustNotEndWith for collections must not throw an exception when the specified collection does not end with the given set (order respected).")]
        [InlineData(new[] { 1, 2, 3 }, new[] { 4, 5 })]
        [InlineData(new[] { 1, 2, 3 }, new[] { 2, 3, 1 })]
        [InlineData(new[] { -144, 155, 166 }, new[] { -144 })]
        [InlineData(new int[0], new[] { 42 })]
        public void CollectionEndsDiffer(IEnumerable<int> collection, IEnumerable<int> set)
        {
            Action act = () => collection.MustNotEndWith(set);

            act.ShouldNotThrow();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Foo".MustNotEndWith("Foo", exception: exception)))
                    .Add(new CustomMessageTest<StringException>(message => "Bar".MustNotEndWith("Bar", message: message)));

            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustNotEndWith(new[] { 1, 2, 3 }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 42 }.MustNotEndWith(new[] { 42 }, message: message)));
        }
    }
}