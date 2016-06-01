using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustHaveCountTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustHaveCount must throw a CollectionException when the collection does not have the specified count.")]
        [MemberData(nameof(CountDifferentData))]
        public void CountDifferent(string[] collection, int count)
        {
            Action act = () => collection.MustHaveCount(count, nameof(collection));

            act.ShouldThrow<CollectionException>()
               .And.Message.Should().Contain($"{nameof(collection)} must have count {count}, but you specified a collection with count {collection.Length}");
        }

        public static readonly TestData CountDifferentData =
            new[]
            {
                new object[] { new[] { "a", "b", "c" }, 2 },
                new object[] { new[] { "Hello", "World" }, 82 },
                new object[] { new string[] { }, 1 }
            };

        [Theory(DisplayName = "MustHaveCount must not throw an exception when the collection has the specified count.")]
        [MemberData(nameof(CountSameData))]
        public void CountSame(int[] collection, int count)
        {
            Action act = () => collection.MustHaveCount(count);

            act.ShouldNotThrow();
        }

        public static readonly TestData CountSameData =
            new[]
            {
                new object[] { new[] { 1, 2, 3, 4 }, 4 },
                new object[] { new[] { -10000, 13432521 }, 2 },
                new object[] { new int[] { }, 0 }
            };

        [Fact(DisplayName = "MustHaveCount must throw an ArgumentNullException when the specified parameter is null.")]
        public void CollectionNull()
        {
            Action act = () => ((List<double>) null).MustHaveCount(42);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory(DisplayName = "MustHaveCount must throw an ArgumentOutOfRangeException when the specified count is less than zero.")]
        [InlineData(-1)]
        [InlineData(-42)]
        public void CountLessThanZero(int invalidCount)
        {
            Action act = () => new[] { 1, 2, 3 }.MustHaveCount(invalidCount);

            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new[] { 1, 2, 3 }.MustHaveCount(1, exception: exception)));

            testData.Add(new CustomMessageTest<CollectionException>(message => new[] { 'a', 'b' }.MustHaveCount(42, message: message)));
        }
    }
}