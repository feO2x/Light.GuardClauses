using System;
using System.Collections.Generic;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests.DictionaryAssertionsTests
{
    public sealed class MustNotContainValueTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustNotContainValue must throw a DictionaryException when the specified value is part of the dictionary.")]
        [MemberData(nameof(ValueContainedData))]
        public void ValueContained(IDictionary<string, object> dictionary, object value)
        {
            Action act = () => dictionary.MustNotContainValue(value, nameof(dictionary));

            act.ShouldThrow<DictionaryException>()
               .And.Message.Should().Contain($"{nameof(dictionary)} must not contain value \"{value.ToStringOrNull()}\", but it does.");
        }

        public static readonly TestData ValueContainedData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["First"] = 1, ["Second"] = 2L, ["Third"] = "What?" }, 2L },
                new object[] { new Dictionary<string, object> { ["1"] = "Hello", ["2"] = null }, null }
            };

        [Theory(DisplayName = "MustNotContainValue must not throw an exception when the specified value is not part of the dictionary.")]
        [MemberData(nameof(ValueNotContainedData))]
        public void ValueNotContained(IDictionary<string, object> dictionary, object value)
        {
            Action act = () => dictionary.MustNotContainValue(value);

            act.ShouldNotThrow();
        }

        public static readonly TestData ValueNotContainedData =
            new[]
            {
                new object[] { new Dictionary<string, object> { ["Foo"] = "Bar", ["Baz"] = "Qux" }, "Quux" },
                new object[] { new Dictionary<string, object> { ["First"] = 1, ["Second"] = null }, 42 }
            };

        [Fact(DisplayName = "MustNotContainValue must throw an ArgumentNullException when the dictionary is null.")]
        public void DictionaryNull()
        {
            Action act = () => ((IDictionary<int, string>) null).MustNotContainValue("Foo");

            act.ShouldThrow<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Dictionary<int, string> { [1] = "Foo" }.MustNotContainValue("Foo", exception: exception)))
                    .Add(new CustomMessageTest<DictionaryException>(message => new Dictionary<int, string> { [42] = "Bar" }.MustNotContainValue("Bar", message: message)));
        }
    }
}