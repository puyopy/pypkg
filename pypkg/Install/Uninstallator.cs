using pypkg;
using pypkg.Commands;

namespace pypkg.Install
{
    public class Uninstallator
    {
        public async Task<CommandStatus> UninstallPackage(string PackageName)
        {
            string PackagesDir = Installator.PackagesDirectory;
            string PackagePath = PackagesDir + PackageName;
            bool DirectoryExists = Directory.Exists(PackagePath);

            if (DirectoryExists)
            {
                Logger.Log($"Uninstalling '{PackageName}' ");
                Directory.Delete(PackagePath, true);

                // TODO: resolve dependencies and remove them if no other package relies on it

                Logger.Log($"Package '{PackageName} has been successfully removed.");
                return CommandStatus.Success;
            } else
            {
                Logger.Log($"Package '{PackageName}' is not installed and thus cannot be removed.",Logger.InfoType.Exception);
                return CommandStatus.Failure;
            }
        }
    }
}
