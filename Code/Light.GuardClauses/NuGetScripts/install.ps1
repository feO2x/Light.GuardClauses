param($installPath, $toolsPath, $package, $project)

# Constants and Variables
$condiditionalCompilationSymbolsId = "DefineConstants"
$compileAssertionsSymbol = "COMPILE_ASSERTIONS"
$shouldSave = $false

# Run through each configuration (Debug, Release, and so on) and set the COMPILE_ASSERTIONS symbol if necessary
foreach($configuration in $project.ConfigurationManager)
{
    $conditionalCompilationSymbols = $configuration.Properties.Item($condiditionalCompilationSymbolsId).Value

	# Check if the COMPILE_ASSERTIONS symbol is present
    if ($conditionalCompilationSymbols.Contains($compileAssertionsSymbol) -eq $false)
    {
		# If not, then add it and mark the flag that the project should be saved afterwards
        $shouldSave = $true
        if ($conditionalCompilationSymbols.Length -gt 0)
        {
			# If the Lenght is greater than zero, there are already conditional compilation symbols present, so our one has to be separated with a semicolon
            $conditionalCompilationSymbols = $conditionalCompilationSymbols + ";"
        }

        $conditionalCompilationSymbols = $conditionalCompilationSymbols + $compileAssertionsSymbol
        $configuration.Properties.Item($condiditionalCompilationSymbolsId).Value = $conditionalCompilationSymbols
    }
}

if ($shouldSave -eq $true)
{
    $project.Save()
    Write-Host "Light.GuardClauses: $compileAssertionsSymbol symbol added"
}
