using pypkg.Parsing;
using pypkg.Install;

namespace pypkg.Commands
{
    public class UninstallCommand : Command
    {
        public static string Name = "uninstall";
        private static readonly Uninstallator Deinstallator = new Uninstallator();
        
        public async Task<CommandStatus> Execute(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: pypkg uninstall <scope>/<name>");
                return CommandStatus.Failure;
            }

            ScopeName ParsedPackage = PackageParser.Parse(args[0]);

            Deinstallator.UninstallPackage(ParsedPackage.Name);
            return CommandStatus.Success;
        } 
    }
}
