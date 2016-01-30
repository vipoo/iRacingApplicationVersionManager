
@echo off
echo 'Building package' %APPVEYOR_BUILD_VERSION%

msbuild iRacingApplicationVersionManager.csproj -p:SolutionDir=%cd%\                                                            ^
                                        -p:Configuration=Release                                                                ^
                                        -t:rebuild                                                                              ^
                                        -t:publish                                                                              ^
                                        -p:"InstallUrl=https://s3-ap-southeast-2.amazonaws.com/iracing-application-version-manager/release/"  ^
                                        -p:ApplicationVersion=%APPVEYOR_BUILD_VERSION%                                          ^
                                        -v:minimal                                          
 

cd bin\Release\app.publish
for /D %%F in ("Application Files\*") do (
    appveyor PushArtifact "%%F\iRacingApplicationVersionManger.exe.config.deploy" -FileName "release\%%F\iRacingApplicationVersionManger.exe.config.deploy" -DeploymentName deploy 
    appveyor PushArtifact "%%F\iRacingApplicationVersionManger.exe.deploy"        -FileName "release\%%F\iRacingApplicationVersionManger.exe.deploy"        -DeploymentName deploy 
    appveyor PushArtifact "%%F\iRacingApplicationVersionManger.pdb.deploy"        -FileName "release\%%F\iRacingApplicationVersionManger.pdb.deploy"        -DeploymentName deploy
    appveyor PushArtifact "%%F\iRacingApplicationVersionManger.exe.manifest"      -FileName "release\%%F\iRacingApplicationVersionManger.exe.manifest"      -DeploymentName deploy 
    appveyor PushArtifact "%%F\Octokit.dll.deploy"                                -FileName "release\%%F\Octokit.dll.deploy"                                -DeploymentName deploy 
    appveyor PushArtifact "%%F\Microsoft.Win32.TaskScheduler.dll.deploy"          -FileName "release\%%F\Microsoft.Win32.TaskScheduler.dll.deploy"          -DeploymentName deploy 
    appveyor PushArtifact "%%F\Octokit.pdb.deploy"                                -FileName "release\%%F\Octokit.pdb.deploy"                                -DeploymentName deploy 
)

appveyor PushArtifact setup.exe                                   -FileName "release\setup.exe"                                    -DeploymentName deploy
appveyor PushArtifact iRacingApplicationVersionManger.application -FileName "release\iRacingApplicationVersionManger.application"  -DeploymentName deploy
cd ..\..\..

