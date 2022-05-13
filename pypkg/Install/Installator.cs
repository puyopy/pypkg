﻿// pypupy - 2022

using Newtonsoft.Json;
using pypkg.API;
using pypkg.Commands;
using System.IO.Compression;

namespace pypkg.Install
{
    public class Installator
    {
        private static readonly string ExecutionDirectory = Directory.GetCurrentDirectory();
        private static readonly string PackagesDirectory = ExecutionDirectory + "/Packages/";
        public static readonly HttpClient HttpClient = Manager.HttpClient;
        private static readonly string TempDirectory = Path.GetTempPath() + "/pypkg/";

        private static readonly string DisclaimerHeader = "-- This package was imported with "
            + Manager.pypkgVersion + " emulating wally v" +
            Manager.WallyVersion +
            "\n-- It is not recommended to modify this file.";

        private async static Task<byte[]> DownloadZIP(string Scope, string PackageName, string Version)
        {
            string DownloadURL = string.Format(APIUrl.PackageZIP, Scope, PackageName, Version);

            try
            {
                var Response = HttpClient.GetAsync(DownloadURL);
                return await Response.Result.Content.ReadAsByteArrayAsync();
            }
            catch (Exception err)
            {
                Logger.Log("Could not download package zip:" + err, Logger.InfoType.Exception);
                return null;
            }
        }
        private static void JanitorCleanPackage(string Scope)
        {
            try
            {
                Directory.Delete(PackagesDirectory + Scope);
            }
            catch (Exception err)
            {
                Logger.Log("Could not clean package:" + err, Logger.InfoType.Exception);
            }
        }

        public static void AddHeader(string PackageDirectory)
        {
            string InitFilePath = PackageDirectory + "/src/init.lua";

            if (File.Exists(InitFilePath))
            {
                string FileContent = File.ReadAllText(InitFilePath);

                if (!FileContent.Contains(DisclaimerHeader))
                {
                    File.WriteAllText(InitFilePath, DisclaimerHeader + "\n\n" + FileContent);
                }
            }
        }

        public async static Task<CommandStatus> InstallPackage(Package Package, PackageVersion VersionManifest,string FolderName)
        {
            string Name = Package.name;
            string[] NameSplit = Name.Split("/");

            // might cause a ton of bugs lol
            string Scope = NameSplit[0];
            string PackageName = NameSplit[1];

            // CleanupTemp(Scope);
            byte[] ZipContent = DownloadZIP(Scope, PackageName, Package.version).Result;

            if (ZipContent != null)
            {
                // Might cause a ton of issues. its microsoft-software after all.
                //CreateTemp(Scope);
                string path = TempDirectory + PackageName + "@" + Package.version + ".zip.temp";
                await File.WriteAllBytesAsync(path, ZipContent);
                string PackageDirectory = PackagesDirectory + FolderName;
                Logger.Log(PackageDirectory);

                if (Directory.Exists(PackageDirectory))
                {
                    Directory.Delete(PackageDirectory, true);
                }


                Logger.Log("Extracting package zip");
                try
                {
                    ZipFile.ExtractToDirectory(path, PackageDirectory);
                }
                catch (Exception err)
                {
                    Logger.Log("Could not extract package zip: " + err, Logger.InfoType.Exception);
                    return CommandStatus.Failure;
                }

                // caution: this part is held together by duct tape.
                Logger.Log("Resolving dependencies");
                if (VersionManifest.dependencies != null)
                {
                    foreach (KeyValuePair<string, string> dependency in VersionManifest.dependencies)
                    {
                        Logger.Log("Installing dependency: " + dependency);

                        string DepenInfo = dependency.Value;

                        string JoinedVer = string.Join("*", DepenInfo);
                        string[] Splits = JoinedVer.Split("/");

                        string DepenFolderName = dependency.Key;

                        string DependencyScope = Splits[0];
                        string DependencyName = Splits[1];

                        int NameIndex = DependencyName.LastIndexOf("@");
                        if (NameIndex >= 0)
                        {
                            DependencyName = DependencyName.Substring(0, NameIndex);
                        }


                        // TODO: perhaps make this code cleaner somehow?
                        string Manifest = await Manifests.PullManifest(DependencyScope, DependencyName);
                        if (Manifest == null)
                        {
                            return CommandStatus.Failure;
                        }
                        string[] split = Manifest.Split('\n');

                        // perform json decode magic
                        ManifestResult ManifestVersions = JsonConvert.DeserializeObject<ManifestResult>(split[0]);

                        // Try install
                        PackageVersion LatestVersion = Manifests.ResolveToVersion(ManifestVersions, "latest");
                        Package latest = LatestVersion.package;
                        Logger.Log("Installing latest dependency version: " + latest.version);

                        CommandStatus Result = await InstallPackage(latest, LatestVersion, DepenFolderName);

                        if (Result == CommandStatus.Success)
                        {
                            Logger.Log("Successfully installed dependency: " + latest.name);
                        }
                        else
                        {
                            Logger.Log("Could not install dependency: " + latest.name, Logger.InfoType.Exception);
                        }
                    }

                    // Add disclaimer header
                    AddHeader(PackageDirectory);
                }

                foreach (var exclude in Package.exclude)
                {
                    Directory.Delete(PackageDirectory + "/" + exclude);
                };

                return CommandStatus.Success;
            }
            else
            {
                Console.WriteLine("Installing package failed, the zip file could not be downloaded. Log file dump inbound.");
                Logger.DumpLogs();
                return CommandStatus.Failure;
            }
        }

    }
}
