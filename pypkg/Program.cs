// pypupy - 2022

using pypkg.Commands;
using System.Reflection;

namespace pypkg
{
    public class Manager
    {
        private static readonly InstallCommand InstallCommand = new InstallCommand();
        private static readonly UninstallCommand UninstallCommand = new UninstallCommand();
        private static readonly QueryCommand QueryCommand = new QueryCommand();

        // TODO: Remove DateTime once we're out of dev
        public static string pypkgVersion = "pypkg-" + DateTime.Now + "-dev";
        // The wally version we "mask" ourselves behind
        public static string WallyVersion = "1.0.0";
        public static readonly HttpClient HttpClient = new HttpClient();

        public static string Help = @"
              pypkg - a simple wally client
            
                Usage:
                
                pypkg install <scope>/<name> - Installs a package from the Wally registry
                pypkg uninstall <scope>/<name> - Attempts to uninstall a wally client.
                pypkg search/query <query> - Searches the wally registry for packages
        ";

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private static Type[] GetTypesInNamespace(string nameSpace)
        {
            return
              Assembly.GetExecutingAssembly().GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }

        // required to make main async:
        public static void Main(string[] args)
        {
            // The API requires every client to provide a wally-version... for some reason..?
            // Hope I don't get caught and be demolished to death
            HttpClient.DefaultRequestHeaders.Add("wally-version", WallyVersion);
            HttpClient.DefaultRequestHeaders.Add("pypkg-version", pypkgVersion);

            // Incase they do decide to demolish me

            HttpClient.DefaultRequestHeaders.Add("pypkg-github", "https://github.com/puyopy/pypkg");
            HttpClient.DefaultRequestHeaders.Add("pypkg-contact", "contact@shiroko.me");
            AsyncMain(args).GetAwaiter().GetResult();
        }

        public static async Task AsyncMain(string[] args)
        {
            string CommandName = args[0];
            string[] CommandArgs = args.Skip(1).ToArray();

            // awful way of doing commands, but it'll work for now
            switch (CommandName)
            {
                case "install":
                    await InstallCommand.Execute(CommandArgs);
                    break;
                case "uninstall":
                    await UninstallCommand.Execute(CommandArgs);
                    break;
                case "search":
                    await QueryCommand.Execute(CommandArgs);
                    break;
                case "query":
                    await QueryCommand.Execute(CommandArgs);
                    break;
                default:
                    Logger.Log(Help);
                    break;
            }
        }
    }
}