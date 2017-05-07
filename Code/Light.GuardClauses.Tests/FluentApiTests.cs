using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class FluentApiTests
    {
        public static readonly List<MethodInfo> AssertionExtensionMethods;
        public static readonly TestData ReturnsInputValueData;

        static FluentApiTests()
        {
            var omittedMethods = new[]
                                 {
                                     nameof(CommonAssertions.MustBeOfType)
                                 };

            AssertionExtensionMethods = typeof(Check).GetTypeInfo()
                                                     .Assembly
                                                     .ExportedTypes
                                                     .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                                                     .Where(method => method.Name.StartsWith("Must") && 
                                                                      method.IsDefined(typeof(ExtensionAttribute)) &&
                                                                      omittedMethods.Contains(method.Name) == false)
                                                     .ToList();

            ReturnsInputValueData = AssertionExtensionMethods.Select(method => new object[] { method }).ToArray();
        }

        [Theory(DisplayName = "Every assertion method of Light.GuardClauses must return the value that it checks.")]
        [MemberData(nameof(ReturnsInputValueData))]
        public void ReturnsInputValue(MethodInfo assertionExtensionMethod)
        {
            var paramters = assertionExtensionMethod.GetParameters();
            paramters.Should().NotBeEmpty($"The method \"{assertionExtensionMethod.Name}\" is no assertion method because it does not take parameters.");

            assertionExtensionMethod.ReturnType.Should().Be(paramters[0].ParameterType);
        }
    }
}