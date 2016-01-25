using System;
using System.Windows.Forms;

namespace iRacingReplayDirectorInstaller
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
            Client = new Octokit.GitHubClient(new Octokit.ProductHeaderValue("iracing-replay-director-installer2"));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
