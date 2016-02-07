using System;
using System.Windows.Forms;

namespace iRacingApplicationVersionManger
{
    internal partial class PluginInstallationForm : Form
    {
        readonly PluginInstaller pluginInstaller;
        readonly string versionStamp;

        public PluginInstallationForm(PluginInstaller pluginInstaller, string versionStamp)
        {
            InitializeComponent();
            this.pluginInstaller = pluginInstaller;
            this.versionStamp = versionStamp;
        }

        private async void LoadingReleases_Load(object sender, EventArgs e)
        {
            this.versionInstalling.Text = this.versionStamp;
            await pluginInstaller.Install(versionStamp, p => progressBar1.Value = p);

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
