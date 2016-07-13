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
    public sealed class MustStartWithEquivalentOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustStartWithEquivalentOf for strings must throw a StringException when the string does not start with the specified text (ignoring case-sensitivity).")]
        [InlineData("Hello", "World")]
        [InlineData("Foo", "Bar")]
        [InlineData("You won't be a prisoner after today, you'll be my wife.", "You will be")]
        public void StartTextDifferent(string @string, string startText)
        {
            Action act = () => @string.MustStartWithEquivalentOf(startText, nameof(@string));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must start with the equivalent of \"{startText}\", but you specified {@string}.");
        }

        [Theory(DisplayName = "MustStartWithEquivalentOf for strings must not throw an exception when the string starts with the specified text (ignoring case-sensitivity).")]
        [InlineData("Hello", "Hell")]
        [InlineData("Foo", "foo")]
        [InlineData("Pwnd", "pWnD")]
        [InlineData("I'm a monster, as well as a dwarf. You should charge me double.", "i'm a MONSTER")]
        public void StartTextEqual(string @string, string startText)
        {
            Action act = () => @string.MustStartWithEquivalentOf(startText);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustStartWithEquivalentOf for strings must throw an ArgumentNullException when parameter or text is null.")]
        [InlineData(null, "Foo")]
        [InlineData("Foo", null)]
        public void ArgumentNull(string @string, string startText)
        {
            Action act = () => @string.MustStartWithEquivalentOf(startText);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory(DisplayName = "MustStartWithEquivalentOf for collections must throw a CollectionException when the collection does not start with the items in the given set in any order.")]
        [InlineData(new[] { 1, 2, 3, 4 }, new[] { 87, -42 })]
        [InlineData(new[] { 150, 200, 250 }, new[] { 0 })]
        [InlineData(new[] { 150000 }, new[] { 150000, 160000 })]
        [InlineData(new[] { 1 }, new[] { 42 })]
        public void CollectionStartsDiffer(IEnumerable<int> collection, IEnumerable<int> set)
        {
            Action act = () => collection.MustStartWithEquivalentOf(set, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must start with the following items in any order:{new StringBuilder().AppendLine().AppendItemsWithNewLine(set).AppendLine()}but it does not.");
        }

        [Theory(DisplayName = "MustStartWithEquivalentOf for collections must not throw an exception when the collection starts with the specified items of set in any order.")]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3 })]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 2, 3, 1 })]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 3, 1, 2 })]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 2, 1 })]
        [InlineData(new[] { 150, -200, 240, -290 }, new[] { -200, 150 })]
        [InlineData(new[] { 1 }, new[] { 1 })]
        public void CollectionStartsEqual(IEnumerable<int> collection, IEnumerable<int> set)
        {
            Action act = () => collection.MustStartWithEquivalentOf(set);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustStartWithEquivalentOf for collections must throw an ArgumentNullException when parameter or set are null.")]
        [InlineData(new object[] { }, null)]
        [InlineData(null, new object[] { })]
        public void CollectionsArgumentNull(IEnumerable<object> collection, IEnumerable<object> set)
        {
            Action act = () => collection.MustStartWithEquivalentOf(set);

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Foo".MustStartWithEquivalentOf("Bar", exception: exception)))
                    .Add(new CustomMessageTest<StringException>(message => "Foo".MustStartWithEquivalentOf("Bar", message: message)));

            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustStartWithEquivalentOf(new[] { 4 }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 3 }.MustStartWithEquivalentOf(new[] { 4 }, message: message)));
        }
    }
}