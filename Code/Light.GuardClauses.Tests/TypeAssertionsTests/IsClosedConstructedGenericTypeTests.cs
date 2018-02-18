using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    public sealed class IsClosedConstructedGenericTypeTests
    {
        [Fact(DisplayName = "IsClosedConstructedGenericType must return false when the specified type is a non-generic type.")]
        public void NonGenericType()
        {
            GenericTypeKinds.NonGenericType.IsClosedConstructedGenericType().Should().BeFalse();
        }

        [Fact(DisplayName = "IsClosedConstructedGenericType must return true when the specified type is a closed constructred generic type.")]
        public void ClosedConstructedType()
        {
            GenericTypeKinds.ClosedConstructedGenericType.IsClosedConstructedGenericType().Should().BeTrue();
        }

        [Fact(DisplayName = "IsClosedConstructedGenericType must return false when the specified type is a generic type definition.")]
        public void GenericTypeDefinition()
        {
            GenericTypeKinds.GenericTypeDefinition.IsClosedConstructedGenericType().Should().BeFalse();
        }

        [Fact(DisplayName = "IsClosedConstructedGenericType must return false when the specified type is a open constructed generic type.")]
        public void OpenConstructedGenericType()
        {
            GenericTypeKinds.OpenConstructedGenericType.IsClosedConstructedGenericType().Should().BeFalse();
        }

        [Fact(DisplayName = "IsClosedConstructedGenericType must return false when the specified type is a generic type parameter.")]
        public void GenericTypeParameter()
        {
            GenericTypeKinds.GenericTypeParameter.IsClosedConstructedGenericType().Should().BeFalse();
        }

        [Fact(DisplayName = "IsClosedConstructedGenericType must throw an ArgumentNullException when type is null.")]
        public void ArgumentNull()
        {
            Action act = () => ((Type) null).IsClosedConstructedGenericType();

            act.Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("type");
        }
    }
}