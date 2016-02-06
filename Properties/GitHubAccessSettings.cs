using GitHubReleases;
using iRacingApplicationVersionManger.Properties;
using System.Configuration;

internal class IracingApplicationVersionManagerProvider : PortableSettingsProvider
{
    public static void MakePortable(ApplicationSettingsBase settings)
    {
        MakePortable<IracingApplicationVersionManagerProvider>(settings);
    }

    public override string GetAppSettingsFilename()
    {
        return "iracing-application-version-manager.settings";
    }
}

internal static class GitHubAccessSettings
{
    public static GitHubCachedReleases[] GitHubCachedReleases
    {
        get
        {
            return Settings.Default.GitHubCachedReleases;
        }
        set
        {
            Settings.Default.GitHubCachedReleases = value;
        }
    }

    public static void Save()
    {
        Settings.Default.Save();
    }
}
