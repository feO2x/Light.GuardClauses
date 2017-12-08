using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustHaveLengthInTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustHaveLengthIn must throw a StringException when the length of the specified string is not within the given range.")]
        [MemberData(nameof(LengthOutOfRangeData))]
        public void LengthOutOfRange(string @string, Range<int> range)
        {
            Action act = () => @string.MustHaveLengthIn(range, nameof(@string));

            var fromBoundaryKind = range.IsFromInclusive ? "inclusive" : "exclusive";
            var toBoundaryKind = range.IsToInclusive ? "inclusive" : "exclusive";
            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} \"{@string}\" must have a length between {range.From} ({fromBoundaryKind}) and {range.To} ({toBoundaryKind}), but it actually has a length of {@string.Length}.");
        }

        public static readonly TestData LengthOutOfRangeData =
            new[]
            {
                new object[] { "Foo", Range<int>.FromInclusive(4).ToExclusive(10) },
                new object[] { "", Range<int>.FromExclusive(0).ToInclusive(5) },
                new object[] { "Isn't it a man's duty to be drunk at his own wedding?", Range<int>.FromInclusive(90).ToInclusive(120) }
            };

        [Theory(DisplayName = "MustHaveLengthIn must not throw an exception when the length of the string is within the specified range.")]
        [MemberData(nameof(LengthInRangeData))]
        public void LengthWithinRange(string @string, Range<int> range)
        {
            var result = @string.MustHaveLengthIn(range);

            result.Should().BeSameAs(@string);
        }

        public static readonly TestData LengthInRangeData =
            new[]
            {
                new object[] { "Bar", Range<int>.FromInclusive(2).ToExclusive(10) },
                new object[] { "", Range<int>.FromInclusive(0).ToInclusive(5) },
                new object[]
                {
                    "Come, wife. I vomited on a girl once in the middle of the act. Not proud of it. But I think honesty is important between a man and wife, don't you agree? Come I'll tell you all about it. Put you in the mood.",
                    Range<int>.FromInclusive(50).ToInclusive(300)
                }
            };

        [Fact(DisplayName = "MustHaveLengthIn must throw an ArgumentNullException when the specified string is null.")]
        public void ParameterNull()
        {
            Action act = () => ((string) null).MustHaveLengthIn(Range<int>.FromInclusive(42).ToExclusive(87));

            act.ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => "Foo".MustHaveLengthIn(Range<int>.FromInclusive(10).ToExclusive(20), exception: exception))
                    .AddMessageTest<StringException>(message => "Bar".MustHaveLengthIn(Range<int>.FromExclusive(7).ToExclusive(9), message: message));
        }
    }
}