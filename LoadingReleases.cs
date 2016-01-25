using Octokit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace iRacingReplayDirectorInstaller
{
    public partial class LoadingReleases : Form
    {
        public IReadOnlyList<Release> releases;

        public LoadingReleases()
        {
            InitializeComponent();
        }

        private async void LoadingReleases_Load(object sender, EventArgs e)
        {
            try
            {
                releases = await Program.Client.Release.GetAll("vipoo", "iRacingReplayOverlay.net");
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
