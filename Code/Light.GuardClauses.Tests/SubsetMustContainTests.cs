using System;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class SubsetMustContainTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustContain must throw a CollectionException when the specified subset is not part of the collection.")]
        [MemberData(nameof(IsNoSupersetData))]
        public void IsNotSuperset<T>(T[] collection, T[] invalidSubset)
        {
            Action act = () => collection.MustContain(invalidSubset, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must contain the following values{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(invalidSubset)}");
        }

        public static readonly TestData IsNoSupersetData =
            new[]
            {
                new object[] { new object[] { 1, 2, 3, 4, 5 }, new object[] { 3, 4, 5, 6 } },
                new object[] { new object[] { 'x', 'z', 'y', 'b' }, new object[] { 'z', 'y', 'f' } }
            };

        [Theory(DisplayName = "MustContain must not throw an exception when every item in the specified subset is contained in the collection.")]
        [MemberData(nameof(IsSupersetData))]
        public void IsSuperset<T>(T[] collection, T[] subset)
        {
            Action act = () => collection.MustContain(subset);

            act.ShouldNotThrow();
        }

        public static readonly TestData IsSupersetData =
            new[]
            {
                new object[] { new object[] { 1, 2, 3, 4 }, new object[] { 2, 3 } },
                new object[] { new[] { "a", "X", "z" }, new[] { "a", "a", "a" } },
                new object[] { new[] { "a", null }, new string[] { null } },
                new object[] { new[] { "1", "2", "3" }, new[] { "3", "1", "2", "3" } }
            };

        [Theory(DisplayName = "MustContain must throw an ArgumentNullException when either the collection or the subset are null.")]
        [InlineData(new object[] { }, null)]
        [InlineData(null, new object[] { })]
        public void SupersetOrCollectionNull(object[] collection, object[] subset)
        {
            Action act = () => collection.MustContain(subset);

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 'a', 'b' }.MustContain(new[] { 'c', 'd' }, exception: exception)));

            testData.Add(new CustomMessageTest<CollectionException>(message => new[] { 'a', 'b' }.MustContain(new[] { 'c', 'd' }, message: message)));
        }
    }
}