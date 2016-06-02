param($installPath, $toolsPath, $package, $project)

# Constants and Variables
$condiditionalCompilationSymbolsId = "DefineConstants"
$compileAssertionsSymbol = "COMPILE_ASSERTIONS"
$shouldSave = $false

# This function removes all instances of COMPILE_ASSERTIONS from the given conditional compilation symbols
function Remove-CompileAssertionsSymbol([string]$conditionalCompilationSymbols)
{
    if ($conditionalCompilationSymbols.Length -eq 0)
    {
        return $conditionalCompilationSymbols
    }

    if ($conditionalCompilationSymbols -eq $compileAssertionsSymbol)
    {
        return ""
    }

    $conditionalCompilationSymbols = $conditionalCompilationSymbols -replace "$compileAssertionsSymbol;", ""
    $conditionalCompilationSymbols = $conditionalCompilationSymbols -replace ";$compileAssertionsSymbol", ""
    return $conditionalCompilationSymbols
}

# Run through all the configurations (Debug, Release, and so on) and remove the COMPILE_ASSERTIONS symbol
foreach ($configuration in $project.ConfigurationManager)
{
    $conditionalCompilationSymbols = $configuration.Properties.Item($condiditionalCompilationSymbolsId).Value

    $newConditionalCompilationSymbols = Remove-CompileAssertionsSymbol($conditionalCompilationSymbols)

    # If the conditional compilation symbols changed, then set them on the configuration and remember to save the project
    if ($condiditionalCompilationSymbols -ne $newConditionalCompilationSymbols)
    {
        $shouldSave = $true

        $configuration.Properties.Item($condiditionalCompilationSymbolsId).Value = $newConditionalCompilationSymbols
    }
}

if ($shouldSave)
{
    $project.Save()
    Write-Host "Light.GuardClauses: $compileAssertionsSymbol removed"
}