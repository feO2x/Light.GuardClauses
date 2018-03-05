#! "netcoreapp2.0"
#load "CSharpCodeWriter.csx"
#load "AssertionOverload.csx"
#load "Namespaces.csx"

var targetFile = Path.Combine("..", "Light.GuardClauses", "CommonAssertions.MustNotBeNull.cs");
var stringBuilder = new StringBuilder();
var writer = new CSharpCodeWriter(new StringWriter(stringBuilder));

var reason = $"when {XmlComment.ToParamRef("parameter")} is null.";

var overloads = new AssertionOverload[]
{
    new DefaultOverload("Throw.MustNotBeNull(parameterName, message);", "ArgumentNullException", reason, "an"),
    new CustomExceptionOverload(reason)
};

writer.WriteCodeGenerationNotice("CommonAssertions.MustNotBeNull.csx")
      .IncludeNamespace(Namespaces.System)
      .IncludeNamespace(Namespaces.SystemRuntimeCompilerServices)
      .IncludeNamespace(Namespaces.LightGuardClausesExceptions)
      .WriteEmptyLine()
      .OpenNamespace(Namespaces.LightGuardClauses)
      .OpenPublicStaticPartialClass("CommonAssertions");

foreach(var overload in overloads)
{
    writer.WriteXmlCommentSummary($"Ensures that the specified parameter is not null, or otherwise throws {overload.ExceptionTextForSummary}.")
          .WriteXmlCommentParam("parameter", "The reference to be checked.");
    overload.WriteXmlCommentForParameters(writer)
            .WriteXmlCommentForException(writer)
            .WriteAggressiveInliningAttribute()
            .OpenMember($"public static T MustNotBeNull<T>(this T parameter, {overload.Parameters}) where T : class")
            .WriteLine("if (parameter == null)")
            .IncreaseIndentation()
            .WriteLine(overload.ThrowStatement)
            .DecreaseIndentation()
            .WriteLine("return parameter;")
            .CloseScope()
            .WriteEmptyLine();
}

writer.CloseRemainingScopes();

var fileContent = stringBuilder.ToString();
File.WriteAllText(targetFile, fileContent);
Console.WriteLine($"The following was written to \"{fileContent}\":");
Console.WriteLine(fileContent);