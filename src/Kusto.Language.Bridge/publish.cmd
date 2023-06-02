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
echo Building IntelliSense JavaScript...
PUSHD %ROOT%
echo "%KUSTO_DETECTED_MSBUILD%" /Restore /p:Configuration=Release
"%KUSTO_DETECTED_MSBUILD%"  /Restore /p:Configuration=Release
POPD
GOTO :EOF

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
where.exe devenv.exe >NUL 2e>NUL
IF ERRORLEVEL 1 GOTO :AutoDetectVisualStudio
FOR /F "delims=*" %%a in ('where.exe devenv.exe') DO SET VSEDITCMDLINE="%%a"
where.exe msbuild.exe > NUL 2>NUL
IF ERRORLEVEL 1 GOTO :AutoDetectVisualStudio
GOTO :eof

:AutoDetectVisualStudio
IF "%KAZZLE_ENABLE_VS2022%" EQU "0" GOTO :AfterVisualStudio2022
SET VS170COMNTOOLS=%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\Common7\Tools
REM This doesn't support nicknames and such.
IF NOT EXIST "%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\Common7\Tools\VsDevCmd.bat" GOTO :AfterVisualStudio2022
ECHO Located Visual Studio: VS2022 Enterprise (64 bit)
REM The below accepts -help to get help on the command-line args
REM -vcvars_ver=14.31.31103
CALL "%ProgramFiles%\Microsoft Visual Studio\2022\Enterprise\Common7\Tools\VsDevCmd.bat" -host_arch=amd64 -arch=amd64 -no_logo
IF "%KAZZLE_DEBUG%" == "1" @ECHO ON
SET KUSTO_DETECTED_VS_VERSION=17
SET KUSTO_DETECTED_MSBUILD=%VSINSTALLDIR%MSBuild\Current\Bin\MSBuild.exe
GOTO :eof

:AfterVisualStudio2022
IF "%KAZZLE_ENABLE_VS2019%" EQU "0" GOTO :AfterVisualStudio2019
REM This doesn't support nicknames and such.
IF NOT EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\Common7\Tools\VsDevCmd.bat" GOTO :AfterVisualStudio2019
ECHO:
ECHO Located Visual Studio: VS2019 Enterprise (64 bit)
REM The below accepts -help to get help on the command-line args
REM -vcvars_ver=14.31.31103
CALL "%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\Common7\Tools\VsDevCmd.bat" -host_arch=amd64 -arch=amd64 -no_logo
IF "%KAZZLE_DEBUG%" == "1" @ECHO ON
SET KUSTO_DETECTED_VS_VERSION=16
SET KUSTO_DETECTED_MSBUILD=%VSINSTALLDIR%MSBuild\Current\Bin\MSBuild.exe
GOTO :eof

:AfterVisualStudio2019
REM This doesn't support nicknames and such.
IF NOT EXIST "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\Common7\Tools\VsDevCmd.bat" GOTO :AfterVisualStudio2017
ECHO Located Visual Studio: VS2017 Enterprise (64 bit)
REM The below accepts -help to get help on the command-line args
CALL "%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Enterprise\Common7\Tools\VsDevCmd.bat" -host_arch=amd64 -arch=amd64 -no_logo
IF "%KAZZLE_DEBUG%" == "1" @ECHO ON
SET KUSTO_DETECTED_VS_VERSION=15
SET KUSTO_DETECTED_MSBUILD=%VSINSTALLDIR%MSBuild\15.0\Bin\MSBuild.exe
GOTO :eof