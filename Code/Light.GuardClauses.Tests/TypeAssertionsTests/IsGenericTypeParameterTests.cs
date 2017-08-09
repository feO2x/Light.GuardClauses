using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsGenericTypeParameterTests
    {
        [Fact(DisplayName = "IsGenericTypeParameter must return false when the specified type is a non-generic type.")]
        public void NonGenericType()
        {
            GenericTypeKinds.NonGenericType.IsGenericTypeParameter().Should().BeFalse();
        }

        [Fact(DisplayName = "IsGenericTypeParameter must return false when the specified type is a closed constructred generic type.")]
        public void ClosedConstructedType()
        {
            GenericTypeKinds.ClosedConstructedGenericType.IsGenericTypeParameter().Should().BeFalse();
        }

        [Fact(DisplayName = "IsGenericTypeParameter must return false when the specified type is a generic type definition.")]
        public void GenericTypeDefinition()
        {
            GenericTypeKinds.GenericTypeDefinition.IsGenericTypeParameter().Should().BeFalse();
        }

        [Fact(DisplayName = "IsGenericTypeParameter must return false when the specified type is a open constructed generic type.")]
        public void OpenConstructedGenericType()
        {
            GenericTypeKinds.OpenConstructedGenericType.IsGenericTypeParameter().Should().BeFalse();
        }

        [Fact(DisplayName = "IsGenericTypeParameter must return true when the specified type is a generic type parameter.")]
        public void GenericTypeParameter()
        {
            GenericTypeKinds.GenericTypeParameter.IsGenericTypeParameter().Should().BeTrue();
        }

        [Fact(DisplayName = "IsGenericTypeParameter must throw an ArgumentNullException when type is null.")]
        public void ArgumentNull()
        {
            Action act = () => ((Type) null).IsGenericTypeParameter();

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("type");
        }
    }
}