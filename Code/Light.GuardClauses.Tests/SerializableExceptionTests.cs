#if (NETCOREAPP2_0 || NET45)
using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Light.GuardClauses.Exceptions;
using Xunit;
using TestData = System.Collections.Generic.IEnumerable<object[]>;

namespace Light.GuardClauses.Tests
{
    public static class SerializableExceptionTests
    {
        public static readonly TestData ExceptionTypesData =
            typeof(StringException).Assembly
                                   .ExportedTypes
                                   .Where(type => type.Namespace == "Light.GuardClauses.Exceptions" && type.IsDerivingFrom(typeof(Exception)))
                                   .Select(exceptionType => new object[] { exceptionType });


        [Theory(DisplayName = "All exception types of Light.GuardClauses should use the SerializableAttribute.")]
        [MemberData(nameof(ExceptionTypesData))]
        public static void SerializableExceptions(Type exceptionType)
        {
            exceptionType.GetCustomAttribute<SerializableAttribute>().Should().NotBeNull($"the type \"{exceptionType}\" must be marked with the SerializableAttribute");
        }
    }
}
#endif