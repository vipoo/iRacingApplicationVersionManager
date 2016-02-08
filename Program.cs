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

            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            CreateUpdateShortCut();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IracingApplicationVersionManagerProvider.MakePortable(Settings.Default);
            Settings.Default.MainExecPath = Assembly.GetExecutingAssembly().Location;
            Settings.Default.Save();

            var installer = new ReleaseInstaller("vipoo", "iRacingReplayOverlay.net");

            if (args.Contains("-update-plugin"))
            {
                var versionStamp = args.First(a => a.StartsWith("-version=")).Substring("-version=".Length);
                var pluginInstaller = installer.ForPlugin("iRacingDirector.Plugin.StandardOverlays");

                Application.Run(new PluginInstallationForm(pluginInstaller, versionStamp));
                return;
            }

            if (args.Contains("-update"))
            {
                Application.Run(new VersionManagerForm());
                return;
            }

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
                var process = Process.Start(processInfo);
                process.WaitForExit();
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

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                Trace.WriteLine(ex.Message, "INFO");
                Trace.WriteLine(ex.StackTrace, "DEBUG");
            }
            else
            {
                Trace.WriteLine(string.Format("An unknown error occured. {0}, {1}", e.ExceptionObject.GetType().Name, e.ExceptionObject.ToString()));
            }
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(string.Format("An error occured.  Details have been logged.\n\n{0}", e.Exception.Message), "Error");
            Trace.WriteLine(e.Exception.Message, "INFO");
            Trace.WriteLine(e.Exception.StackTrace, "DEBUG");
        }
    }
}
