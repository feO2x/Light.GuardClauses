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
    public sealed class MustNotStartWithTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotStartWith for strings must throw a StringException when the string starts with the specified text.")]
        [InlineData("Hello", "Hell")]
        [InlineData("Foo", "Foo")]
        [InlineData("Would it be excessive of me to ask you to save my life twice in a week?", "Would it be excessive")]
        public void StartTextEqual(string @string, string startText)
        {
            Action act = () => @string.MustNotStartWith(startText, nameof(@string));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must not start with \"{startText}\", but you specified {@string}.");
        }

        [Theory(DisplayName = "MustNotStartWith for strings must not throw an exception when the string does not start with the specified text.")]
        [InlineData("Hello", "World")]
        [InlineData("Foo", "Bar")]
        [InlineData("Those are brave men knocking at our door. Let's go kill them!", "These are brave")]
        public void StartTextDifferent(string @string, string startText)
        {
            Action act = () => @string.MustNotStartWith(startText);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustNotStartWith for strings must throw an ArgumentNullException when parameter or text is null.")]
        [InlineData(null, "Foo")]
        [InlineData("Foo", null)]
        public void ArgumentNull(string @string, string startText)
        {
            Action act = () => @string.MustNotStartWith(startText);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory(DisplayName = "MustNotStartWith for collections must throw a CollectionException when the collection starts with the specified subset in the same order.")]
        [InlineData(new[] { 1, 2, 3 }, new[] { 1, 2 })]
        [InlineData(new[] { -1, -2, -3, -4, -5 }, new[] { -1, -2, -3 })]
        [InlineData(new[] { 88, 412, -33 }, new[] { 88, 412, -33 })]
        [InlineData(new[] { 1 }, new[] { 1 })]
        public void StartItemsEqual(IEnumerable<int> collection, IEnumerable<int> subset)
        {
            Action act = () => collection.MustNotStartWith(subset, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must not start with the following items:{new StringBuilder().AppendLine().AppendItemsWithNewLine(subset).AppendLine()}but it does.");
        }

        [Theory(DisplayName = "MustNotStartWith for collections must not throw an exception when the collection does not start with the specified subset in the same order.")]
        [InlineData(new[] { 1, 2, 3 }, new[] { 1, 3 })]
        [InlineData(new[] { 1, 2, 3 }, new[] { 2, 3 })]
        [InlineData(new[] { 1, 2, 3 }, new[] { 2, 3, 1 })]
        [InlineData(new[] { 1, 2, 3 }, new[] { 3, 1, 2 })]
        [InlineData(new[] { -42 }, new[] { -42, 69, 55 })]
        [InlineData(new[] { 42, 83, 1567 }, new[] { -1, -2 })]
        [InlineData(new[] { 1 }, new[] { -3 })]
        public void StartItemsDifferent(IEnumerable<int> collection, IEnumerable<int> subset)
        {
            Action act = () => collection.MustNotStartWith(subset);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustNotStartWith for collections must throw an ArgumentNullException when any of the collection parameters is null.")]
        [InlineData(null, new[] { "Foo" })]
        [InlineData(new[] { "Foo" }, null)]
        [InlineData(null, null)]
        public void ParametersNull(IEnumerable<string> first, IEnumerable<string> second)
        {
            Action act = () => first.MustNotStartWith(second);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact(DisplayName = "MustNotStartWith for collections must throw an EmptyCollectionException when specified subset has no items.")]
        public void SubsetEmpty()
        {
            var collection = new[] { "Foo", "Bar" };

            Action act = () => collection.MustNotStartWith(new List<string>());

            act.ShouldThrow<EmptyCollectionException>()
               .And.Message.Should().Contain("Your precondition is set up wrongly: set is an empty collection.");
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Foo".MustNotStartWith("Foo", exception: exception)))
                    .Add(new CustomMessageTest<StringException>(message => "Foo".MustNotStartWith("Foo", message: message)));

            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2 }.MustNotStartWith(new[] { 1 }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2 }.MustNotStartWith(new[] { 1 }, message: message)));
        }
    }
}