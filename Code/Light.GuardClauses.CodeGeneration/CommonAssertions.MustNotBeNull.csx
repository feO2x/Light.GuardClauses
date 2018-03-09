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
Console.WriteLine($"The following production code was written to \"{targetFile}\":");
Console.WriteLine(fileContent);

var testFile = Path.Combine("..", "Light.GuardClauses.Tests", "CommonAssertions", "MustNotBeNullTests.cs");
stringBuilder.Clear();
writer = new CSharpCodeWriter(new StringWriter(stringBuilder));

writer.WriteCodeGenerationNotice("CommonAssertions.MustNotBeNull.csx")
      .IncludeNamespace(Namespaces.System)
      .IncludeNamespace(Namespaces.FluentAssertions)
      .IncludeNamespace(Namespaces.Xunit)
      .WriteEmptyLine()
      .OpenNamespace("Light.GuardClauses.Tests.CommonAssertions")
      .OpenPublicStaticPartialClass("MustNotBeNullTests")
      .WriteTheoryAttribute()
      .WriteMetasyntacticVariablesDataAttribute()
      .OpenMember("public static void ParameterNull(string parameterName)")
      .WriteLine("Action act = () => ((object) null).MustNotBeNull(parameterName);")
      .WriteEmptyLine()
      .WriteLine("var throwAssertion = act.Should().Throw<ArgumentNullException>();")
      .WriteLine("throwAssertion.Which.ParamName.Should().BeSameAs(parameterName);")
      .WriteLine("throwAssertion.Which.Message.Should().Contain($\"{parameterName} must not be null.\");")
      .CloseRemainingScopes();

fileContent = stringBuilder.ToString();
File.WriteAllText(testFile, fileContent);
Console.WriteLine($"The following test code was written to \"{testFile}\":");
Console.WriteLine(fileContent);
