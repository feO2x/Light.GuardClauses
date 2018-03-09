
// PLEASE NOTICE: this code is auto-generated. Any changes you make to this file are going to be overwritten when this file is recreated by the code generator.
// Check "CommonAssertions.MustNotBeNull.csx" for details.

using System;
using FluentAssertions;
using Xunit;

namespace Light.GuardClauses.Tests.CommonAssertions
{
    public static partial class MustNotBeNullTests
    {
        [Theory]
        [MetasyntacticVariablesData]
        public static void ParameterNull(string parameterName)
        {
            Action act = () => ((object) null).MustNotBeNull(parameterName);
            
            var throwAssertion = act.Should().Throw<ArgumentNullException>();
            throwAssertion.Which.ParamName.Should().BeSameAs(parameterName);
            throwAssertion.Which.Message.Should().Contain($"{parameterName} must not be null.");
        }
    }
}
