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
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustStartWithTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustStartWith for collections must throw a CollectionException when the collection does not start with the specified subset in the same order.")]
        [InlineData(new[] { 1, 2, 3 }, new[] { 1, 3 })]
        [InlineData(new[] { 88, 22, 36 }, new[] { 1 })]
        [InlineData(new[] { 1 }, new[] { 1, 2, 3 })]
        public void StartItemsDifferent(IEnumerable<int> collection, IEnumerable<int> subset)
        {
            Action act = () => collection.MustStartWith(subset, parameterName: nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must start with the following items:{new StringBuilder().AppendLine().AppendItemsWithNewLine(subset).AppendLine()}but it does not.");
        }

        [Theory(DisplayName = "MustStartWith for collections must not throw an exception when the collection starts with the specified items in the same order.")]
        [InlineData(new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3 })]
        [InlineData(new[] { -10, 355, 2 }, new[] { -10, 355 })]
        [InlineData(new[] { 99, 104, 4 }, new[] { 99, 104, 4 })]
        public void StartItemsEqual(IEnumerable<int> collection, IEnumerable<int> subset)
        {
            Action act = () => collection.MustStartWith(subset);

            act.ShouldNotThrow();
        }

        [Theory(DisplayName = "MustStartWith for collections must throw an ArgumentNullException when any of the collection parameters is null.")]
        [InlineData(null, new[] { "Foo" })]
        [InlineData(new[] { "Foo" }, null)]
        [InlineData(null, null)]
        public void ParametersNull(IEnumerable<string> first, IEnumerable<string> second)
        {
            Action act = () => first.MustStartWith(second);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Fact(DisplayName = "MustStartWith for collections must throw an EmptyCollectionException when specified subset has no items.")]
        public void SubsetEmpty()
        {
            var collection = new[] { "Foo", "Bar" };

            Action act = () => collection.MustStartWith(new List<string>());

            act.ShouldThrow<EmptyCollectionException>()
               .And.Message.Should().Contain("Your precondition is set up wrongly: set is null or an empty collection.");
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2 }.MustStartWith(new[] { 3 }, exception: exception)))
                    .Add(new CustomMessageTest<CollectionException>(message => new[] { 1, 2 }.MustStartWith(new[] { 3 }, message: message)));
        }
    }
}
