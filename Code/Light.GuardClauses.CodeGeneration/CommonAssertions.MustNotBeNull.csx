#! "netcoreapp2.0"
#load "CSharpCodeWriter.csx"
#load "AssertionOverload.csx"
#load "Namespaces.csx"
#load "GuardClauseInfo.csx"

var reason = $"when {XmlComment.ToParamRef("parameter")} is null.";
var overloads = new AssertionOverload[]
{
    new DefaultOverload("Throw.MustNotBeNull(parameterName, message);", "ArgumentNullException", reason, "an"),
    new CustomExceptionOverload(reason)
};
var guardClauseInfo = new GuardClauseInfo("MustNotBeNull",
                                          "CommonAssertions",
                                          new Parameter("T", true),
                                          overloads,
                                          new [] { Namespaces.System, Namespaces.SystemRuntimeCompilerServices, Namespaces.LightGuardClausesExceptions},
                                          "Ensures that the specified reference is not null",
                                          "The reference to be checked",
                                          (context, writer, overload) => 
                                                writer.WriteLine("if (parameter == null)")
                                                      .IncreaseIndentation()
                                                      .WriteLine(overload.ThrowStatement)
                                                      .DecreaseIndentation()
                                                      .WriteLine("return parameter;")
                                          );
guardClauseInfo.WriteGuardClauseOverloads();

var testFile = Path.Combine("..", "Light.GuardClauses.Tests", "CommonAssertions", "MustNotBeNullTests.cs");
var stringBuilder = new StringBuilder();
var writer = new CSharpCodeWriter(new StringWriter(stringBuilder));

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

var fileContent = stringBuilder.ToString();
File.WriteAllText(testFile, fileContent);
Console.WriteLine($"The following test code was written to \"{testFile}\":");
Console.WriteLine(fileContent);
