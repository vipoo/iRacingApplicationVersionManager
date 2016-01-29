using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iRacingApplicationVersionManger
{
    public partial class InitialInstallationForm : Form
    {
        readonly ReleaseInstaller installer;

        public InitialInstallationForm(ReleaseInstaller installer)
        {
            InitializeComponent();
            this.installer = installer;
        }

        private async void LoadingReleases_Load(object sender, EventArgs e)
        {
            try
            {
                var versions = await installer.AvailableVersions();
                var firstVersion = versions.First();
                versionInstalling.Text = firstVersion.VersionStamp;

                await installer.install(firstVersion.VersionStamp, p => progressBar1.Value = p);

                installer.Run();
                this.Close();
            }
            catch(RateLimitExceededException)
            {
                MessageBox.Show("Rate Limit Exceeded.  Please try again later");
                System.Windows.Forms.Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
