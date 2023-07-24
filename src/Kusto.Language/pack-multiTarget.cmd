@ECHO OFF

%CECHO% "{color-10}  Going to pack Kusto.Language, simulating how this project is compiled in OneBranch.Official.KustoLanguage pipeline\n"

SET KUSTO_BuildMultiTarget=true
call dotnet pack Kusto.Language.csproj /p:Configuration=Release /p:Platform="Any CPU" --output %KUSTOROOT%\bin\packages --no-build
SET KUSTO_BuildMultiTarget=

:EOF
