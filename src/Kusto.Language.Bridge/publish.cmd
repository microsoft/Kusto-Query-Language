@ECHO OFF
SETLOCAL
IF "%PUBLISH_DEBUG%" == "1" ECHO ON

SET ROOT=%~dp0
SET KUSTOROOT=%~dp0..\..\..\
set OUT=%KUSTOROOT%\bin\Release\Projects\Kusto.Language.Bridge\bridge

SET ALL=0
IF "%1" == "" SET ALL=1

IF "%1" == "npm" (CALL :Build & CALL :PublishNpm & GOTO :EOF)

:ArgLoop
IF "%1" == "build" (CALL :Build & SHIFT & GOTO :ArgLoop)
IF "%1" == "npm" (CALL :PublishNpm & SHIFT & GOTO :ArgLoop)
IF "%1" NEQ "" GOTO :ArgLoop

REM If we get here either %1 was empty to begin with (ALL=1)
REM or we've completed doing all that was asked of us (ALL=0)
IF "%ALL%" == "0" GOTO :EOF

CALL :Build
CALL :PublishNpm
%CECHO% "{color-10}Done.\n"
GOTO :EOF

:Build
%CECHO% "{color-10}Building IntelliSense JavaScript...\n"
PUSHD %ROOT%
@REM for some reason, build doesn't work for me from the bridge folder. going up one folder works.
pushd ..
msbuild /p:Configuration=Release
popd
POPD
GOTO :EOF

:: versioning is currently done by specifying version in package.config file.
:: fortunately publish will fail if trying to publish a version that already exists.
:PublishNpm
%CECHO% "{color-10}Packing output directory as an npm package...\n"
where npm
IF %ERRORLEVEL% EQU 1 GOTO :EOF
PUSHD %ROOT%
COPY package.json %OUT%
:: We don't need to worry about .npmrc now that we're publishing to public registry
:: COPY .npmrc %OUT%
PUSHD %OUT%
CALL npm.cmd publish --access=public
DEL package.json
:: DEL .npmrc
POPD
POPD
GOTO :EOF

:Error
%CECHO% "{color-12}%error%"
exit /B 1
:EOF
