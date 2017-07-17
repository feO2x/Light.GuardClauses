using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.DateTimeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeLocalTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeLocal must throw an exception when the specified date time's kind is DateTimeKind.Utc or DateTimeKind.Unspecified.")]
        [MemberData(nameof(NotLocalData))]
        public void NotLocal(DateTime invalidDateTime)
        {
            Action act = () => invalidDateTime.MustBeLocal(nameof(invalidDateTime));

            act.ShouldThrow<InvalidDateTimeException>()
               .And.Message.Should().Contain($"The specified date time \"{invalidDateTime:O}\" must be of kind {DateTimeKind.Local}, but actually is {invalidDateTime.Kind}.");
        }

        public static readonly TestData NotLocalData =
            new[]
            {
                new object[] { DateTime.UtcNow },
                new object[] { new DateTime(2017, 1, 26, 19, 18, 42, DateTimeKind.Unspecified) },
                new object[] { new DateTime(2017, 1, 26, 18, 19, 39, DateTimeKind.Utc) }
            };

        [Theory(DisplayName = "MustBeLocal must not throw an exception when the specified date time's kind is DateTimeKind.Local.")]
        [MemberData(nameof(LocalData))]
        public void Local(DateTime dateTime)
        {
            Action act = () => dateTime.MustBeLocal();

            act.ShouldNotThrow();
        }

        public static readonly TestData LocalData =
            new[]
            {
                new object[] { DateTime.Now },
                new object[] { new DateTime(2017, 1, 26, 19, 22, 26, DateTimeKind.Local) }
            };

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => DateTime.UtcNow.MustBeLocal(exception: exception)))
                    .Add(new CustomMessageTest<InvalidDateTimeException>(message => DateTime.UtcNow.MustBeLocal(message: message)));
        }
    }
}