using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.DateTimeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustBeUnspecifiedTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustBeUnspecified must throw an exception when the specified date time's kind is DateTimeKind.Utc or DateTimeKind.Local.")]
        [MemberData(nameof(NotUnspecifiedData))]
        public void NotUnspecified(DateTime invalidDateTime)
        {
            Action act = () => invalidDateTime.MustBeUnspecified(nameof(invalidDateTime));

            act.ShouldThrow<InvalidDateTimeException>()
               .And.Message.Should().Contain($"The specified date time \"{invalidDateTime:O}\" must be of kind {DateTimeKind.Unspecified}, but actually is {invalidDateTime.Kind}.");
        }

        public static readonly TestData NotUnspecifiedData =
            new[]
            {
                new object[] { DateTime.UtcNow },
                new object[] { new DateTime(2017, 1, 26, 19, 18, 42, DateTimeKind.Local) },
                new object[] { new DateTime(2017, 1, 26, 18, 19, 39, DateTimeKind.Utc) }
            };

        [Theory(DisplayName = "MustBeUnspecified must not throw an exception when the specified date time's kind is DateTimeKind.Unspecified.")]
        [MemberData(nameof(UnspecifiedData))]
        public void Unspecified(DateTime dateTime)
        {
            Action act = () => dateTime.MustBeUnspecified();

            act.ShouldNotThrow();
        }

        public static readonly TestData UnspecifiedData =
            new[]
            {
                new object[] { DateTime.MinValue },
                new object[] { new DateTime(2017, 1, 26, 19, 22, 26, DateTimeKind.Unspecified) }
            };

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => DateTime.UtcNow.MustBeUnspecified(exception: exception)))
                    .Add(new CustomMessageTest<InvalidDateTimeException>(message => DateTime.UtcNow.MustBeUnspecified(message: message)));
        }
    }
}