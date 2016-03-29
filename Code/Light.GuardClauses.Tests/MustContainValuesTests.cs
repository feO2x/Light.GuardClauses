using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public sealed class MustContainValuesTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustContainValues must throw a ValueNotFoundException when at least one of the specified values is not present in the dicionary.")]
        [MemberData(nameof(ValuesNotContainedData))]
        public void ValuesNotContained(IDictionary<string, object> dictionary, IEnumerable<object> values)
        {
            Action act = () => dictionary.MustContainValues(values, nameof(dictionary));

            act.ShouldThrow<ValueNotFoundException>()
               .And.Message.Should().Contain($"{nameof(dictionary)} must contain all the following values:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(values)}");
        }

        public static readonly TestData ValuesNotContainedData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["First"] = "Foo", ["Second"] = "Bar" }, new[] { "1", "2", "3" } },
                new object[] { new Dictionary<string, object> { ["1"] = "Foo", ["2"] = null, ["3"] = 42 }, new object[] { "other value", 42 } }
            };

        [Theory(DisplayName = "MustContainValues must not throw an exception when the specified values are all part of the dictionary.")]
        [MemberData(nameof(ValuesContainedData))]
        public void ValuesContained(IDictionary<string, object> dictionary, object[] values)
        {
            Action act = () => dictionary.MustContainValues(values);

            act.ShouldNotThrow();
        }

        public static readonly TestData ValuesContainedData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["First"] = "Foo", ["Second"] = "Bar", ["Third"] = "Baz" }, new[] { "Foo", "Bar", "Baz" } },
                new object[] { new Dictionary<string, object> { ["1"] = "Foo", ["2"] = null, ["3"] = 42 }, new object[] { "Foo", 42 } },
                new object[] { new Dictionary<string, object> { ["1"] = "Foo", ["2"] = null, ["3"] = 42 }, new object[] { "Foo", null, "Foo" } }
            };

        [Theory(DisplayName = "MustContainValues must throw an ArgumentNullException when parameter or values is null.")]
        [MemberData(nameof(ArgumentNullData))]
        public void ArgumentNull(IDictionary<char, object> dictionary, object[] values)
        {
            Action act = () => dictionary.MustContainValues(values);

            act.ShouldThrow<ArgumentNullException>();
        }

        public static readonly TestData ArgumentNullData =
            new[]
            {
                new object[] { new Dictionary<char, object>(), null },
                new object[] { null, new object[2] }
            };

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Dictionary<int, string> { [1] = "Foo" }.MustContainValues(new[] { "Bar", "Baz" }, exception: exception)));

            testData.Add(new CustomMessageTest<ValueNotFoundException>(message => new Dictionary<int, string> { [1] = "Foo" }.MustContainValues(new[] { "Bar", "Baz" }, message: message)));
        }
    }
}