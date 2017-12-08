using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.StringAssertionsTests
{
    public sealed class MustStartWithEquivalentOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustStartWithEquivalentOf for strings must throw a StringException when the string does not start with the specified text (ignoring case-sensitivity).")]
        [InlineData("Hello", "World")]
        [InlineData("Foo", "Bar")]
        [InlineData("You won't be a prisoner after today, you'll be my wife.", "You will be")]
        public void StartTextDifferent(string @string, string startText)
        {
            Action act = () => @string.MustStartWithEquivalentOf(startText, nameof(@string));

            act.ShouldThrow<StringException>()
               .And.Message.Should().Contain($"{nameof(@string)} must start with the equivalent of \"{startText}\", but you specified {@string}.");
        }

        [Theory(DisplayName = "MustStartWithEquivalentOf for strings must not throw an exception when the string starts with the specified text (ignoring case-sensitivity).")]
        [InlineData("Hello", "Hell")]
        [InlineData("Foo", "foo")]
        [InlineData("Pwnd", "pWnD")]
        [InlineData("I'm a monster, as well as a dwarf. You should charge me double.", "i'm a MONSTER")]
        public void StartTextEqual(string @string, string startText)
        {
            var result = @string.MustStartWithEquivalentOf(startText);

            result.Should().BeSameAs(@string);
        }

        [Theory(DisplayName = "MustStartWithEquivalentOf for strings must throw an ArgumentNullException when parameter or text is null.")]
        [InlineData(null, "Foo")]
        [InlineData("Foo", null)]
        public void ArgumentNull(string @string, string startText)
        {
            Action act = () => @string.MustStartWithEquivalentOf(startText);

            act.ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => "Foo".MustStartWithEquivalentOf("Bar", exception: exception)))
                    .Add(new CustomMessageTest<StringException>(message => "Foo".MustStartWithEquivalentOf("Bar", message: message)));
        }
    }
}