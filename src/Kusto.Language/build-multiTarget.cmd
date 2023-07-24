@ECHO OFF

%CECHO% "{color-10}  Going to build Kusto.Language to multiple TargetFrameworks, simulating how this project is compiled in OneBranch.Official.KustoLanguage pipeline (which calls build-kustolanguage.cmd)\n"

REM Setting this variable is required when compiling to multiple TargetFrameworks
SET KUSTO_BuildMultiTarget=true
call dotnet build Kusto.Language.csproj --configuration Release
SET KUSTO_BuildMultiTarget=

:EOF
