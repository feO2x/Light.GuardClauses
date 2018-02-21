using System;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;

namespace Light.GuardClauses.Tests.UriAssertionsTests
{
    public sealed class MustHaveOneSchemeOfTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustHaveOneSchemeOf must throw an ArgumentException when the specified URI is not an absolute one or does not have one of the specified schemes.")]
        [InlineData("file:///C:/foo.txt", UriKind.Absolute)]
        [InlineData("ftp://localhost:8080", UriKind.Absolute)]
        public void InvalidScheme(string uri, UriKind kind)
        {
            Action act = () => new Uri(uri, kind).MustHaveOneSchemeOf(new[] { "http", "https" }, nameof(uri));

            act.Should().Throw<ArgumentException>()
               .And.Message.Should().Contain($"{nameof(uri)} must use one of the following schemes:");
        }

        [Theory(DisplayName = "MustHaveOneSchemeOf must not throw an exception when the specified URI is an absolute one having one of the specified schemes.")]
        [InlineData("http://www.feo2x.com")]
        [InlineData("https://docs.microsoft.com")]
        public void ValidScheme(string uriText)
        {
            var uri = new Uri(uriText);
            var result = uri.MustHaveOneSchemeOf(new[] { "http", "https" });

            result.Should().BeSameAs(uri);
        }

        [Fact(DisplayName = "MustHaveOneSchemeOf must throw an ArgumentNullException when the specified URI is null.")]
        public void UriNull()
        {
            Action act = () => ((Uri) null).MustHaveOneSchemeOf(new[] { "ftp" });

            act.Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Uri("http://foo.com").MustHaveOneSchemeOf(new[] { "https" }, exception)))
                    .Add(new CustomMessageTest<InvalidUriSchemeException>(message => new Uri("http://foo.com").MustHaveOneSchemeOf(new[] { "ftp" }, message: message)));
        }
    }
}