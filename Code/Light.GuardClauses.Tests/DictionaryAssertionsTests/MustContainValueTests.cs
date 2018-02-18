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
    public sealed class MustContainValueTests : ICustomMessageAndExceptionTestDataProvider
    {
        [Theory(DisplayName = "MustContainValue must throw a ValueNotFoundException when the specified value is not part of the dictionary.")]
        [MemberData(nameof(ValueNotContainedData))]
        public void ValueNotContained(IDictionary<string, string> dictionary, string value)
        {
            Action act = () => dictionary.MustContainValue(value, nameof(dictionary));

            act.Should().Throw<ValueNotFoundException>()
               .And.Message.Should().Contain($"{nameof(dictionary)} must contain value \"{value.ToStringOrNull()}\", but it does not.");
        }

        public static readonly TestData ValueNotContainedData =
            new[]
            {
                new object[] { new Dictionary<string, string> { ["Hello"] = "World" }, "There" },
                new object[] { new Dictionary<string, string> { ["1"] = "Pardon me", ["2"] = "Where is my robe?" }, null }
            };

        [Theory(DisplayName = "MustContainValue must not throw an exception when the specified value is part of the dictionary.")]
        [MemberData(nameof(ValueContainedData))]
        public void ValueContained(IDictionary<string, string> dictionary, string value)
        {
            var result = dictionary.MustContainValue(value);

            result.Should().BeSameAs(dictionary);
        }

        public static readonly TestData ValueContainedData =
            new[]
            {
                new object[] { new Dictionary<string, string> { ["First Entry"] = "42", ["Second Entry"] = "-124142566" }, "42" },
                new object[] { new Dictionary<string, string> { ["First Entry"] = "Foo", ["Second Entry"] = null, ["Third Entry"] = "Baz" }, null }
            };

        [Fact(DisplayName = "MustContainValue must throw an ArgumentNullException when the dictionary is null.")]
        public void DictionaryNull()
        {
            Action act = () => ((Dictionary<string, string>) null).MustContainValue("Foo");

            act.Should().Throw<ArgumentNullException>();
        }

        void ICustomMessageAndExceptionTestDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(CustomMessageAndExceptionTestData testData)
        {
            testData.Add(new CustomExceptionTest(exception => new Dictionary<int, string> { [1] = "Foo" }.MustContainValue("Bar", exception: exception)))
                    .Add(new CustomMessageTest<ValueNotFoundException>(message => new Dictionary<int, string> { [1] = "Foo" }.MustContainValue("Bar", message: message)));
        }
    }
}