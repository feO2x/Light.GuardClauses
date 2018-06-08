using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Xunit;

namespace Light.GuardClauses.Tests.UriAssertions
{
    public static class MustHaveOneSchemeOfTests
    {
        [Theory]
        [InlineData("http://foo.com/", new[] { "ftp", "ftps" })]
        [InlineData("fttps://bar.com/", new[] { "http", "https" })]
        public static void SchemeNotPresent(string uri, string[] schemes)
        {
            Action act = () => new Uri(uri).MustHaveOneSchemeOf(schemes, nameof(uri));

            act.Should().Throw<InvalidUriSchemeException>()
               .And.Message.Should().Contain(new StringBuilder().AppendLine($"{nameof(uri)} must use one of the following schemes")
                                                                .AppendItems(schemes, ErrorMessageExtensions.DefaultNewLineSeparator).AppendLine()
                                                                .Append($"but it actually is \"{uri}\".")
                                                                .ToString());
        }

        [Theory]
        [InlineData("http://www.feO2x.com", new[] { "http", "https" })]
        [InlineData("https://www.microsoft.com", new[] { "http", "https" })]
        public static void SchemePresent(string uri, string[] schemes)
        {
            var instance = new Uri(uri);

            instance.MustHaveOneSchemeOf(schemes).Should().BeSameAs(instance);
        }

        [Fact]
        public static void UriNull()
        {
            Action act = () => ((Uri) null).MustHaveOneSchemeOf(new string[0]);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public static void UriRelative()
        {
            Action act = () => new Uri("/api/foo", UriKind.Relative).MustHaveOneSchemeOf(new string[0]);

            act.Should().Throw<RelativeUriException>();
        }

        [Fact]
        public static void CustomException() =>
            Test.CustomException(new Uri("https://www.microsoft.com"),
                                 new List<string> { "http", "fttp" },
                                 (uri, schemes, exceptionFactory) => uri.MustHaveOneSchemeOf(schemes, exceptionFactory));

        [Fact]
        public static void CustomMessage() =>
            Test.CustomMessage<InvalidUriSchemeException>(message => new Uri("https://go.com").MustHaveOneSchemeOf(new[] { "http" }, message: message));
    }
}