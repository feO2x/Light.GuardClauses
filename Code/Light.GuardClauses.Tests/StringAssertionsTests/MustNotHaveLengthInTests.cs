using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class MustNotHaveLengthInTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustMotHaveLengthIn must throw a StringException when the length of the specified string is within the given range.")]
        [MemberData(nameof(LengthInRangeData))]
        public void LengthWithinRange(string @string, Range<int> range)
        {
            Action act = () => @string.MustMotHaveLengthIn(range, nameof(@string));

            var fromBoundaryKind = range.IsFromInclusive ? "inclusive" : "exclusive";
            var toBoundaryKind = range.IsToInclusive ? "inclusive" : "exclusive";
            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} \"{@string}\" must not have a length between {range.From} ({fromBoundaryKind}) and {range.To} ({toBoundaryKind}), but it actually has a length of {@string.Length}.");
        }

        public static readonly IEnumerable<object[]> LengthInRangeData =
            new[]
            {
                new object[] { "Foo", Range<int>.FromInclusive(0).ToExclusive(10) },
                new object[] { "", Range<int>.FromInclusive(0).ToInclusive(5) },
                new object[] { "Is it two wives you want or two castles?", Range<int>.FromInclusive(15).ToInclusive(50) }
            };

        [Theory(DisplayName = "MustMotHaveLengthIn must not throw an exception when the length of the string is out of the specified range.")]
        [MemberData(nameof(LengthOutOfRangeData))]
        public void LengthOutOfRange(string @string, Range<int> range)
        {
            var result = @string.MustMotHaveLengthIn(range);

            result.Should().BeSameAs(@string);
        }

        public static readonly IEnumerable<object[]> LengthOutOfRangeData =
            new[]
            {
                new object[] { "Bar", Range<int>.FromInclusive(4).ToExclusive(10) },
                new object[] { "", Range<int>.FromExclusive(0).ToInclusive(5) },
                new object[] { "That's what I do. I drink and I know things. ", Range<int>.FromInclusive(50).ToInclusive(100) }
            };

        [Fact(DisplayName = "MustMotHaveLengthIn must throw an ArgumentNullException when the specified string is null.")]
        public void ParameterNull()
        {
            Action act = () => ((string) null).MustMotHaveLengthIn(Range<int>.FromInclusive(42).ToExclusive(87));

            act.ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.AddExceptionTest(exception => "Foo".MustMotHaveLengthIn(Range<int>.FromInclusive(2).ToInclusive(3), exception: exception))
                    .AddMessageTest<StringException>(message => "Bar".MustMotHaveLengthIn(Range<int>.FromInclusive(2).ToInclusive(3), message: message));
        }
    }
}