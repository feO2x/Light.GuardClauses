using System;
using FluentAssertions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.UriAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustHaveOneSchemeOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustHaveOneSchemeOf must throw an ArgumentException when the specified URI is not an absolute one or does not have one of the specified schemes.")]
        [InlineData("/foo", UriKind.Relative)]
        [InlineData("ftp://localhost:8080", UriKind.Absolute)]
        public void InvalidScheme(string uri, UriKind kind)
        {
            Action act = () => new Uri(uri, kind).MustHaveOneSchemeOf(new[] { "http", "https" }, nameof(uri));

            act.ShouldThrow<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(uri)} must have one of the following schemes:");
        }

        [Theory (DisplayName = "MustHaveOneSchemeOf must not throw an exception when the specified URI is an absolute one having one of the specified schemes.")]
        [InlineData("http://www.feo2x.com")]
        [InlineData("https://docs.microsoft.com")]
        public void ValidScheme(string uri)
        {
            Action act = () => new Uri(uri).MustHaveOneSchemeOf(new[] { "http", "https" });

            act.ShouldNotThrow();
        }

        [Fact(DisplayName = "MustHaveOneSchemeOf must throw an ArgumentNullException when the specified URI is null.")]
        public void UriNull()
        {
            Action act = () => ((Uri) null).MustHaveOneSchemeOf(new[] { "ftp" });

            act.ShouldThrow<ArgumentNullException>();
        }

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Uri("http://foo.com").MustHaveOneSchemeOf(new[] { "https" }, exception: exception)))
                    .Add(new CustomMessageTest<ArgumentException>(message => new Uri("http://foo.com").MustHaveOneSchemeOf(new[] { "ftp" }, message: message)));
        }
    }
}