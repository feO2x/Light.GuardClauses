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
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class MustNotContainValuesTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotContainValues must throw a DictionaryException when any of the specified values is not part of the dictionary.")]
        [MemberData(nameof(ValuesContainedData))]
        public void ValuesContained(IDictionary<string, object> dictionary, IEnumerable<object> values)
        {
            Action act = () => dictionary.MustNotContainValues(values, nameof(dictionary));

            act.ShouldThrow<DictionaryException>()
               .And.Message.Should().Contain($"{nameof(dictionary)} must not contain any of the following values:{Environment.NewLine}{new StringBuilder().AppendItemsWithNewLine(values)}");
        }

        public static readonly TestData ValuesContainedData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["Foo"] = "Bar", ["Baz"] = "Qux" }, new[] { "Bar", "Qux" } },
                new object[] { new Dictionary<string, object> { ["First"] = 42, ["Second"] = 8718718, ["Third"] = null }, new object[] { null, 42 } }
            };

        [Theory(DisplayName = "MustNotContainValues must not throw an exception when the dictionary does not contain any of the specified values.")]
        [MemberData(nameof(ValuesNotContainedData))]
        public void ValuesNotContained(IDictionary<string, object> dictionary, object[] values)
        {
            Action act = () => dictionary.MustNotContainValues(values);

            act.ShouldNotThrow();
        }

        public static readonly TestData ValuesNotContainedData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["Foo"] = "Bar", ["Baz"] = "Qux" }, new[] { "Quux", "Corge" } },
                new object[] { new Dictionary<string, object> { ["First"] = 2, ["Second"] = 4, ["Third"] = 8 }, new object[] { 3, null } }
            };

        [Theory(DisplayName = "MustNotContainValues must throw an ArgumentNullException when parameter or values is null.")]
        [MemberData(nameof(ArgumentNullData))]
        public void ArgumentNull(IDictionary<string, object> dictionary, object[] values)
        {
            Action act = () => dictionary.MustNotContainValues(values);

            act.ShouldThrow<ArgumentNullException>();
        }

        public static readonly TestData ArgumentNullData =
            new[]
            {
                new object[] { new Dictionary<string, object>(), null },
                new object[] { null, new object[0] }
            };

        public void PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Dictionary<int, string> { [1] = "Foo", [2] = "Bar" }.MustNotContainValues(new[] { "Bar", "foo" }, exception: exception)));

            testData.Add(new CustomMessageTest<DictionaryException>(message => new Dictionary<int, string> { [1] = "Foo", [2] = "Bar" }.MustNotContainValues(new[] { "Bar", "foo" }, message: message)));
        }
    }
}