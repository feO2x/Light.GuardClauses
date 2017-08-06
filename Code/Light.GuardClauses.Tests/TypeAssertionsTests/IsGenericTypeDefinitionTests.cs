using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsGenericTypeDefinitionTests
    {
        [Fact(DisplayName = "IsGenericTypeDefinition must return false when the specified type is a non-generic type.")]
        public void NonGenericType()
        {
            GenericTypeKinds.NonGenericType.IsGenericTypeDefinition().Should().BeFalse();
        }

        [Fact(DisplayName = "IsGenericTypeDefinition must return false when the specified type is a closed constructred generic type.")]
        public void ClosedConstructedType()
        {
            GenericTypeKinds.ClosedConstructedGenericType.IsGenericTypeDefinition().Should().BeFalse();
        }

        [Fact(DisplayName = "IsGenericTypeDefinition must return true when the specified type is a generic type definition.")]
        public void GenericTypeDefinition()
        {
            GenericTypeKinds.GenericTypeDefinition.IsGenericTypeDefinition().Should().BeTrue();
        }

        [Fact(DisplayName = "IsGenericTypeDefinition must return false when the specified type is a open constructed generic type.")]
        public void OpenConstructedGenericType()
        {
            GenericTypeKinds.OpenConstructedGenericType.IsGenericTypeDefinition().Should().BeFalse();
        }

        [Fact(DisplayName = "IsGenericTypeDefinition must return false when the specified type is a generic type parameter.")]
        public void GenericTypeParameter()
        {
            GenericTypeKinds.GenericTypeParameter.IsGenericTypeDefinition().Should().BeFalse();
        }

        [Fact(DisplayName = "IsGenericTypeDefinition must throw an ArgumentNullException when type is null.")]
        public void ArgumentNull()
        {
            Action act = () => ((Type) null).IsGenericTypeDefinition();

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("type");
        }
    }
}