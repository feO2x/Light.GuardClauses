using System;
using System.Text.RegularExpressions;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests
{
    public sealed class MustMatchTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Fact(DisplayName = "MustMatch must throw an exception when the specified string does not match the regular expression.")]
        public void StringDoesNotMatch()
        {
            var pattern = new Regex(@"\d{5}");
            const string @string = "12c45";

            Action act = () => @string.MustMatch(pattern, nameof(@string));

            act.ShouldThrow<StringDoesNotMatchException>()
               .And.ParamName.Should().Be(nameof(@string));
        }

        [Fact(DisplayName = "MustMatch must not throw an exception when the specified string matches the regular expression.")]
        public void StringMatches()
        {
            var pattern = new Regex(@"\w{5}");
            const string @string = "abcde";

            Action act = () => @string.MustMatch(pattern, nameof(@string));

            act.ShouldNotThrow();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "12345".MustMatch(new Regex(@"\W{5}"), exception: exception)));

            testData.Add(new CustomMessageTest<StringDoesNotMatchException>(message => "12345".MustMatch(new Regex(@"\W{5}"), message: message)));
        }
    }
}