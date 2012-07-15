@echo off

set msBuild=%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe

set configuration=Release
set target=Rebuild

if exist ..\output rd ..\output /S /Q
if exist ..\deployment rd ..\deployment /S /Q
md ..\deployment

set sofwareComponent=Message Router
echo Building the %sofwareComponent%
"%msbuild%" "..\src\%sofwareComponent%.sln" ^
	/Target:"%target%" ^
	/p:Configuration="%configuration%"
if %ERRORLEVEL% NEQ 0 GOTO FAILED_BUILD;


set sofwareComponent=Image Processing Server and Client
echo Building the %sofwareComponent%
"%msbuild%" "..\src\%sofwareComponent%.sln" ^
	/Target:"%target%" ^
	/p:Configuration="%configuration%"

if %ERRORLEVEL% NEQ 0 GOTO FAILED_BUILD;

set sofwareComponent=Local Setup
echo Building the %sofwareComponent%
"%msbuild%" "..\src\%sofwareComponent%.sln" ^
	/Target:"%target%" ^
	/p:Configuration="%configuration%"

if %ERRORLEVEL% NEQ 0 GOTO FAILED_BUILD;

echo Preparing Environment

set operation=Copy IP-Camera Emulator IpCamEmu
echo %operation%.
XCOPY "..\third-party\IpCamEmu 1.3" "..\output\IpCamEmu 1.3" /s /i /y
if %ERRORLEVEL% NEQ 0 GOTO FAILED_OPERATION;

set operation=Copy Documentation
echo %operation%.
XCOPY "..\docs" "..\output\docs" /s /i /y
if %ERRORLEVEL% NEQ 0 GOTO FAILED_OPERATION;

set operation=Copy 'IpCamClientServer - Local Installation Guide.docx' to root
echo %operation%.
XCOPY "..\docs\IpCamClientServer - Local Installation Guide.docx" "..\output" /s /i /y
if %ERRORLEVEL% NEQ 0 GOTO FAILED_OPERATION;

set operation=Preparing Scripts (start.cmd)
echo %operation%.
XCOPY "..\src\scripts\start.cmd" "..\output" /s /i /y
if %ERRORLEVEL% NEQ 0 GOTO FAILED_OPERATION;

set operation=Preparing Scripts (setup.cmd)
echo %operation%.
XCOPY "..\src\scripts\setup.cmd" "..\output" /s /i /y
if %ERRORLEVEL% NEQ 0 GOTO FAILED_OPERATION;

set operation=Prepare dependent software
echo %operation%.
rem XCOPY "..\third-party\Environment" "..\output\Environment" /s /i /y
if %ERRORLEVEL% NEQ 0 GOTO FAILED_OPERATION;

echo Deployment

set operation=Prepare binaries
echo %operation%.
rem "..\third-party\ZipSolution-5.9\ZipSolution.Console.exe" "SolutionFile=deploy-binaries.xml" "ExtractVersionFromAssemblyFile=..\Output\Image Processing Server\ServerC.exe"
if %ERRORLEVEL% NEQ 0 GOTO FAILED_OPERATION;

goto END;

:FAILED_BUILD
echo ERROR: Failed to build %sofwareComponent%.
pause
goto END;

:FAILED_OPERATION
echo ERROR: Failed to %operation%.
pause
goto END;

:END
echo on