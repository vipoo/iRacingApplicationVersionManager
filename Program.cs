using iRacingApplicationVersionManger.Properties;
using IWshRuntimeLibrary;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
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

            CreateUpdateShortCut();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            PortableSettingsProvider.MakePortable(Settings.Default);
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

        static void CreateUpdateShortCut()
        {
            var name = "iRacing Application Updates";

            var shortCutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs", "Dean Netherton"); 
            Directory.CreateDirectory(shortCutPath);
            var shortCutFilePath = Path.Combine(shortCutPath, name + ".lnk");

            var shell = new WshShell();
            var shortcut = (IWshShortcut)shell.CreateShortcut(shortCutFilePath);

            shortcut.Description = name;
            shortcut.Arguments = "-update";
            shortcut.TargetPath = Assembly.GetExecutingAssembly().Location;
            shortcut.Save();
        }
    }
}
