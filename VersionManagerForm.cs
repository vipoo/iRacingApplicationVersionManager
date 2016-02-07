using GitHubReleases;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iRacingApplicationVersionManger
{
    public partial class VersionManagerForm : Form
    {
        Timer processRunningWatcher;
        bool isInstalling = false;
        ReleaseInstaller installer;
        VersionItem[] versions;

        public VersionManagerForm()
        {
            InitializeComponent();
            installer = new ReleaseInstaller("vipoo", "iRacingReplayOverlay.net");
        }
                
        void Form1_Load(object sender, EventArgs e)
        {
            processRunningWatcher = new Timer();
            processRunningWatcher.Interval = 10;
            processRunningWatcher.Tick += (s, a) => OnCheckProcessCount();
            processRunningWatcher.Start();
            
            twoColumnDropDown();
        }

        void OnCheckProcessCount()
        {
            isRunningWarningPanel.Visible = currentApplicationIsRunning();
            isRunningWarningPanel.Top = (this.Height / 2) - (isRunningWarningPanel.Height / 2);
            versionSelector_SelectedIndexChanged(null, null);
            versionSelector.Enabled = !this.isInstalling;

            try
            {
                var currentInstalledVersion = installer.CurrentInstalledVersion;
                if (currentInstalledVersion != null)
                {
                    currentVersion.Text = currentInstalledVersion;
                    openApplication.Enabled = true && !this.isInstalling;
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

        void twoColumnDropDown()
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
                string name = " " + drv.DateTimeStamp;

                // Get the bounds for the first column
                Rectangle r1 = args.Bounds;
                r1.Width = 120;

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
                r2.X = r1.Width;
                r2.Width /= 2;

                // Draw the text on the second column
                using (SolidBrush sb = new SolidBrush(args.ForeColor))
                {
                    args.Graphics.DrawString(name, args.Font, sb, r2);
                }
            };
        }

        async void button1_Click(object sender, EventArgs e)
        {
            installButton.Enabled = downloadButton.Enabled = false;
            isInstalling = true;
            versionSelector.Enabled = false;
            try
            {
                var versionToInstall = (VersionItem)versionSelector.SelectedItem;

                progressBar1.Value = 0;
                progressPanel.Visible = true;

                await installer.download(versionToInstall.VersionStamp, p => progressBar1.Value = p);

                progressBar1.Value = 100;
                await Task.Delay(200);
                progressPanel.Visible = false;
            }
            finally
            {
                isInstalling = false;
            }
        }

        void versionSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            installButton.Enabled = downloadButton.Enabled = versionSelector.SelectedIndex != -1 && !currentApplicationIsRunning() && !isInstalling;

            if (versionSelector.SelectedItem != null)
            {
                var versionToInstall = (VersionItem)versionSelector.SelectedItem;
                downloadButton.Text = installer.IsVersionDownloaded(versionToInstall.VersionStamp) ? "Re-download" : "download";
            }
            else
                downloadButton.Text = "download";
        }

        void button2_Click(object sender, EventArgs e)
        {
            foreach (var p in Process.GetProcessesByName("iRacingReplayOverlay"))
                p.CloseMainWindow();
        }
        
        void openApplication_Click(object sender, EventArgs e)
        {
            installer.Run();

            this.Close();
        }

        bool hasInited = false;

        async void Form1_Activated(object sender, EventArgs e)
        {
            if (hasInited)
                return;

            hasInited = true;
            await RefreshVersionList();

            versionSelector.Enabled = true;
        }

        async void installButton_Click(object sender, EventArgs e)
        {
            installButton.Enabled = downloadButton.Enabled = false;
            isInstalling = true;
            versionSelector.Enabled = false;
            try
            {
                var versionToInstall = (VersionItem)versionSelector.SelectedItem;

                progressBar1.Value = 0;
                progressPanel.Visible = true;

                await installer.Install(versionToInstall.VersionStamp, p => progressBar1.Value = p);

                progressBar1.Value = 100;
                await Task.Delay(200);
                progressPanel.Visible = false;
            }
            finally
            {
                isInstalling = false;
            }
        }

        private void prereleaseCheck_CheckedChanged(object sender, EventArgs e)
        {
            RefreshVersionList();
        }

        private async Task RefreshVersionList()
        {
            versionSelector.Items.Clear();

            versions = await installer.AvailableVersions();

            foreach (var v in versions)
                if ((v.Prerelease && prereleaseCheck.Checked) || !v.Prerelease)
                    versionSelector.Items.Add(v);
        }
    }
}
