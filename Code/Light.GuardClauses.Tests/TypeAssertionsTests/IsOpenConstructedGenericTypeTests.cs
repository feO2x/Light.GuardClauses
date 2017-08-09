using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.TypeAssertionsTests
{
    [Trait("Category", Traits.FunctionalTests)]
    public sealed class IsOpenConstructedGenericTypeTests
    {
        [Fact(DisplayName = "IsOpenConstructedGenericType must return false when the specified type is a non-generic type.")]
        public void NonGenericType()
        {
            GenericTypeKinds.NonGenericType.IsOpenConstructedGenericType().Should().BeFalse();
        }

        [Fact(DisplayName = "IsOpenConstructedGenericType must return false when the specified type is a closed constructred generic type.")]
        public void ClosedConstructedType()
        {
            GenericTypeKinds.ClosedConstructedGenericType.IsOpenConstructedGenericType().Should().BeFalse();
        }

        [Fact(DisplayName = "IsOpenConstructedGenericType must return false when the specified type is a generic type definition.")]
        public void GenericTypeDefinition()
        {
            GenericTypeKinds.GenericTypeDefinition.IsOpenConstructedGenericType().Should().BeFalse();
        }

        [Fact(DisplayName = "IsOpenConstructedGenericType must return true when the specified type is a open constructed generic type.")]
        public void OpenConstructedGenericType()
        {
            GenericTypeKinds.OpenConstructedGenericType.IsOpenConstructedGenericType().Should().BeTrue();
        }

        [Fact(DisplayName = "IsOpenConstructedGenericType must return false when the specified type is a generic type parameter.")]
        public void GenericTypeParameter()
        {
            GenericTypeKinds.GenericTypeParameter.IsOpenConstructedGenericType().Should().BeFalse();
        }

        [Fact(DisplayName = "IsOpenConstructedGenericType must throw an ArgumentNullException when type is null.")]
        public void ArgumentNull()
        {
            Action act = () => ((Type) null).IsOpenConstructedGenericType();

            act.ShouldThrow<ArgumentNullException>()
               .And.ParamName.Should().Be("type");
        }
    }
}