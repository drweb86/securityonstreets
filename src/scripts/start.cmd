@echo off

set IpCamEmuDir=IpCamEmu 1.3
set MessageRouterDir=Message Router
set ImageProcessingServerDir=Image Processing Server
set ClientDir=client

echo Starting %IpCamEmuDir%
rem Issue with finding self settings
cd %IpCamEmuDir%
start "" "HDE.IpCamEmuWpf.exe" /NOWAIT
cd ..

echo Starting %MessageRouterDir%
start "" "%MessageRouterDir%\MessageRouter.Server.WpfServer.exe" /NOWAIT

echo Starting %ImageProcessingServerDir%
start "" "%ImageProcessingServerDir%\ServerC.exe" /NOWAIT

echo Starting %ClientDir%
start "" "%ClientDir%\Client.exe" /NOWAIT
