using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Light.GuardClauses.FrameworkExtensions;
using Light.GuardClauses.Tests.CustomMessagesAndExceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class CustomMessagesAndCustomExceptionsTests
    {
        public static readonly IList<Type> OmmitedTestClasses =
            new[]
            {
                typeof(AgainstTests),
                typeof(CustomMessagesAndCustomExceptionsTests),
                typeof(NotNullTests),
                typeof(RangeTests),
                typeof(ThatTests),
                typeof(Metadata),
                typeof(Traits),
                typeof(EqualityTests),
                typeof(IsValidEnumValueTests),
                typeof(IsEmptyTests),
                typeof(IsInTests),
                typeof(IsNotInTests),
                typeof(IsNullOrEmptyTests),
                typeof(IsNullOrWhiteSpaceTests),
                typeof(IsSameAsTests),
                typeof(ContainsOnlyLettersTests),
                typeof(ContainsOnlyLettersAndDigitsTests),
                typeof(FluentApiTests),
                typeof(AsReadOnlyListTests),
                typeof(IsEquivalentToTests),
                typeof(IsDerivingFromTests),
                typeof(IsClassTests),
                typeof(IsDelegateTests),
                typeof(IsStructTests)
            };

        private static readonly List<ICustomMessageAndExceptionTestData> PopulatedTestData = new List<ICustomMessageAndExceptionTestData>();
        private static readonly List<Type> TestClassesWithoutInterfaceImplementation = new List<Type>();
        private static readonly List<Type> WronglyPopulatedTestDataProviders = new List<Type>();

        static CustomMessagesAndCustomExceptionsTests()
        {
            // ReSharper disable PossibleMultipleEnumeration
            var affectedTestClasses = typeof(CustomMessagesAndCustomExceptionsTests).GetTypeInfo()
                                                                                    .Assembly
                                                                                    .ExportedTypes
                                                                                    .Where(t => t.Namespace == "Light.GuardClauses.Tests" && OmmitedTestClasses.Contains(t) == false && t.IsNested == false);

            var testClassesWithInterfaceImplementation = affectedTestClasses.Where(t => t.GetTypeInfo().ImplementedInterfaces.Contains(typeof(ICustomMessageAndExceptionTestDataProvider)));
            TestClassesWithoutInterfaceImplementation.AddRange(affectedTestClasses.Except(testClassesWithInterfaceImplementation));

            var testDataProviders = testClassesWithInterfaceImplementation.Select(Activator.CreateInstance)
                                                                          .Cast<ICustomMessageAndExceptionTestDataProvider>();

            foreach (var testDataProvider in testDataProviders)
            {
                var testData = new CustomMessageAndExceptionTestData(testDataProvider.GetType());
                testDataProvider.PopulateTestDataForCustomExceptionAndCustomMessageTests(testData);

                var interfaceTestData = (ICustomMessageAndExceptionTestData) testData;
                if (interfaceTestData.IsPopulatedCorrectly)
                    PopulatedTestData.Add(interfaceTestData);
                else
                    WronglyPopulatedTestDataProviders.Add(interfaceTestData.TestClassType);
            }
            // ReSharper restore PossibleMultipleEnumeration
        }

        [Theory(DisplayName = "The caller can specify for all extension methods a custom message that has to be injected into the exception object instead of the default message.")]
        [MemberData(nameof(CustomMessageTestData))]
        public void CheckCustomMessage(Type testClassType, IRunnableTest test)
        {
            try
            {
                test.RunTest();
            }
            catch (Exception ex)
            {
                throw new Exception($"The custom message test for class {testClassType.Name} failed - see inner exception for details.", ex);
            }
        }

        public static TestData CustomMessageTestData => PopulatedTestData.SelectMany(testData => testData.CustomMessageTests,
                                                                                     (testData, customMessageTest) => new object[] { testData.TestClassType, customMessageTest });

        [Theory(DisplayName = "The caller can specify for all extension methods a custom exception that must be thrown instead of the default one.")]
        [MemberData(nameof(CustomExceptionTestData))]
        public void CheckCustomException(Type testClassType, IRunnableTest test)
        {
            try
            {
                test.RunTest();
            }
            catch (Exception ex)
            {
                throw new Exception($"The custom exception test for class {testClassType.Name} failed - see inner exception for details.", ex);
            }
        }

        public static TestData CustomExceptionTestData = PopulatedTestData.SelectMany(testData => testData.CustomExceptionTests,
                                                                                      (testData, customExceptionTest) => new object[] { testData.TestClassType, customExceptionTest });

        [Fact(DisplayName = "All test classes of extension methods must populate the custom message and custom exception test data correctly.")]
        [Trait("Category", Traits.InformativeTests)]
        public void WronglyPopulatedTestData()
        {
            if (WronglyPopulatedTestDataProviders.Count == 0)
                return;

            throw new Exception(new StringBuilder("The following test classes do not populate the CustomMessageAndExceptionTestData correctly (please be sure to specify at least one of CustomExceptionTest and CustomMessageTest each):")
                                    .AppendLine()
                                    .AppendItemsWithNewLine(WronglyPopulatedTestDataProviders.Select(p => p.Name))
                                    .AppendLine()
                                    .ToString());
        }

        [Fact(DisplayName = "All test classes of extension methods must implement the ICustomMessageAndExceptionTestDataProvider interface.")]
        [Trait("Category", Traits.InformativeTests)]
        public void TestClassesWithoutInterface()
        {
            if (TestClassesWithoutInterfaceImplementation.Count == 0)
                return;

            throw new Exception(new StringBuilder("The following test classes do not implement the ICustomMessageAndExceptionTestDataProvider interface, although they should:")
                                    .AppendLine()
                                    .AppendItemsWithNewLine(TestClassesWithoutInterfaceImplementation.Select(t => t.Name))
                                    .AppendLine()
                                    .AppendLine()
                                    .Append("If you think that your test class's appearance on this list is unjustified, then adjust the OmmitedTestClasses field in CustomMessagesAndCustomExceptionsTests.")
                                    .ToString());
        }
    }
}