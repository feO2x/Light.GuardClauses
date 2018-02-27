#! "netcoreapp2.0"
#r "nuget: Light.GuardClauses 3.5.0"
#load "CSharpCodeWriter.csx"

using Light.GuardClauses;

public abstract class AssertionOverload
{
    public abstract string Parameters { get; }
    public abstract string ThrowStatement { get; }
    public abstract string ExceptionTextForSummary { get; }

    public abstract AssertionOverload WriteXmlCommentForParameters(CSharpCodeWriter writer);
    public abstract CSharpCodeWriter WriteXmlCommentForException(CSharpCodeWriter writer);
}

public sealed class DefaultOverload : AssertionOverload
{
    private readonly string _exceptionType;
    private readonly string _exceptionReason;

    public DefaultOverload(string throwStatement, string exceptionType, string exceptionReason, string nounMarker = "a")
    {
        ThrowStatement = throwStatement.MustNotBeNullOrWhiteSpace(nameof(throwStatement));
        _exceptionType = exceptionType.MustNotBeNullOrWhiteSpace(nameof(exceptionType));
        ExceptionTextForSummary = $"{nounMarker} {XmlComment.ToSee(exceptionType)}";
        _exceptionReason = exceptionReason.MustNotBeNullOrWhiteSpace(nameof(exceptionReason));
    }

    public override string Parameters => "string parameterName = null, string message = null";

    public override string ThrowStatement { get; }

    public override string ExceptionTextForSummary { get; }

    public override CSharpCodeWriter WriteXmlCommentForException(CSharpCodeWriter writer) => writer.WriteXmlCommentException(_exceptionType, $"Thrown {_exceptionReason}.");

    public override AssertionOverload WriteXmlCommentForParameters(CSharpCodeWriter writer)
    {
        writer.WriteDefaultXmlCommentForParameterName()
              .WriteDefaultXmlCommentForMessage("InvalidUriSchemeException");
        return this;
    }
}

public sealed class CustomExceptionOverload : AssertionOverload
{
    private readonly Action<CSharpCodeWriter> _writeXmlCommentForAdditionalParameters;
    private readonly string _exceptionReason;

    public CustomExceptionOverload(string exceptionReason)
    {
        _exceptionReason = exceptionReason.MustNotBeNullOrWhiteSpace(nameof(exceptionReason));
        Parameters = "Func<Exception> exceptionFactory";

    }

    public CustomExceptionOverload(string exceptionReason, string additionalParameters, Action<CSharpCodeWriter> writeXmlCommentForAdditionalParameters)
    {
        _exceptionReason = exceptionReason.MustNotBeNullOrWhiteSpace(nameof(exceptionReason));
        _writeXmlCommentForAdditionalParameters = writeXmlCommentForAdditionalParameters.MustNotBeNull(nameof(writeXmlCommentForAdditionalParameters));
        Parameters = $"Func<Exception> exceptionFactory, {additionalParameters}";
    }

    public override string Parameters { get; }

    public override string ThrowStatement => "Throw.CustomException(exceptionFactory);";

    public override string ExceptionTextForSummary => "your custom exception";

    public override CSharpCodeWriter WriteXmlCommentForException(CSharpCodeWriter writer) => writer.WriteXmlCommentException("Exception", $"Your custom exception thrown {_exceptionReason}.");

    public override AssertionOverload WriteXmlCommentForParameters(CSharpCodeWriter writer)
    {
        writer.WriteXmlCommentParam("exceptionFactory", "The delegate that creates the exception to be thrown.");
        _writeXmlCommentForAdditionalParameters?.Invoke(writer);
        return this;
    }
}

public sealed class CustomExceptionOverloadWithOneParameter : AssertionOverload
{
    private readonly string _exceptionReason;
    private readonly string _parameterName;
    private readonly Action<CSharpCodeWriter> _writeXmlCommentForAdditionalParameters;

    public CustomExceptionOverloadWithOneParameter(string parameterType, string exceptionReason, string parameterName = "parameter")
    {
        Parameters = $"Func<{parameterType}, Exception> exceptionFactory";
        _exceptionReason = exceptionReason.MustNotBeNullOrWhiteSpace(nameof(exceptionReason));
        _parameterName = parameterName.MustNotBeNullOrWhiteSpace(nameof(parameterName));
        ThrowStatement = $"Throw.CustomException(exceptionFactory, {_parameterName});";
    }

    public CustomExceptionOverloadWithOneParameter(string parameterType, string exceptionReason, string additionalParameters, Action<CSharpCodeWriter> writeXmlCommentForAdditionalParameters, string parameterName = "parameter")
    {
        Parameters = $"Func<{parameterType}, Exception> exceptionFactory, {additionalParameters}";
        _exceptionReason = exceptionReason.MustNotBeNullOrWhiteSpace(nameof(exceptionReason));
        _parameterName = parameterName.MustNotBeNullOrWhiteSpace(nameof(parameterName));
        _writeXmlCommentForAdditionalParameters = writeXmlCommentForAdditionalParameters;
        ThrowStatement = $"Throw.CustomException(exceptionFactory, {_parameterName});";
    }

    public override string Parameters { get; }
    public override string ThrowStatement { get; }
    public override string ExceptionTextForSummary => "your custom exception";

    public override CSharpCodeWriter WriteXmlCommentForException(CSharpCodeWriter writer) => writer.WriteXmlCommentException("Exception", $"Your custom exception thrown {_exceptionReason}.");
    public override AssertionOverload WriteXmlCommentForParameters(CSharpCodeWriter writer)
    {
        writer.WriteXmlCommentParam("exceptionFactory", "The delegate that creates the exception to be thrown.");
        _writeXmlCommentForAdditionalParameters?.Invoke(writer);
        return this;
    }
}