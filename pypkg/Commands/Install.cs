// pypupy - 2022

using pypkg;
using pypkg.Install;
using Newtonsoft.Json;

namespace pypkg.Commands
{

    public class InstallCommand : Command
    {
        public string name = "install";

        public async Task<CommandStatus> Execute(string[] args)
        {
            if (args[0] == null || args[0] == null) {
                Console.WriteLine("Usage: pypkg install <package>");
                return CommandStatus.Success;
            };

            string[] SplitArgs = args[0].Split("/");

            string Scope = SplitArgs[0];
            string PackageName = SplitArgs[1];

            string Manifest = await Manifests.PullManifest(Scope,PackageName);
            if (Manifest == null)
            {
                return CommandStatus.Failure;   
            }
            string[] split = Manifest.Split('\n');

            // perform json decode magic
            ManifestResult ManifestVersions = JsonConvert.DeserializeObject<ManifestResult>(split[0]);

            // TODO: add a filter for versions
            Logger.Log(ManifestVersions.versions.Length + " versions found");

            // get the latest version
            PackageVersion LatestVersion = Manifests.ResolveToVersion(ManifestVersions, "latest");
            Package latest = LatestVersion.package;
            Logger.Log("Installing latest");

            CommandStatus Result = await Installator.InstallPackage(latest,LatestVersion,PackageName);
            
            if (Result == CommandStatus.Success)
            {
                Logger.Log("Successfully installed " + latest.name);
            } else
            {
                Logger.Log("Failed to install " + latest.name);
                Logger.DumpLogs();
            }


            return CommandStatus.Success;
        }
    }
}
