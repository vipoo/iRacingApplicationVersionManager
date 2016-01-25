using IWshRuntimeLibrary;
using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iRacingReplayDirectorInstaller
{
    public partial class Form1 : Form
    {
        class VersionItem
        {
            public string VersionStamp;
            public string DateTimeStamp;
        }

        IReadOnlyList<Release> releases;
        WebClient WebClient;
        Timer processRunningWatcher;
        private string programFilesPath;
        private string appPath;
        private string downloadFilePath;
        private string mainExePath;
        private string shortCutPath;
        private string iconPath;
        bool isInstalling = false;

        public Form1()
        {
            InitializeComponent();

            programFilesPath = Environment.GetEnvironmentVariable("PROGRAMFILES");
            appPath = programFilesPath + "\\iRacingReplayDirector";
            downloadFilePath = appPath + "\\release.zip";
            mainExePath = appPath + "\\iRacingReplayOverlay.exe";
            shortCutPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + "\\iRacing Replay Director.lnk";
            iconPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\images.ico";

            var f = new LoadingReleases();
            f.ShowDialog();
            this.releases = f.releases;
            f.Close();

            WebClient = new WebClient();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (WebClient != null)
                WebClient.Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            processRunningWatcher = new System.Windows.Forms.Timer();
            processRunningWatcher.Interval = 10;
            processRunningWatcher.Tick += (s, a) => OnCheckProcessCount();
            processRunningWatcher.Start();

            twoColumnDropDown();
            
            versionSelector.Items.Clear();

            foreach(var r in releases)
            {
                var i = new VersionItem { VersionStamp = r.TagName, DateTimeStamp = r.CreatedAt.ToString() };
                versionSelector.Items.Add(i);
            }

            versionSelector.Enabled = true;
        }

        private void OnCheckProcessCount()
        {
            isRunningWarningPanel.Visible = currentApplicationIsRunning();
            isRunningWarningPanel.Top = (this.Height / 2) - (isRunningWarningPanel.Height / 2);
            versionSelector_SelectedIndexChanged(null, null);
            versionSelector.Enabled = !this.isInstalling;
            openApplication.Enabled = !this.isInstalling;

            try
            {
                if (System.IO.File.Exists(mainExePath))
                {
                    currentVersion.Text = AssemblyName.GetAssemblyName(mainExePath).Version.ToString();
                    openApplication.Enabled = true;
                }
                else
                    openApplication.Enabled = false;
            }
            catch
            { }
        }

        bool currentApplicationIsRunning()
        {
            return Process.GetProcessesByName("iRacingReplayOverlay").Length > 0;
        }

        private void twoColumnDropDown()
        {
            this.versionSelector.DrawMode = DrawMode.OwnerDrawFixed;
            // Handle the DrawItem event to draw the items.
            this.versionSelector.DrawItem += delegate (object cmb, DrawItemEventArgs args)
            {
                // Draw the default background
                args.DrawBackground();

                if (args.Index == -1)
                    return;

                // The ComboBox is bound to a DataTable,
                // so the items are DataRowView objects.
                var drv = (VersionItem)this.versionSelector.Items[args.Index];

          
                // Retrieve the value of each column.
                string id = drv.VersionStamp;
                string name = "  " + drv.DateTimeStamp;

                // Get the bounds for the first column
                Rectangle r1 = args.Bounds;
                r1.Width /= 2;

                // Draw the text on the first column
                using (SolidBrush sb = new SolidBrush(args.ForeColor))
                {
                    args.Graphics.DrawString(id, args.Font, sb, r1);
                }

                // Draw a line to isolate the columns 
                using (Pen p = new Pen(Color.Black))
                {
                    args.Graphics.DrawLine(p, r1.Right, 0, r1.Right, r1.Bottom);
                }

                // Get the bounds for the second column
                Rectangle r2 = args.Bounds;
                r2.X = args.Bounds.Width / 2;
                r2.Width /= 2;

                // Draw the text on the second column
                using (SolidBrush sb = new SolidBrush(args.ForeColor))
                {
                    args.Graphics.DrawString(name, args.Font, sb, r2);
                }
            };
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            isInstalling = true;
            versionSelector.Enabled = false;
            try
            {
                var versionToInstall = (VersionItem)versionSelector.SelectedItem;

                Directory.CreateDirectory(appPath);

                progressBar1.Value = 0;
                progressPanel.Visible = true;

                var latestRelease = releases.Where(r => r.TagName == versionToInstall.VersionStamp).First();

                var assets = await Program.Client.Release.GetAllAssets("vipoo", "iRacingReplayOverlay.net", latestRelease.Id);

                var asset = assets.First();

                WebClient.DownloadProgressChanged += (s, ee) =>
                {
                    progressBar1.Value = ee.ProgressPercentage;
                    Trace.WriteLine(Math.Min(ee.ProgressPercentage, 95));
                };

                await WebClient.DownloadFileTaskAsync(new Uri(asset.BrowserDownloadUrl), downloadFilePath);

                var di = new DirectoryInfo(appPath);

                foreach (FileInfo file in di.GetFiles().Where(f => !f.Name.EndsWith("release.zip")))
                    file.Delete();

                await Task.Delay(1000);

                foreach (FileInfo file in di.GetFiles().Where(f => !f.Name.EndsWith("release.zip")))
                    file.Delete();

                ZipFile.ExtractToDirectory(downloadFilePath, appPath);

                createShortCut();
                progressBar1.Value = 100;
                await Task.Delay(200);
                progressPanel.Visible = false;
            }
            finally
            {
                isInstalling = false;
            }
        }

        private void createShortCut()
        {
            var shell = new WshShell();
            var shortcut = (IWshShortcut)shell.CreateShortcut(shortCutPath);

            shortcut.Description = "iRacing Replay Director (x.x.x.x)";  
            shortcut.IconLocation = iconPath;
            shortcut.TargetPath = mainExePath;
            shortcut.Save();
        }

        private void versionSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = versionSelector.SelectedIndex != -1 && !currentApplicationIsRunning() && !isInstalling;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var p in Process.GetProcessesByName("iRacingReplayOverlay"))
                p.CloseMainWindow();
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        private void openApplication_Click(object sender, EventArgs e)
        {
            var processes = Process.GetProcessesByName("iRacingReplayOverlay");
            if( processes.Length > 0)
            {
                SetForegroundWindow(processes.First().MainWindowHandle);
            }
            else
                Process.Start(mainExePath);
        }
    }
}
