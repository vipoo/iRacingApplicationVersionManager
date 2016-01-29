using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace iRacingApplicationVersionManger
{
    public partial class LoadingReleases : Form
    {
        internal VersionItem[] versions;
        private readonly ReleaseInstaller installer;

        public LoadingReleases(ReleaseInstaller installer)
        {
            InitializeComponent();
            this.installer = installer;
        }

        private async void LoadingReleases_Load(object sender, EventArgs e)
        {
            try
            {
                versions = await installer.AvailableVersions();
                this.Hide();
            }
            catch(RateLimitExceededException)
            {
                MessageBox.Show("Rate Limit Exceeded.  Please try again later");
                System.Windows.Forms.Application.Exit();
            }
        }
    }
}
