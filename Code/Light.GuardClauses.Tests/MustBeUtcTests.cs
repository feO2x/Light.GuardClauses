using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeUtcTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeUtc must throw an exception when the specified DateTime.Kind is not DateTimeKind.UTC")]
        [MemberData(nameof(NotUtcData))]
        public void NotUtc(DateTime invalidDateTime)
        {
            Action act = () => invalidDateTime.MustBeUtc(nameof(invalidDateTime));

            act.ShouldThrow<InvalidDateTimeException>()
               .And.Message.Should().Contain($"The specified date time \"{invalidDateTime:O}\" must be of kind UTC, but actually is {invalidDateTime.Kind}.");
        }

        public static readonly TestData NotUtcData =
            new[]
            {
                new object[] { new DateTime(2017, 1, 26, 18, 50, 5, DateTimeKind.Local) },
                new object[] { new DateTime(1333, 3, 30, 0, 16, 49, DateTimeKind.Unspecified) },
                new object[] { DateTime.Now }
            };

        [Theory(DisplayName = "MustBeUtc must not throw an exception when the specified DateTime is of kind UTC.")]
        [MemberData(nameof(UtcData))]
        public void Utc(DateTime utcDateTime)
        {
            Action act = () => utcDateTime.MustBeUtc();

            act.ShouldNotThrow();
        }

        public static readonly TestData UtcData =
            new[]
            {
                new object[] { DateTime.UtcNow },
                new object[] { new DateTime(2017, 1, 26, 18, 1, 51, DateTimeKind.Utc) }
            };

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => DateTime.Now.MustBeUtc(exception: exception)))
                    .Add(new CustomMessageTest<InvalidDateTimeException>(message => DateTime.Now.MustBeUtc(message: message)));
        }
    }
}