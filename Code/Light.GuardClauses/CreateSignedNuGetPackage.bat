copy bin\Release\Light.GuardClauses.dll bin\signedRelease\ /Y
copy bin\Release\Light.GuardClauses.xml bin\signedRelease\ /Y

ildasm bin\signedRelease\Light.GuardClauses.dll /out:bin\signedRelease\Light.GuardClauses.il
del bin\signedRelease\Light.GuardClauses.dll
ilasm bin\signedRelease\Light.GuardClauses.il /dll /key=bin\signedRelease\Light.GuardClauses.snk
del bin\signedRelease\Light.GuardClauses.il
del bin\signedRelease\Light.GuardClauses.res

nuget pack Light.GuardClauses.nuspec