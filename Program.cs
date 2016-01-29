using iRacingApplicationVersionManger.Properties;
using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;

namespace iRacingApplicationVersionManger
{
    static class Program
    {
        public static Octokit.GitHubClient Client { get; private set; }

        [STAThread]
        static void Main(string[] arg)
        {
            MakePortable(Settings.Default);

            Client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("iracing-replay-director-installer"));
            var installer = new ReleaseInstaller("vipoo", "iRacingReplayOverlay.net");

            var currentInstalledVersion = installer.CurrentInstalledVersion;
            if (currentInstalledVersion == null)
            {
                installer.Run();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new InitialInstallationForm(installer));
        }

        static void MakePortable(Settings settings)
        {
            var pp = settings.Providers.OfType<PortableSettingsProvider>().First();

            foreach (SettingsProperty p in settings.Properties)
                p.Provider = pp;
            settings.Reload();
        }
    }
}
