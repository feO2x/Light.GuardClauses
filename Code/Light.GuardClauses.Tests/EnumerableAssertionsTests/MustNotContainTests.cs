using System;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.EnumerableAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotContainTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotContain must throw a CollectionException when the collection contains any of the items of the specified set.")]
        [MemberData(nameof(SetContainedData))]
        public void SetContained<T>(T[] collection, T[] set)
        {
            Action act = () => collection.MustNotContain(set, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must not contain any of the following values:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(set)}");
        }

        public static readonly TestData SetContainedData =
            new[]
            {
                new object[] { new[] { "Hello", "World" }, new[] { "Hello", "There" } },
                new object[] { new object[] { 1, 2, 3, 4, 5 }, new object[] { 2, 5 } },
                new object[] { new[] { new object(), null }, new object[] { null } }
            };

        [Theory(DisplayName = "MustNotContain must not throw an exception when the collection does not contain any items of the specified set.")]
        [MemberData(nameof(SetNotContainedData))]
        public void SetNotContained<T>(T[] collection, T[] set)
        {
            Action act = () => collection.MustNotContain(set);

            act.ShouldNotThrow();
        }

        public static readonly TestData SetNotContainedData =
            new[]
            {
                new object[] { new[] { "Hello", "There" }, new[] { "What's", "Up?" } },
                new object[] { new object[] { 1, 2, 3, 4 }, new object[] { 81, -34445, 20 } }
            };

        [Theory(DisplayName = "MustNotContain must throw an ArgumentNullException when the specified parameter or set is null.")]
        [InlineData(new object[0], null)]
        [InlineData(null, new object[0])]
        public void SetArgumentsNull(object[] collection, object[] set)
        {
            Action act = () => collection.MustNotContain(set);

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 'a', 'b' }.MustNotContain(new[] { 'a' }, exception: exception)));

            testData.Add(new CustomMessageTest<CollectionException>(message => new[] { 'a', 'b' }.MustNotContain(new[] { 'a' }, message: message)));
        }
    }
}