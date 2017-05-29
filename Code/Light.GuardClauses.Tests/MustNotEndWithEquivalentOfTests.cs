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
    public sealed class MustNotEndWithEquivalentOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotEndWithEquivalentOf for strings must throw a StringException when the string ends with the specified text (case-sensitivity ignored).")]
        [InlineData("Foo", "Foo")]
        [InlineData("Bar", "bAR")]
        [InlineData("Death is so terribly final, while life is full of possibilities", "Full of Possibilities")]
        public void StringEndsEqual(string @string, string endText)
        {
            Action act = () => @string.MustNotEndWithEquivalentOf(endText, nameof(@string));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must not end with equivalent of \"{endText}\", but you specified {@string}.");
        }

        [Theory(DisplayName = "MustNotEndWithEquivalentOf for strings must not throw an exception when the string does not end with the specified text (case-sensitivity ignored).")]
        [InlineData("Foo", "Bar")]
        [InlineData("And I have a tender spot in my heart for cripples and bastards and broken things", "broken glass")]
        public void StringEndsDiffer(string @string, string endText)
        {
            Action act = () => @string.MustNotEndWithEquivalentOf(endText);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustNotEndWithEquivalentOf for strings must throw an ArgumentNullException when either paramter or text is null.")]
        [InlineData(null, "Foo")]
        [InlineData("Foo", null)]
        public void StringArgumentNull(string @string, string endText)
        {
            Action act = () => @string.MustNotEndWithEquivalentOf(endText);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory(DisplayName = "MustNotEndWithEquivalentOf for collections must throw a CollectionException when the collection does end with the specified items in any order.")]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 4, 3, 2 })]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 3, 2, 4 })]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 3, 4 })]
        [InlineData(new[] { -1255 }, new[] { -1255 })]
        public void CollectionEndsEqual(IEnumerable<int> collection, IEnumerable<int> set)
        {
            Action act = () => collection.MustNotEndWithEquivalentOf(set, parameterName: nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must not end with the following items in any order:{new StringBuilder().AppendLine().AppendItemsWithNewLine(set).AppendLine()}but it does.");
        }

        [Theory(DisplayName = "MustNotEndWithEquivalentOf for collections must not throw an exception when the collection does not end with the specified items in any order.")]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 42, 87 })]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 2, 3 })]
        [InlineData(new[] { 603 }, new[] { 499, 717 })]
        public void CollectionEndsDiffer(IEnumerable<int> collection, IEnumerable<int> set)
        {
            Action act = () => collection.MustNotEndWithEquivalentOf(set);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustNotEndWithEquivalentOf for collections must throw an ArgumentNullException when parameter or set is null.")]
        [InlineData(new object[] { }, null)]
        [InlineData(null, new object[] { })]
        public void CollectionArgumentNull(IEnumerable<object> collection, IEnumerable<object> set)
        {
            Action act = () => collection.MustNotEndWithEquivalentOf(set);

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Foo".MustNotEndWithEquivalentOf("foo", exception: exception)))
                    .Add(new CustomMessageTest<StringException>(message => "Bar".MustNotEndWithEquivalentOf("Bar", message: message)));

            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustNotEndWithEquivalentOf(new[] { 3, 2 }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 3 }.MustNotEndWithEquivalentOf(new[] { 3, 2 }, message: message)));
        }
    }
}