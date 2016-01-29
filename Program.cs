using System;
using System.Windows.Forms;

namespace iRacingApplicationVersionManger
{
    static class Program
    {
        public static Octokit.GitHubClient Client { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("iracing-replay-director-installer"));
            var installer = new ReleaseInstaller("vipoo", "iRacingReplayOverlay.net");

            var currentInstalledVersion = installer.CurrentInstalledVersion;
            if (currentInstalledVersion != null)
            {
                installer.Run();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new InitialInstallationForm(installer));
        }
    }
}
