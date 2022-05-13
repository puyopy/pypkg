// pypupy - 2022

using pypkg;
using pypkg.API;
//using LibGit2Sharp;

namespace pypkg.Install
{
    public class Manifests
    {
       // private static readonly string RegistryUrl = "https://raw.githubusercontent.com/UpliftGames/wally-index/main/{0}/{1}";
        // <summary>
        // Returns the manifest from the registry.
        // </summary>
        public static async Task<string> PullManifest(string author = "7kayoh", string name = "fusionrouter")
        {

            // ugly.. should probably replace with format strings.
            Logger.Log("Attempting to pull manifest from registry for " + author + "/" + name);
            string UrlPath = string.Format(APIUrl.PackageMetadata, author, name);
            HttpResponseMessage Result = await Manager.HttpClient.GetAsync(UrlPath);
            if (!Result.IsSuccessStatusCode) {
                Logger.Log("Failed to pull manifest from registry for " + author + "/" + name + ":" + Result.StatusCode);
                return null;
            }

            // my head is getting dizzy from so much asyncrhous garbage..
            string Manifest = await Result.Content.ReadAsStringAsync();

            Logger.Log(author + "/" + name + ":" + Manifest);
            
            
            return Manifest;
        }

        public static PackageVersion ResolveToVersion(ManifestResult Results, string Version)
        {
            if (Version == "latest")
            {
                return Results.versions[0];
            }

            foreach (PackageVersion VersionResult in Results.versions)
            {
                if (VersionResult.package.version == Version)
                {
                    return VersionResult;
                }
            }
            return null;
            
        }
    }
}
