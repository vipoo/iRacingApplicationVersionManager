
@echo off
echo 'Building package' %APPVEYOR_BUILD_VERSION%

msbuild iRacingApplicationVersionManager.csproj -p:SolutionDir=%cd%\                                                            ^
                                        -p:Configuration=Release                                                                ^
                                        -t:rebuild                                                                              ^
                                        -t:publish                                                                              ^
                                        -p:"InstallUrl=https://iracing-application-version-manager.s3-website-ap-southeast-2.amazonaws.com/release/"  ^
                                        -p:ApplicationVersion=%APPVEYOR_BUILD_VERSION%                                          ^
                                        -v:minimal                                          
 

cd bin\Release\app.publish
for /D %%F in ("Application Files\*") do (
    appveyor PushArtifact "%%F\iRacingApplicationVersionManger.exe.config.deploy" -FileName "release\%%F\iRacingApplicationVersionManger.exe.config.deploy" -DeploymentName deploy 
    appveyor PushArtifact "%%F\iRacingApplicationVersionManger.exe.deploy"        -FileName "release\%%F\iRacingApplicationVersionManger.exe.deploy"        -DeploymentName deploy 
    appveyor PushArtifact "%%F\iRacingApplicationVersionManger.pdb.deploy"        -FileName "release\%%F\iRacingApplicationVersionManger.pdb.deploy"        -DeploymentName deploy
    appveyor PushArtifact "%%F\iRacingApplicationVersionManger.exe.manifest"      -FileName "release\%%F\iRacingApplicationVersionManger.exe.manifest"      -DeploymentName deploy 
    appveyor PushArtifact "%%F\Octokit.dll.deploy"                                -FileName "release\%%F\Octokit.dll.deploy"                                -DeploymentName deploy 
    appveyor PushArtifact "%%F\Octokit.pdb.deploy"                                -FileName "release\%%F\Octokit.pdb.deploy"                                -DeploymentName deploy 
)

appveyor PushArtifact setup.exe                             -FileName "release\setup.exe"                                    -DeploymentName deploy
appveyor PushArtifact iRacingReplayOverlay.test.application -FileName "release\iRacingApplicationVersionManger.application"  -DeploymentName deploy
cd ..\..\..

