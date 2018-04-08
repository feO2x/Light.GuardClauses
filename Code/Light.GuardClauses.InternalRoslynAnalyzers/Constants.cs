using System;

namespace Light.GuardClauses.InternalRoslynAnalyzers
{
    public static class ParameterNameConstants
    {
        public const string ParameterName = "parameterName";
        public const string DefaultComment = "The name of the parameter (optional).";
    }

    public static class MessageConstants
    {
        public const string ParameterName = "message";
        public const string FullDefaultComment = CommentStart + "the resulting exception" + CommentEnd;
        public const string CommentStart = "The message that will be passed to ";
        public const string CommentEnd = " (optional).";
    }

    public static class ExceptionFactoryConstants
    {
        public static readonly Type FuncOfTType = typeof(Func<>);
        public static readonly Type ExceptionType = typeof(Exception);
        public const string ParameterName = "exceptionFactory";
    }
}