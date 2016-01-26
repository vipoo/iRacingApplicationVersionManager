using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iRacingReplayDirectorInstaller
{
    public partial class Form1 : Form
    {
        Timer processRunningWatcher;
        bool isInstalling = false;
        private ReleaseInstaller installer;
        private VersionItem[] versions;

        public Form1()
        {
            InitializeComponent();
            installer = new ReleaseInstaller("vipoo", "iRacingReplayOverlay.net");
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }
        
        private  void Form1_Load(object sender, EventArgs e)
        {
            processRunningWatcher = new Timer();
            processRunningWatcher.Interval = 10;
            processRunningWatcher.Tick += (s, a) => OnCheckProcessCount();
            processRunningWatcher.Start();
            
            twoColumnDropDown();
        }

        private void OnCheckProcessCount()
        {
            isRunningWarningPanel.Visible = currentApplicationIsRunning();
            isRunningWarningPanel.Top = (this.Height / 2) - (isRunningWarningPanel.Height / 2);
            versionSelector_SelectedIndexChanged(null, null);
            versionSelector.Enabled = !this.isInstalling;

            try
            {
                if (System.IO.File.Exists(installer.MainExePath))
                {
                    currentVersion.Text = AssemblyName.GetAssemblyName(installer.MainExePath).Version.ToString();
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

                

                progressBar1.Value = 0;
                progressPanel.Visible = true;

                await installer.install(versionToInstall.VersionStamp, p => progressBar1.Value = p);

                progressBar1.Value = 100;
                await Task.Delay(200);
                progressPanel.Visible = false;
            }
            finally
            {
                isInstalling = false;
            }
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
                Process.Start(installer.MainExePath);
        }

        bool hasInited = false;

        private async void Form1_Activated(object sender, EventArgs e)
        {
            if (hasInited)
                return;

            hasInited = true;
            versionSelector.Items.Clear();

            versions = await installer.AvailableVersions();

            foreach (var v in versions)
                versionSelector.Items.Add(v);

            versionSelector.Enabled = true;
        }
    }
}
