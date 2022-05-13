// pypupy - 2022

using System.Reflection;
using pypkg.Commands;

namespace pypkg
{
    public class Manager
    {
        private static readonly InstallCommand Install = new InstallCommand();
        // TODO: Remove DateTime once we're out of dev
        public static string pypkgVersion = "pypkg-" + DateTime.Now + "-dev";
        // The wally version we "mask" ourselves behind
        public static string WallyVersion = "1.0.0";
        public static readonly HttpClient HttpClient = new HttpClient();

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
        public static void Main(string [] args)
        {
            // The API requires every client to provide a wally-version... for some reason..?
            // Hope I don't get caught and be demolished to death
            HttpClient.DefaultRequestHeaders.Add("wally-version", WallyVersion);
            HttpClient.DefaultRequestHeaders.Add("pypkg-version", pypkgVersion);

            // Incase they do decide to demolish me

            HttpClient.DefaultRequestHeaders.Add("pypkg-abuse", "contact@shiroko.me");
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
                    await Install.Execute(CommandArgs);
                    break;
            }
        }
    }
}