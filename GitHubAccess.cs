using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iRacingReplayDirectorInstaller
{
    public struct VersionItem
    {
        public int Id;
        public string VersionStamp;
        public string DateTimeStamp;
    }

    public struct RepoKey
    {
        public string User;
        public string Repo;
    }

    public interface GitHubCachedResult<T1, T2>
    {
        DateTime RetreivedAt { get; set; }
        T1 Key { get; set; }
        T2 Data { get; set; }
    }

    public class GitHubCachedReleases : GitHubCachedResult<RepoKey, VersionItem[]>
    {
        public VersionItem[] Data { get; set; }
        public RepoKey Key { get; set; }
        public DateTime RetreivedAt { get; set; }
    }

    public static class GitHubAccess
    {
        static readonly GitHubClient Client;

        static GitHubAccess()
        {
            Client = new GitHubClient(new ProductHeaderValue("iracing-replay-director-installer"));
        }

        public async static Task<string> GetReleaseDownloadUrl(string user, string repo, string versionStamp)
        {
            var release = (await GetVersions(user, repo)).Where(r => r.VersionStamp == versionStamp).First();

            var assets = await Program.Client.Release.GetAllAssets(user, repo, release.Id);
            var asset = assets.First();
            return asset.BrowserDownloadUrl;
        }

        public static async Task<VersionItem[]> GetVersions(string user, string repo)
        {
            var key = new RepoKey { Repo = repo, User = user };
            var allReleases = Properties.Settings.Default.Releases == null ? new List<GitHubCachedReleases>() : Properties.Settings.Default.Releases.ToList();
            var cacheHit = allReleases.FirstOrDefault(r => r.Key.Equals(key));

            if (cacheHit != null)
            {
                await Task.Delay(3000).ConfigureAwait(true);
                return cacheHit.Data;
            }

            var releases = await Program.Client.Release.GetAll("vipoo", "iRacingReplayOverlay.net");

            var versionReleases = releases
                    .Select(r => new VersionItem { DateTimeStamp = r.CreatedAt.ToString(), VersionStamp = r.TagName, Id = r.Id })
                    .ToArray();

            allReleases.Add(new GitHubCachedReleases { Key = key, Data = versionReleases, RetreivedAt = DateTime.Now });

            Properties.Settings.Default.Releases = allReleases.ToArray();
            Properties.Settings.Default.Save();

            return versionReleases;
        }
    }
}
