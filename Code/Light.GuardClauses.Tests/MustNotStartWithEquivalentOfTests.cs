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
    public sealed class MustNotStartWithEquivalentOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotStartWithEquivalentOf for strings must throw a StringException when the string starts with the specified text (ignoring case-sensitivity).")]
        [InlineData("Hello", "Hell")]
        [InlineData("Foo", "foo")]
        [InlineData("PASSWORD", "PaSsWOrd")]
        [InlineData("You love your children. It's your one redeeming quality; that and your cheekbones.", "you love your children")]
        public void StartTextEqual(string @string, string startText)
        {
            Action act = () => @string.MustNotStartWithEquivalentOf(startText, nameof(@string));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must not start with the equivalent of \"{startText}\", but you specified {@string}.");
        }

        [Theory(DisplayName = "MustNotStartWithEquivalentOf for strings must not throw an exception when the string does not start with the specified text (ignoring case-sensitivity).")]
        [InlineData("Hello", "World")]
        [InlineData("Foo", "Bar")]
        [InlineData("As impossible as it seems, there was once a time I was unaccustomed to wine.", "I was unaccustomed")]
        public void StartTextDifferent(string @string, string startText)
        {
            Action act = () => @string.MustNotStartWithEquivalentOf(startText);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustNotStartWithEquivalentOf for strings must throw an ArgumentNullException when parameter or text is null.")]
        [InlineData(null, "Foo")]
        [InlineData("Foo", null)]
        public void StringArgumentNull(string @string, string startText)
        {
            Action act = () => @string.MustNotStartWithEquivalentOf(startText);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory(DisplayName = "MustNotStartWithEquivalentOf for collections must throw a CollectionException when the collection starts with the items of set in any order.")]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 3, 2, 1 })]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 2, 1, 3 })]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2 })]
        [InlineData(new[] { 150, -100, 99 }, new[] { 150 })]
        public void CollectionsStartEqual(IEnumerable<int> collection, IEnumerable<int> set)
        {
            Action act = () => collection.MustNotStartWithEquivalentOf(set, parameterName: nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must not start with the following items in any order:{new StringBuilder().AppendLine().AppendItemsWithNewLine(set).AppendLine()}but it does.");
        }

        [Theory(DisplayName = "MustNotStartWithEquivalentOf for collections must not throw an exception when the collection does not the contain the items of set at the start in any order.")]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 42, -55 })]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 2 })]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 3, 2 })]
        [InlineData(new[] { 2500 }, new[] { 2500, 2600 })]
        public void CollectionsStartDifferent(IEnumerable<int> collection, IEnumerable<int> set)
        {
            Action act = () => collection.MustNotStartWithEquivalentOf(set);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustNotStartWithEquivalentOf for collections must throw an ArgumentNullException when parameter or set is null.")]
        [InlineData(new object[] { }, null)]
        [InlineData(null, new object[] { })]
        public void CollectionArgumentNull(IEnumerable<object> collection, IEnumerable<object> set)
        {
            Action act = () => collection.MustNotStartWithEquivalentOf(set);

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Foo".MustNotStartWithEquivalentOf("foo", exception: exception)))
                    .Add(new CustomMessageTest<StringException>(message => "Foo".MustNotStartWithEquivalentOf("foo", message: message)));

            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustNotStartWithEquivalentOf(new[] { 1 }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2, 3 }.MustNotStartWithEquivalentOf(new[] { 1 }, message: message)));
        }
    }
}