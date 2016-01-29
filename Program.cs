using iRacingApplicationVersionManger.Properties;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace iRacingApplicationVersionManger
{
    static class Program
    {
        public static Octokit.GitHubClient Client { get; private set; }

        [STAThread]
        static void Main(string[] arg)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MakePortable(Settings.Default);
            Settings.Default.MainExecPath = Assembly.GetExecutingAssembly().Location;
            Settings.Default.Save();

            Client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("iracing-replay-director-installer"));
            var installer = new ReleaseInstaller("vipoo", "iRacingReplayOverlay.net");

            if (arg.Contains("-update"))
                Application.Run(new Form1());
            else
                InitialInstallation(installer);
        }

        private static void InitialInstallation(ReleaseInstaller installer)
        {
            var currentInstalledVersion = installer.CurrentInstalledVersion;
            if (currentInstalledVersion != null)
                installer.Run();
            else
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
