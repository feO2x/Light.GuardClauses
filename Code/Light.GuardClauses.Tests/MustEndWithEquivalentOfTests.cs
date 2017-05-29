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
    public sealed class MustEndWithEquivalentOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustEndWithEquivalentOf for strings must throw a StringException when the string does not end with the specified text (ignoring case-sensitivity).")]
        [InlineData("Foo", "Bar")]
        [InlineData("This should end", "begin")]
        [InlineData("I'm the captain. If this ship goes down, I go down with it.", "This Ship goes down")]
        public void StringEndsDiffers(string @string, string endText)
        {
            Action act = () => @string.MustEndWithEquivalentOf(endText, nameof(@string));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must end with the equivalent of \"{endText}\", but you specified {@string}.");
        }

        [Theory(DisplayName = "MustEndWithEquivalentOf for strings must not throw an exception when the string ends with the specified text (ignoring case-sensitivity).")]
        [InlineData("This is the end", "is the end")]
        [InlineData("Hello", "LO")]
        [InlineData("Why are all the gods such vicious cunts? Where is the god of tits and wine?", "Tits and Wine?")]
        public void StringEndsEqual(string @string, string endText)
        {
            Action act = () => @string.MustEndWithEquivalentOf(endText);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustEndWithEquivalentOf for strings must throw an ArgumentNullException when either paramter or text is null.")]
        [InlineData(null, "Foo")]
        [InlineData("Foo", null)]
        public void StringArgumentNull(string @string, string endText)
        {
            Action act = () => @string.MustEndWithEquivalentOf(endText);

            act.ShouldThrow<ArgumentNullException>();
        }

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
        public void CollectionEndsEqual(IEnumerable<int> collection, IEnumerable<int> set)
        {
            Action act = () => collection.MustEndWithEquivalentOf(set);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustEndWithEquivalentOf for collections must throw an ArgumentNullException when the parameter or set is null.")]
        [InlineData(new object[] { }, null)]
        [InlineData(null, new object[] { })]
        public void CollectionArgumentNull(IEnumerable<object> collection, IEnumerable<object> set)
        {
            Action act = () => collection.MustEndWithEquivalentOf(set);

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "The Lord of Light wants his enemies burnt".MustEndWithEquivalentOf("One should treat others as one would like others to treat oneself", exception: exception)))
                    .Add(new CustomMessageTest<StringException>(message => "The Drowned God wants his enemies drowned".MustEndWithEquivalentOf("altruism", message: message)));

            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustEndWithEquivalentOf(new[] { 42 }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 3 }.MustEndWithEquivalentOf(new[] { 42 }, message: message)));
        }
    }
}