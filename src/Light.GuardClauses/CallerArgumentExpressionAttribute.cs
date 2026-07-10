#if !NET8_0_OR_GREATER
// ReSharper disable once CheckNamespace -- CallerArgumentExpression must be in exactly this namespace
namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Parameter)]
internal sealed class CallerArgumentExpressionAttribute : Attribute
{
    public CallerArgumentExpressionAttribute(string parameterName) =>
        ParameterName = parameterName;

    public string ParameterName { get; }
}
#endif
