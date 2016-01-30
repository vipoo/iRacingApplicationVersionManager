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
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        readonly string user;
        readonly string repo;
        readonly string appPath;
        readonly string shortCutPath;
        readonly string mainExePath;
        readonly string downloadPath;
        readonly string tmpPath;

        public ReleaseInstaller(string user, string repo)
        {
            this.user = user;
            this.repo = repo;

            var programFilesPath = Environment.GetEnvironmentVariable("PROGRAMFILES");
            appPath = Path.Combine(programFilesPath, "iRacing Application Version Manager", user, repo, "current");
            tmpPath = Path.Combine(programFilesPath, "iRacing Application Version Manager", user, repo, "tmp");
            downloadPath = Path.Combine(programFilesPath, "iRacing Application Version Manager", user, repo);
            mainExePath = appPath + "\\iRacingReplayOverlay.exe";
            shortCutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "iRacing Applications"); 
        }

        public async Task install(string versionStamp, Action<int> progress) {
            var downloadFilePath = GetDownloadReleaseZipFilePath(versionStamp);

            if (!System.IO.File.Exists(downloadFilePath))
                await download(versionStamp, progress);

            RemoveCurrentRelease();
            InstallNewRelease(downloadFilePath);
            createShortCut(versionStamp);
        }

        public async Task download(string versionStamp, Action<int> progress)
        {
            var downloadFilePath = GetDownloadReleaseZipFilePath(versionStamp);

            CreateReleaseDirectory();
            var releaseDownloadUrl = await GetReleaseDownloadUrl(versionStamp);
            await DownloadReleaseZip(progress, downloadFilePath, releaseDownloadUrl);
        }

        public async Task<VersionItem[]> AvailableVersions()
        {
            return await GitHubAccess.GetVersions(user, repo).ConfigureAwait(true);
        }

        public void Run()
        {
            var processes = Process.GetProcessesByName("iRacingReplayOverlay");
            if (processes.Length > 0)
                SetForegroundWindow(processes.First().MainWindowHandle);
            else
                DeElevatedProcess.Start(mainExePath);
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

        public bool IsVersionDownloaded(string versionStamp)
        {
            return System.IO.File.Exists(GetDownloadReleaseZipFilePath(versionStamp));
        }

        void CreateReleaseDirectory()
        {
            Directory.CreateDirectory(appPath);
        }

        void InstallNewRelease(string downloadFilePath)
        {
            var di = new DirectoryInfo(tmpPath);
            if( di.Exists)
                di.Delete(true);
            ZipFile.ExtractToDirectory(downloadFilePath, tmpPath);
            di.MoveTo(appPath);
        }

        void RemoveCurrentRelease()
        {
            var di = new DirectoryInfo(appPath);
            if( di.Exists)
                di.Delete(true);

            di = new DirectoryInfo(shortCutPath);
            if (di.Exists)
                di.Delete(true);
        }

        static async Task DownloadReleaseZip(Action<int> progress, string downloadFilePath, string releaseDownloadUrl)
        {
            using (var webClient = new WebClient())
            {
                webClient.DownloadProgressChanged += (s, ee) => progress(ee.ProgressPercentage);
                await webClient.DownloadFileTaskAsync(new Uri(releaseDownloadUrl), downloadFilePath);
            }
        }

        Task<string> GetReleaseDownloadUrl( string versionStamp)
        {
            return GitHubAccess.GetReleaseDownloadUrl(user, repo, versionStamp);
        }

        void createShortCut(string versionStamp)
        {
            var name = "iRacing Replay Director (" + versionStamp + ")";

            Directory.CreateDirectory(shortCutPath);
            var shortCutFilePath = Path.Combine(shortCutPath, name + ".lnk");

            var shell = new WshShell();
            var shortcut = (IWshShortcut)shell.CreateShortcut(shortCutFilePath);

            shortcut.Description = name;
            shortcut.TargetPath = mainExePath;
            shortcut.Save();
        }

        private string GetDownloadReleaseZipFilePath(string versionStamp)
        {
            return Path.Combine(downloadPath, "version-" + versionStamp + ".release.zip");
        }
    }
}
