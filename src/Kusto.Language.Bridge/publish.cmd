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
CALL :MSbuildSetup
IF ERRORLEVEL 1 EXIT /B %ERRORLEVEL%
echo Building IntelliSense JavaScript...
PUSHD %ROOT%
echo "%KUSTO_DETECTED_MSBUILD%" /Restore /p:Configuration=Release
"%KUSTO_DETECTED_MSBUILD%"  /Restore /p:Configuration=Release
SET BUILD_ERROR_LEVEL=%ERRORLEVEL%
POPD
EXIT /B %BUILD_ERROR_LEVEL%

:: versioning is currently done by specifying version in package.json.
:: publish will fail if trying to publish a version that already exists.
:PublishNpm
ECHO Packing output directory as an npm package...
where npm
IF %ERRORLEVEL% EQU 1 GOTO :EOF
PUSHD %ROOT%
COPY package.json %OUT%
COPY .npmrc %OUT%
PUSHD %OUT%
CALL npm.cmd publish --access=public
SET PUBLISH_ERROR_LEVEL=%ERRORLEVEL%
DEL package.json
DEL .npmrc
IF %PUBLISH_ERROR_LEVEL% EQU 1 exit /b 1
POPD
POPD
GOTO :EOF

:Error
echo %error%
exit /B 1
:EOF

REM---------------------------------------------
REM Set MSBuild full path in KUSTO_DETECTED_MSBUILD
REM---------------------------------------------
:MSbuildSetup

echo Find MSBuild path
SET "_VSWHERE="
IF EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe" SET "_VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
IF NOT DEFINED _VSWHERE IF EXIST "%ProgramFiles%\Microsoft Visual Studio\Installer\vswhere.exe" SET "_VSWHERE=%ProgramFiles%\Microsoft Visual Studio\Installer\vswhere.exe"
IF NOT DEFINED _VSWHERE GOTO :MSBuildNotFound

SET "_VS_INSTALL_PATH="
FOR /F "delims=" %%I IN ('"%_VSWHERE%" -latest -products * -requires Microsoft.Component.MSBuild -property installationPath') DO SET "_VS_INSTALL_PATH=%%I"
IF NOT DEFINED _VS_INSTALL_PATH GOTO :MSBuildNotFound
IF NOT EXIST "%_VS_INSTALL_PATH%\Common7\Tools\VsDevCmd.bat" GOTO :MSBuildNotFound

ECHO Located Visual Studio: %_VS_INSTALL_PATH%
SET KUSTO_DETECTED_MSBUILD=%_VS_INSTALL_PATH%\MSBuild\Current\Bin\MSBuild.exe
IF NOT EXIST "%KUSTO_DETECTED_MSBUILD%" GOTO :MSBuildNotFound
EXIT /B 0

:MSBuildNotFound
ECHO Error: Failed to locate MSBuild.
EXIT /B 1
