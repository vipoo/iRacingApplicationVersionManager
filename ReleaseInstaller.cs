using IWshRuntimeLibrary;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace iRacingApplicationVersionManger
{
    public class ReleaseInstaller
    {
        readonly string user;
        readonly string repo;
        readonly string appPath;
        readonly string downloadFilePath;
        readonly string shortCutPath;
        readonly string iconPath;
        readonly string mainExePath;

        public string MainExePath { get { return mainExePath; } }

        public ReleaseInstaller(string user, string repo)
        {
            this.user = user;
            this.repo = repo;

            var programFilesPath = Environment.GetEnvironmentVariable("PROGRAMFILES");
            appPath = Path.Combine(programFilesPath, "iRacing Application Version Manager", user, repo);
            downloadFilePath = appPath + "\\release.zip";
            mainExePath = appPath + "\\iRacingReplayOverlay.exe";
            shortCutPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\iRacing Replay Director.lnk";
            iconPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\images.ico";
        }

        public async Task install(string versionStamp, Action<int> progress) {
            CreateReleaseDirectory(appPath);
            var releaseDownloadUrl = await GetReleaseDownloadUrl(user, repo, versionStamp);
            await DownloadReleaseZip(progress, downloadFilePath, releaseDownloadUrl);
            await RemoveCurrentRelease(appPath);
            InstallNewRelease(appPath, downloadFilePath);
            createShortCut(shortCutPath, iconPath, mainExePath);
        }

        public async Task<VersionItem[]> AvailableVersions()
        {
            return await GitHubAccess.GetVersions(user, repo).ConfigureAwait(true);
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public void Run()
        {
            var processes = Process.GetProcessesByName("iRacingReplayOverlay");
            if (processes.Length > 0)
            {
                SetForegroundWindow(processes.First().MainWindowHandle);
            }
            else
                Process.Start(mainExePath);
        }

        public string CurrentInstalledVersion
        {
            get
            {
                if (System.IO.File.Exists(mainExePath))
                {
                    return AssemblyName.GetAssemblyName(mainExePath).Version.ToString();
                }

                return null;
            }
        }

        static void CreateReleaseDirectory(string appPath)
        {
            Directory.CreateDirectory(appPath);
        }

        static void InstallNewRelease(string appPath, string downloadFilePath)
        {
            ZipFile.ExtractToDirectory(downloadFilePath, appPath);
        }

        static async Task RemoveCurrentRelease(string appPath)
        {
            var di = new DirectoryInfo(appPath);
            foreach (FileInfo file in di.GetFiles().Where(f => !f.Name.EndsWith("release.zip")))
                file.Delete();

            await Task.Delay(1000);

            foreach (FileInfo file in di.GetFiles().Where(f => !f.Name.EndsWith("release.zip")))
                file.Delete();
        }

        static async Task DownloadReleaseZip(Action<int> progress, string downloadFilePath, string releaseDownloadUrl)
        {
            using (var webClient = new WebClient())
            {
                webClient.DownloadProgressChanged += (s, ee) => progress(ee.ProgressPercentage);
                await webClient.DownloadFileTaskAsync(new Uri(releaseDownloadUrl), downloadFilePath);
            }
        }

        static Task<string> GetReleaseDownloadUrl(string user, string repo, string versionStamp)
        {
            return GitHubAccess.GetReleaseDownloadUrl(user, repo, versionStamp);
        }

        static void createShortCut(string shortCutPath, string iconPath, string mainExePath)
        {
            var shell = new WshShell();
            var shortcut = (IWshShortcut)shell.CreateShortcut(shortCutPath);

            shortcut.Description = "iRacing Replay Director (x.x.x.x)";
            shortcut.IconLocation = iconPath;
            shortcut.TargetPath = mainExePath;
            shortcut.Save();
        }
    }
}
