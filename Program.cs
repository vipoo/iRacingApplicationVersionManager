using iRacingApplicationVersionManger.Properties;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;

namespace iRacingApplicationVersionManger
{
    static class Program
    {
        static bool IsRunAsAdministrator()
        {
            var wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        [STAThread]
        static void Main(string[] args)
        {
            if (!IsRunAsAdministrator())
            {
                RunAsAdministrator(args);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MakePortable(Settings.Default);
            Settings.Default.MainExecPath = Assembly.GetExecutingAssembly().Location;
            Settings.Default.Save();

            var installer = new ReleaseInstaller("vipoo", "iRacingReplayOverlay.net");

            if (args.Contains("-update"))
                Application.Run(new VersionManagerForm());
            else
                InitialInstallation(installer);
        }

        private static void RunAsAdministrator(string[] args)
        {
            var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);

            processInfo.UseShellExecute = true;
            processInfo.Verb = "runas";
            processInfo.Arguments = String.Join(" ", args);

            try
            {
                Process.Start(processInfo);
            }
            catch
            {
            }

            return;
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
